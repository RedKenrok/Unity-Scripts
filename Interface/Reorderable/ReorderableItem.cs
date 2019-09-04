using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Interface.Reorderable {
	[RequireComponent(typeof(LayoutElement))]
	public class ReorderableItem : MonoBehaviour {
		#region Variables
			[SerializeField]
			private OnDragged _onDragged = default;
			
			private RectTransform _canvasRectTransform = default;
			private ReorderableList _reorderableList = default;
			private LayoutElement _layoutElement = default;
			
			private bool _isDragging = false;
			private Vector2 _dragOffset = default;
		#endregion
		
		#region MonoBehaviour functions
			private void Awake() {
				_canvasRectTransform = GetComponentInParent<Canvas>().GetComponent<RectTransform>();
				_reorderableList = GetComponentInParent<ReorderableList>();
				_layoutElement = GetComponent<LayoutElement>();
			}
			
			private void OnEnable() {
				_onDragged.onPointerDown.AddListener(OnPointerDown);
				_onDragged.onDragStart.AddListener(OnDragStart);
				_onDragged.onDrag.AddListener(OnDrag);
				_onDragged.onDragEnd.AddListener(OnDragEnd);
			}
			private void OnDisable() {
				_onDragged.onPointerDown.RemoveListener(OnPointerDown);
				_onDragged.onDragStart.RemoveListener(OnDragStart);
				_onDragged.onDrag.RemoveListener(OnDrag);
				_onDragged.onDragEnd.RemoveListener(OnDragEnd);
			}
		#endregion
		
		#region Event handlers
			private void OnPointerDown(PointerEventData _pointerData) {
				// Calculate drag offset.
				_dragOffset = RectTransformUtility.WorldToScreenPoint(_reorderableList.camera, transform.position) - _pointerData.position;
			}
			private void OnDragStart(PointerEventData _pointerData) {
				// Prevent multiple elements to be dragged at once.
				if (_reorderableList.isDragging) {
					return;
				}
				_reorderableList.isDragging = _isDragging = true;
				
				// Take element out of list.
				_layoutElement.ignoreLayout = true;
				transform.SetParent(_canvasRectTransform);
				
				// Set transform position.
				transform.position = GetDragPosition(_pointerData.position + _dragOffset);
				
				// Set placement indicator.
				_reorderableList.UpdateIndicatorPosition(_pointerData.position + _dragOffset);
			}
			private void OnDrag(PointerEventData _pointerData) {
				if (!_isDragging) {
					return;
				}
				
				// Set transform position.
				transform.position = GetDragPosition(_pointerData.position + _dragOffset);
				
				// Set placement indicator.
				_reorderableList.UpdateIndicatorPosition(_pointerData.position + _dragOffset);
			}
			private void OnDragEnd(PointerEventData _pointerData) {
				if (!_isDragging) {
					return;
				}
				
				// Add element back to list.
				int _siblingIndex = _reorderableList.GetSiblingIndex(_pointerData.position + _dragOffset);
				transform.SetParent(_reorderableList.transform);
				transform.SetSiblingIndex(_siblingIndex);
				_layoutElement.ignoreLayout = false;
				
				_reorderableList.isDragging = _isDragging = false;
			}
		#endregion
		
		#region Private functions
			private Vector3 GetDragPosition(Vector2 _screenPosition) {
				Vector3 _position;
				RectTransformUtility.ScreenPointToWorldPointInRectangle(_canvasRectTransform, _screenPosition, _reorderableList.camera, out _position);
				return _position;
			}
		#endregion
	}
}