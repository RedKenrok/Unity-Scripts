using UnityEngine;

namespace Animation {
	public interface ISimpleAnimation {
		/// <summary>Setup animation, retrieve components of the game object.</summary>
		/// <param name="_gameObject"></param>
		void OnStart(GameObject _gameObject);
		/// <summary>Animating, update the values on the game object.</summary>
		/// <param name="_step"></param>
		/// <returns>Whether the animation has finished early.</returns>
		bool OnUpdate(float _step);
		/// <summary>Animating finished, dispose of any memory.</summary>
		void OnComplete();
	}
}