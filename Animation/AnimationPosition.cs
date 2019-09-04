using UnityEngine;

namespace Animation {
	public class AnimationPosition : MonoBehaviour, ISimpleAnimation {
		#region Variables
			[SerializeField]
			public bool useStart = false;
			[SerializeField]
			public Vector3 start = Vector3.zero;
			[SerializeField]
			public Vector3 end = Vector3.zero;
			
			[SerializeField]
			public AnimationCurve curve = AnimationCurve.EaseInOut(0, 0, 1, 1);
			
			[SerializeField]
			public bool useLocalSpace = false;
			
			private Transform _transform = default;
			private Vector3 _startTemp = default;
		#endregion
		
		#region Public functions
			public void OnStart(GameObject gameObject) {
				// Get components.
				_transform = gameObject.transform;
				
				_startTemp = useStart ? start : (useLocalSpace ? _transform.localPosition : _transform.position);
			}
			public bool OnUpdate(float step) {
				// Update values on components.
				if (useLocalSpace) {
					_transform.localPosition = Vector3.LerpUnclamped(_startTemp, end, curve.Evaluate(step));
				} else {
					_transform.position = Vector3.LerpUnclamped(_startTemp, end, curve.Evaluate(step));
				}
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