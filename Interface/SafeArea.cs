using UnityEngine;

namespace Interface {
	/// <summary>In the rect transform set: anchorMin to 0,0; anchorMax to 1,1; left, right, bottom, top to 0;</summary>
	[RequireComponent(typeof(RectTransform))]
	public class SafeArea : MonoBehaviour {
		#region Variables
			private RectTransform _rectTransform;
			private Canvas _canvas;
		#endregion
		
		#region MonoBehaviour functions
			private void Awake() {
				_rectTransform = GetComponent<RectTransform>();
				_canvas = GetComponentInParent<Canvas>();
			}
			
			private void OnEnable() {
				// Setup the safe area.
				OnChangeResolution(new Vector2Int(Screen.width, Screen.height));
				// You want to call that function everytime the screens resolution changes.
				// If you have the Device.Resolution script included you can do:
				// Device.Resolution.onChange += OnChangeResolution.
				// Then in OnDisable do not forget to call:
				// Device.Resolution.onChange -= OnChangeResolution.
			}
		#endregion
		
		#region Public functions
			public void OnChangeResolution(Vector2Int _resolution) {
				Rect _safeArea = Screen.safeArea;
				
				Vector2 _anchorMin = _safeArea.position;
				_anchorMin.x /= _canvas.pixelRect.width;
				_anchorMin.y /= _canvas.pixelRect.height;
				_rectTransform.anchorMin = _anchorMin;
				
				Vector2 _anchorMax = _safeArea.position + _safeArea.size;
				_anchorMax.x /= _canvas.pixelRect.width;
				_anchorMax.y /= _canvas.pixelRect.height;
				_rectTransform.anchorMax = _anchorMax;
			}
		#endregion
	}
}