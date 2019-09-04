using System;
using System.Collections;
using System.ComponentModel;

using UnityEngine;

// Do note this script uses the UniRx reactive programming library!
using UniRx;

namespace Device {
	public static class Resolution {
		#region Variables
			private static IDisposable resolutionRoutine;
			private static Vector2Int? _resolution = default;
			public static Vector2Int resolution {
				get {
					if (!_resolution.HasValue) {
						_resolution = GetResolution();
						
						if (resolutionRoutine == default) {
							resolutionRoutine = Observable.FromMicroCoroutine(DetectResolutionChange).Subscribe();
						}
					}
					
					return _resolution.Value;
				}
			}
			private static event Action<Vector2Int> _onChange = default;
			public static Action<Vector2Int> onChange {
				get {
					if (resolutionRoutine == default) {
						resolutionRoutine = Observable.FromMicroCoroutine(DetectResolutionChange).Subscribe();
					}
					
					return _onChange;
				}
				set {
					_onChange = value;
				}
			}
		#endregion
		
		#region Private functions
			private static Vector2Int GetResolution() {
				return new Vector2Int(Screen.width, Screen.height);
			}
			private static IEnumerator DetectResolutionChange() {
				Vector2Int newResolution;
				
				// Loop forever.
				while(true) {
					// Get current resolution.
					newResolution = GetResolution();
					// Compare know and current resolution.
					if (newResolution != resolution) {
						// If different overwrite known, and invoke action.
						_resolution = newResolution;
						if (onChange != default) {
							onChange.Invoke(resolution);
						}
					}
					
					// Wait for next frame.
					yield return null;
				}
			}
		#endregion
	}
}