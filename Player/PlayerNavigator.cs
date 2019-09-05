using UnityEngine;
using UnityEngine.AI;

namespace Player {
	/// <summary>For a detailed explanation of this class see the article found in [_Articles/PlayerNavigator.md](https://github.com/RedKenrok/Unity-Scripts/blob/master/_Articles/PlayerNavigator.md)</summary>
	[RequireComponent(typeof(NavMeshAgent), typeof(Rigidbody))]
	public class PlayerNavigator : MonoBehaviour {
		#region Variables
			[SerializeField] [Range(0f, 8f)] [Tooltip("Additional movement speed when using the Move function.")]
			public float _speed = 2f;
			[SerializeField] [Tooltip("The name of the walkable area.")]
			private string _areaMaskName = "Walkable";
			[SerializeField] [Tooltip("The layers of the raycastable and walkable surface.")]
			private LayerMask _layerMask = default;
			
			private Camera _camera = default;
			private NavMeshAgent _navigationAgent = default;
			private Rigidbody _rigidbody = default;
			
			private int _areaMask = default;
			private float _slopeMaxHeight = 0;
			private Vector2? _input = default;
			
			private NavMeshHit _navigationHitTemp = default;
			private Vector3 _offsetPositionTemp = default;
		#endregion
		
		#region MonoBehaviour functions
			private void Awake() {
				_camera = GetComponentInChildren<Camera>();
				_navigationAgent = GetComponent<NavMeshAgent>();
				_navigationAgent.updateRotation = false;
				_rigidbody = GetComponent<Rigidbody>();
				
				_areaMask = 1 << NavMesh.GetAreaFromName(_areaMaskName);
				_slopeMaxHeight = Mathf.Sin(Mathf.Deg2Rad * NavMesh.GetSettingsByID(_navigationAgent.agentTypeID).agentSlope) * 2f;
			}
			
			private void OnEnable() {
				// Place object on navigation surface.
				if (NavMesh.SamplePosition(transform.position, out _navigationHitTemp, 16f, _areaMask)) {
					_navigationAgent.Warp(_navigationHitTemp.position);
				}
			}
			
			private void FixedUpdate() {
				// Return early if there is no input value set.
				if (!_input.HasValue) {
					return;
				}
				
				// Calculate the offset position.
				_offsetPositionTemp = (_navigationAgent.speed * _speed * Time.fixedDeltaTime) * (CalculateRotationY(_camera.transform.rotation) * new Vector3(_input.Value.x, 0, _input.Value.y));
				
				// Sample the nearest position, return early if none found.
				if (!NavMesh.SamplePosition(transform.position + _offsetPositionTemp, out _navigationHitTemp, _offsetPositionTemp.magnitude, _areaMask)) {
					return;
				}
				
				// Check if position is out of range in y axis.
				if (Mathf.Abs(_navigationHitTemp.position.y - transform.position.y) > _slopeMaxHeight * Vector2.Distance(transform.position, _navigationHitTemp.position)) {
					return;
				}
				
				// Apply new position to rigidbody and navigation agent.
				_rigidbody.MovePosition(_navigationHitTemp.position);
				_navigationAgent.SetDestination(transform.position);
			}
		#endregion
		
		#region Public functions
			/// <summary> Stop the agent from moving.</summary>
			public void Stop() {
				// Reset position.
				_input = default;
				_navigationAgent.SetDestination(transform.position);
			}
			
			/// <summary>Set the agents destination</summary>
			/// <param name="_transform">The transform of the game object hit with the raycast.</param>
			/// <param name="_position">The raycast hit position.</param>
			public void SetDestination(Transform _transform, Vector3 _position) {
				// Check if the layer of the game object is valid.
				if (!_layerMask.Contains(_transform.gameObject.layer)) {
					return;
				}
				
				// Sample the nearest position.
				if (NavMesh.SamplePosition(_position, out _navigationHitTemp, _navigationAgent.stoppingDistance * 64, _areaMask)) {
					_input = default;
					_navigationAgent.SetDestination(_navigationHitTemp.position);
				}
			}
			
			/// <summary>Set the move vector.</summary>
			/// <param name="_input">Input vector.</param>
			private void Move(Vector2 _input) {
				// Override the current input.
				this._input = _input;
			}
			/// <summary>Reset values when moving is done.</summary>
			private void MoveCanceled() {
				// Reset input.
				_input = default;
			}
		#endregion
		
		#region Private functions
			/// <summary>Check whether the layer is contained in the mask.</summary>
			/// <param name="_mask">The layer mask.</param>
			/// <param name="_layer">The index of the layer.</param>
			/// <returns>True if layer is contained in mask.</returns>
			private static bool Contains(this LayerMask _mask, int _layer) {
				return (_mask & 1 << _layer) > 0;
			}
			
			/// <summary>Get the rotation on y-axis of quaternion.</summary>
			/// <param name="quaternion">The rotation.</param>
			/// <returns>The angle offset on the y-axis.</returns>
			private static Quaternion CalculateRotationY(Quaternion quaternion) {
				float theta = Mathf.Atan2(quaternion.y, quaternion.w);
				return new Quaternion(0, Mathf.Sin(theta), 0, Mathf.Cos(theta));
			}
		#endregion
	}
}