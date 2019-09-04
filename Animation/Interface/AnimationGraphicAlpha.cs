using UnityEngine;
using UnityEngine.UI;

namespace Animation.Interface {
	public class AnimationGraphicAlpha : MonoBehaviour, ISimpleAnimation {
		#region Variables
			[SerializeField]
			public bool useStart = false;
			[SerializeField]
			public float start = 0f;
			[SerializeField]
			public float end = 1f;
			[SerializeField]
			public AnimationCurve curve = AnimationCurve.EaseInOut(0, 0, 1, 1);
			
			private Graphic _graphic = default;
			private float _startTemp = default;
		#endregion
		
		#region Public functions
			public void OnStart(GameObject _gameObject) {
				// Get components.
				_graphic = _gameObject.GetComponent<Graphic>();
				
				if (_graphic == default) {
					Log.ErrorFormat("A graphic is required when animating a alpha. To solve this add a reference to the object with the SimpleAnimator component.");
					return;
				}
				
				_startTemp = useStart ? start : _graphic.color.a;
			}
			public bool OnUpdate(float _step) {
				// Update values on components.
				_graphic.color = new Color(_graphic.color.r, _graphic.color.g, _graphic.color.b, Mathf.Lerp(_startTemp, end, curve.Evaluate(_step)));
				return false;
			}
			public void OnComplete() {
				// Dispose of memory.
				_graphic = default;
				_startTemp = default;
			}
		#endregion
	}
}