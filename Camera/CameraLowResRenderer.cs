using UnityEngine;
using UnityEngine.Rendering;

namespace Camera {
	/// <summary>Only works with Unity's new scriptable render pipeline.</summary>
	[RequireComponent(typeof(UnityEngine.Camera))]
	public class CameraLowResRenderer : MonoBehaviour {
		#region Variables
			[SerializeField] [Tooltip("The least amount of pixels there needs to be on the side of the screen with the lowest resolution.")]
			private int _shortSidePixelSize = 240;
			[SerializeField] [Tooltip("The filter mode of the render texture. FilterMode.Point creates a pixelated look.")]
			private FilterMode _filterMode = FilterMode.Point;
			// You might want to add more render texture settings here.
			
			private UnityEngine.Camera _camera = default;
			private RenderTexture _renderTexture = default;
		#endregion
		
		#region MonoBehaviour functions
			private void Awake() {
				_camera = GetComponent<UnityEngine.Camera>();
			}
			
			private void OnEnable() {
				// Setup the render texture.
				OnChangeResolution(new Vector2Int(Screen.width, Screen.height));
				// You want to call that function everytime the screens resolution changes.
				// If you have the Device.Resolution script included you can do:
				// Device.Resolution.onChange += OnChangeResolution.
				// Then in On Disable do not forget to call:
				// Device.Resolution.onChange -= OnChangeResolution.
				
				// Listen to the render events.
				RenderPipelineManager.beginCameraRendering += OnBeginRendering;
				RenderPipelineManager.endCameraRendering += OnEndRendering;
			}
			private void OnDisable() {
				// Stop listening to the render events.
				RenderPipelineManager.beginCameraRendering -= OnBeginRendering;
				RenderPipelineManager.endCameraRendering -= OnEndRendering;
			}
			
			private void OnBeginRendering(ScriptableRenderContext _context, UnityEngine.Camera _camera) {
				if (_camera != this._camera) {
					return;
				}
				
				_camera.targetTexture = _renderTexture;
			}
			private void OnEndRendering(ScriptableRenderContext _context, UnityEngine.Camera _camera) {
				if (_camera != this._camera) {
					return;
				}
				
				_camera.targetTexture = null;
				Graphics.Blit(_renderTexture, null as RenderTexture);
			}
		#endregion
		
		#region Public functions
			public void OnChangeResolution(Vector2Int _resolution) {
				// Calculate render texture resolution.
				Vector2Int _renderResolution;
				if (_resolution.y > _resolution.x) {
					_renderResolution = new Vector2Int(
						_shortSidePixelSize,
						Mathf.RoundToInt(_shortSidePixelSize * ((float)_resolution.y / _resolution.x))
					);
				} else {
					_renderResolution = new Vector2Int(
						Mathf.RoundToInt(_shortSidePixelSize * ((float)_resolution.x / _resolution.y)),
						_shortSidePixelSize
					);
				}
				
				// Setup new render texture.
				_renderTexture = new RenderTexture(_renderResolution.x, _renderResolution.y, 16);
				_renderTexture.filterMode = _filterMode;
			}
		#endregion
	}
}