using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Interface.Reorderable {
	[RequireComponent(typeof(RectTransform))]
	public class OnDragged : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler, IEndDragHandler {
		#region Classes
			[Serializable]
			public class EventPointerData : UnityEvent<PointerEventData> {}
		#endregion
		
		#region Variables
			[SerializeField]
			public EventPointerData onPointerDown = default;
			
			[SerializeField]
			public EventPointerData onPointerEnter = default;
			[SerializeField]
			public EventPointerData onPointerExit = default;
			
			[SerializeField]
			public EventPointerData onDragStart = default;
			[SerializeField]
			public EventPointerData onDrag = default;
			[SerializeField]
			public EventPointerData onDragEnd = default;
			
			private RectTransform _rectTransform = default;
		#endregion
		
		#region MonoBehaviour functions
			private void Awake() {
				_rectTransform = GetComponent<RectTransform>();
			}
		#endregion
		
		#region Event handlers
			public void OnPointerDown(PointerEventData _pointerData) {
				onPointerDown.Invoke(_pointerData);
			}
			
			public void OnPointerEnter(PointerEventData _pointerData) {
				onPointerEnter.Invoke(_pointerData);
			}
			public void OnPointerExit(PointerEventData _pointerData) {
				onPointerExit.Invoke(_pointerData);
			}
			
			public void OnBeginDrag(PointerEventData _pointerData) {
				onDragStart.Invoke(_pointerData);
			}
			public void OnDrag(PointerEventData _pointerData) {
				onDrag.Invoke(_pointerData);
			}
			public void OnEndDrag(PointerEventData _pointerData) {
				onDragEnd.Invoke(_pointerData);
			}
		#endregion
	}
}