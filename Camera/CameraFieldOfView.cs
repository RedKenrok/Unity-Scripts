using UnityEngine;

namespace Camera {
	[RequireComponent(typeof(UnityEngine.Camera))]
	public class CameraFieldOfView : MonoBehaviour {
		#region Variables
			private UnityEngine.Camera _camera = default;
			private float _fieldOfView = default;
		#endregion
		
		#region MonoBehaviour functions
			private void Awake() {
				_camera = GetComponent<UnityEngine.Camera>();
				_fieldOfView = _camera.fieldOfView;
			}
			
			private void OnEnable() {
				// Setup field of view.
				OnChangeResolution(new Vector2Int(Screen.width, Screen.height));
				// You want to call that function everytime the screens resolution changes.
				// If you have the Device.Resolution script included you can do:
				// Device.Resolution.onChange += OnChangeResolution.
				// Then in On Disable do not forget to call:
				// Device.Resolution.onChange -= OnChangeResolution.
			}
		#endregion
		
		#region Public functions
			public void OnChangeResolution(Vector2Int _resolution) {
				// Calculate field of view.
				SetFieldOfView(_fieldOfView, _resolution);
			}
			public void SetFieldOfView(float _fieldOfView, Vector2Int _resolution) {
				// Makes sure that the the field of view axis is the axis with the smallest resolution.
				
				// If vertical resolution is larger than horizontal resolution.
				if (_resolution.y > _resolution.x) {
					// Then convert field of view degrees from vertical to horizontal with inverted aspect ratio.
					_fieldOfView = UnityEngine.Camera.VerticalToHorizontalFieldOfView(_fieldOfView, (float)_resolution.y / (float)_resolution.x);
				}
				
				// Assign field of view to camera.
				_camera.fieldOfView = _fieldOfView;
			}
			public void SetFieldOfView(float _fieldOfView) {
				if (_camera.targetTexture != default) {
					SetFieldOfView(_fieldOfView, new Vector2Int(_camera.targetTexture.width, _camera.targetTexture.height));
					return;
				}
				
				SetFieldOfView(_fieldOfView, new Vector2Int(Screen.width, Screen.height));
			}
		#endregion
	}
}