using UnityEngine;

namespace Animation {
	public class AnimationRotation : MonoBehaviour, ISimpleAnimation {
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
			private Quaternion _startTemp = default;
			private Quaternion _endTemp = default;
		#endregion
		
		#region Public functions
			public void OnStart(GameObject gameObject) {
				// Get components.
				_transform = gameObject.transform;
				
				_startTemp = useStart ? Quaternion.Euler(start) : (useLocalSpace ? _transform.localRotation : _transform.rotation);
				_endTemp = Quaternion.Euler(end);
			}
			public bool OnUpdate(float step) {
				// Update values on components.
				if (useLocalSpace) {
					_transform.localRotation = Quaternion.LerpUnclamped(_startTemp, _endTemp, curve.Evaluate(step));
				} else {
					_transform.rotation = Quaternion.LerpUnclamped(_startTemp, _endTemp, curve.Evaluate(step));
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