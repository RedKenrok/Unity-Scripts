using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Interface.Selectable {
	public abstract class SelectionItem<T> : MonoBehaviour, IPointerClickHandler {
		#region Classes
			[Serializable]
			public class EventTransform : UnityEvent<Transform> {}
		#endregion
		
		#region Variables
			[SerializeField]
			public EventTransform onClick = default;
			
			[SerializeField]
			public UnityEvent onSelected = default;
			[SerializeField]
			public UnityEvent onDeselected = default;
		#endregion
		
		#region Event handlers
			public void OnPointerClick(PointerEventData _pointerData) {
				onClick.Invoke(transform);
			}
		#endregion
		
		#region Public functions
			public virtual void SetData(T _data) {}
		#endregion
	}
}