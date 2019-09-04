using UnityEngine;

namespace Animation {
	public class AnimationScale : MonoBehaviour, ISimpleAnimation {
		#region Variables
			[SerializeField]
			public bool useStart = false;
			[SerializeField] [Tooltip("Local scale only.")]
			public Vector3 start = Vector3.zero;
			[SerializeField] [Tooltip("Local scale only.")]
			public Vector3 end = Vector3.zero;
			
			[SerializeField]
			public AnimationCurve curve = AnimationCurve.EaseInOut(0, 0, 1, 1);
			
			private Transform _transform = default;
			private Vector3 _startTemp = default;
		#endregion
		
		#region Public functions
			public void OnStart(GameObject gameObject) {
				// Get components.
				_transform = gameObject.GetComponent<Transform>();
				
				_startTemp = useStart ? start : _transform.localScale;
			}
			public bool OnUpdate(float step) {
				// Update values on components.
				_transform.localScale = Vector3.LerpUnclamped(_startTemp, end, curve.Evaluate(step));
				return false;
			}
			public void OnComplete() {
				// Dispose of memory.
				_transform = default;
				_startTemp = default;
			}
		#endregion
	}
}