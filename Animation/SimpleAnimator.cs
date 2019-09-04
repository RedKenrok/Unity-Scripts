using System;
using System.Collections;

using UnityEngine;
using UnityEngine.Events;

// Do note this script uses the UniRx reactive programming library!
using UniRx;

namespace Animation {
	public class SimpleAnimator : MonoBehaviour {
		#region Variables
			[SerializeField] [Tooltip("If not set, automatically grabs this game object.")]
			public GameObject animatedObject;
			
			[SerializeField] [Tooltip("Whether to start animating when the component is enabled.")]
			public bool playOnEnable = false;
			[SerializeField] [Tooltip("Whether to finish the animation in instant when cancel is called.")]
			public bool finishOnCancel = false;
			[SerializeField] [Tooltip("Delay before animating starts in seconds.")]
			public float delay = 0f;
			[SerializeField] [Tooltip("The maximum time the animation takes in seconds.")]
			public float duration = 1f;
			[SerializeField] [Tooltip("Invoked when play is invoked.")]
			public UnityEvent onInvoke = default;
			[SerializeField] [Tooltip("Invoked after the delay has passed.")]
			public UnityEvent onStart = default;
			[SerializeField] [Tooltip("Invoked after the animations are done.")]
			public UnityEvent onComplete = default;
			
			private ISimpleAnimation[] _animations = default;
			
			private IDisposable _animateRoutine = default;
			private bool delayDone = true;
			private Action _callback = default;
		#endregion
		
		#region MonoBehaviour functions
			private void OnEnable() {
				if (playOnEnable) {
					Play();
				}
			}
		#endregion
		
		#region Private functions
			private IEnumerator Animate() {
				delayDone = false;
				
				if (onInvoke != default) {
					onInvoke.Invoke();
				}
				
				if (delay > 0) {
					yield return new WaitForSeconds(delay);
				}
				delayDone = true;
				
				if (onStart != default) {
					onStart.Invoke();
				}
				
				GameObject _animatedObject = animatedObject ? animatedObject : gameObject;
				// Call on start on each animation.
				foreach(ISimpleAnimation _animation in _animations) {
					_animation.OnStart(animatedObject);
				}
				
				// Store animation start time and iterate until finished.
				float _timeStart = Time.time,
					_timeStep;
				bool[] _animationsFinished = new bool[_animations.Length];
				bool _continueLoop = true;
				// Stop looping when all animations are done or the duration time has passed.
				while(_continueLoop && Time.time - _timeStart < duration) {
					// Calculate percentage of duration passed.
					_timeStep = (Time.time - _timeStart) / duration;
					
					// Set continue loop to false.
					_continueLoop = false;
					
					// Call on update on each animation.
					for (int i = 0; i < _animations.Length; i++) {
						// If animation is complete ignore it.
						if (_animationsFinished[i]) {
							continue;
						}
						// Update animation and store result.
						_animationsFinished[i] = _animations[i].OnUpdate(_timeStep);
						
						// If any animation is not finished then continue the loop.
						if (!_animationsFinished[i]) {
							_continueLoop = true;
						}
					}
					
					// Wait for next frame.
					yield return null;
				}
				
				// Call on update on each animation.
				foreach(ISimpleAnimation animation in _animations) {
					animation.OnUpdate(1);
					animation.OnComplete();
				}
				
				if (onComplete != default) {
					onComplete.Invoke();
				}
			}
		#endregion
		
		#region Public functions
			public void Play() {
				if (_animateRoutine != default) {
					_animateRoutine.Dispose();
				}
				
				_animations = GetComponents<ISimpleAnimation>();
				
				_animateRoutine = Observable.FromCoroutine(Animate).Subscribe(_ => {
					if (_animateRoutine != default) {
						_animateRoutine.Dispose();
						_animateRoutine = default;
					}
				});
			}
			public void Play(Action _callback) {
				if (_animateRoutine != default) {
					_animateRoutine.Dispose();
				}
				
				_animations = GetComponents<ISimpleAnimation>();
				
				this._callback = _callback;
				_animateRoutine = Observable.FromCoroutine(Animate).Subscribe(_ => {
					if (_animateRoutine != default) {
						_animateRoutine.Dispose();
						_animateRoutine = default;
					}
					
					this._callback.Invoke();
					this._callback = default;
				});
			}
			
			public void Cancel() {
				if (_animateRoutine == default) {
					return;
				}
				
				// Dispose of routine.
				_animateRoutine.Dispose();
				
				if (finishOnCancel) {
					// Finish event keypoints as if every thing happend instantly.
					GameObject _animatedObject = animatedObject ? animatedObject : gameObject;
					
					// Skip code between starting the delay and animation update calls.
					if (!delayDone) {
						if (onStart != default) {
							onStart.Invoke();
						}
						// Call start on each animation.
						foreach(ISimpleAnimation _animation in _animations) {
							_animation.OnStart(_animatedObject);
						}
					}
					
					// Call update and complete on each animation.
					foreach(ISimpleAnimation _animation in _animations) {
						_animation.OnUpdate(1);
						_animation.OnComplete();
					}
					
					if (onComplete != default) {
						onComplete.Invoke();
					}
					
					if (_callback != default) {
						_callback.Invoke();
						_callback = default;
					}
				} else if (delayDone) {
					// Call update and complete on each animation.
					foreach(ISimpleAnimation _animation in _animations) {
						_animation.OnComplete();
					}
				}
			}
		#endregion
	}
}