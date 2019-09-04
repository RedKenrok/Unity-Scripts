using UnityEngine;
using UnityEngine.UI;

namespace Interface.Reorderable {
	[RequireComponent(typeof(VerticalLayoutGroup))]
	public class ReorderableList : MonoBehaviour {
		#region Variables
			[SerializeField]
			private RectTransform _indicator = default;
			
			new public Camera camera {
				get {
					return _canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : _canvas.worldCamera;
				}
			}
			private Canvas _canvas = default;
			private VerticalLayoutGroup _layoutGroup = default;
			
			private bool _isDragging = false;
			public bool isDragging {
				get {
					return _isDragging;
				}
				set {
					if (_isDragging == value) {
						return;
					}
					
					_isDragging = value;
					
					// Activate placement indicator and set as last item in list.
					_indicator.gameObject.SetActive(value);
				}
			}
		#endregion
		
		#region MonoBehaviour functions
			private void Awake() {
				_canvas = GetComponentInParent<Canvas>();
				
				_layoutGroup = GetComponent<VerticalLayoutGroup>();
			}
		#endregion
		
		#region Public functions
			public void UpdateIndicatorPosition(Vector2 _screenPosition) {
				UpdateIndicatorPosition(GetSiblingIndex(_screenPosition));
			}
			public void UpdateIndicatorPosition(int _siblingIndex) {
				if (transform.childCount == 0 || (transform.childCount == 1 && transform.GetChild(0) == _indicator.transform)) {
					_indicator.anchoredPosition = new Vector2(_indicator.anchoredPosition.x, _layoutGroup.spacing / 2f);
					return;
				}
				
				if (_siblingIndex < transform.childCount) {
					RectTransform _child = transform.GetChild(_siblingIndex).GetComponent<RectTransform>();
					_indicator.anchoredPosition = new Vector2(_indicator.anchoredPosition.x, _child.anchoredPosition.y + (_layoutGroup.spacing / 2f));
				} else {
					RectTransform _child = transform.GetChild(transform.childCount - 1).GetComponent<RectTransform>();
					_indicator.anchoredPosition = new Vector2(_indicator.anchoredPosition.x, (_child.anchoredPosition.y - _child.sizeDelta.y) - (_layoutGroup.spacing / 2f));
				}
			}
			
			public int GetSiblingIndex(Vector2 _screenPosition) {
				int _indicatorIndex = _indicator.GetSiblingIndex();
				RectTransform _child;
				for (int _i = 0; _i < transform.childCount; _i++) {
					// Skip indicator.
					if (_i == _indicatorIndex) {
						continue;
					}
					
					// Get child rect transform.
					_child = transform.GetChild(_i).GetComponent<RectTransform>();
					// Check relative height.
					if (RectTransformUtility.WorldToScreenPoint(camera, _child.position).y < _screenPosition.y) {
						return _i;
					}
				}
				
				return transform.childCount;
			}
		#endregion
	}
}