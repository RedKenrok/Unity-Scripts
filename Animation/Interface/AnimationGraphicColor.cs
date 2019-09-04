using UnityEngine;
using UnityEngine.UI;

namespace Animation.Interface {
	public class AnimationGraphicColor : MonoBehaviour, ISimpleAnimation {
		#region Variables
			[SerializeField]
			public bool useStart = false;
			[SerializeField]
			public Color start = new Color(1f, 1f, 1f, 1f);
			[SerializeField]
			public Color end = new Color(0f, 0f, 0f, 0f);
			[SerializeField]
			public AnimationCurve curve = AnimationCurve.EaseInOut(0, 0, 1, 1);
			
			private Graphic _graphic = default;
			private Color _startTemp = default;
		#endregion
		
		#region Public functions
			public void OnStart(GameObject _gameObject) {
				// Get components.
				_graphic = _gameObject.GetComponent<Graphic>();
				
				if (_graphic == default) {
					Log.ErrorFormat("A graphic is required when animating a color. To solve this add a reference to the object with the SimpleAnimator component.");
					return;
				}
				
				_startTemp = useStart ? start : _graphic.color;
			}
			public bool OnUpdate(float _step) {
				// Update values on components.
				_graphic.color = Color.Lerp(_startTemp, end, curve.Evaluate(_step));
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