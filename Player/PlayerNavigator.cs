using UnityEngine;
using UnityEngine.AI;

namespace Player {
	[RequireComponent(typeof(NavMeshAgent), typeof(Rigidbody))]
	public class PlayerNavigator : MonoBehaviour {
		#region Variables
			[SerializeField] [Range(0f, 8f)]
			public float _speed = 2f;
			[SerializeField]
			private string _areaMaskName = "Ground";
			[SerializeField]
			private Camera _camera = default;
			
			private NavMeshAgent _navigationAgent = default;
			private Rigidbody _rigidbody = default;
			
			private LayerMask _layerMask = default;
			private float _slopeMaxHeight = 0;
			private Vector2? _input = default;
			
			private NavMeshHit _navigationHitTemp = default;
			private float _distanceTemp = default;
			private Vector3 _offsetPositionTemp = default;
		#endregion
		
		#region MonoBehaviour functions
			private void Awake() {
				_navigationAgent = GetComponent<NavMeshAgent>();
				_navigationAgent.updateRotation = false;
				_navigationAgent.updateUpAxis = false;
				_rigidbody = GetComponent<Rigidbody>();
				
				_layerMask = LayerMask.NameToLayer(_areaMaskName);
				_slopeMaxHeight = Mathf.Sin(Mathf.Deg2Rad * NavMesh.GetSettingsByID(_navigationAgent.agentTypeID).agentSlope) * 2f;
			}
			
			private void OnEnable() {
				// Place on navigation surface.
				if (NavMesh.SamplePosition(transform.position, out _navigationHitTemp, 16f, NavMesh.AllAreas)) {
					_navigationAgent.Warp(_navigationHitTemp.position);
				}
			}
			
			private void FixedUpdate() {
				if (!_input.HasValue) {
					return;
				}
				
				// Calculate the offset position.
				_offsetPositionTemp = _navigationAgent.speed * _speed * Time.fixedDeltaTime * (CalculateRotationY(_camera.transform.rotation) * new Vector3(_input.Value.x, 0, _input.Value.y));
				
				if (!NavMesh.SamplePosition(transform.position + _offsetPositionTemp, out _navigationHitTemp, _offsetPositionTemp.magnitude, NavMesh.AllAreas)) {
					return;
				}
				
				// Calculate the distance between the agent position and the hit position.
				_distanceTemp = Vector2.Distance(transform.position, _navigationHitTemp.position);
				
				// Check if position is out of range in the horizontal plane or y axis.
				if (Mathf.Abs(_navigationHitTemp.position.y - transform.position.y) > _slopeMaxHeight * _distanceTemp) {
					return;
				}
				
				_rigidbody.MovePosition(_navigationHitTemp.position);
				_navigationAgent.SetDestination(transform.position);
			}
		#endregion
		
		#region Public functions
			public void Stop() {
				_navigationAgent.SetDestination(transform.position);
				_input = default;
			}
			
			public void MoveStart() {
				_navigationAgent.enabled = false;
			}
			public void Move(Vector2 _position) {
				_input = _position;
			}
			public void MoveCanceled() {
				_input = default;
				
				_navigationAgent.enabled = true;
				_navigationAgent.SetDestination(transform.position);
			}
			
			public void SetDestination(Transform _transform, Vector3 _position) {
				if (_layerMask.Contains(_transform.gameObject.layer)) {
					return;
				}
				
				if (NavMesh.SamplePosition(_position, out _navigationHitTemp, 10f, NavMesh.AllAreas)) {
					_input = default;
					_navigationAgent.SetDestination(_navigationHitTemp.position);
				}
			}
		#endregion
		
		#region Private functions
			private static Quaternion CalculateRotationY(Quaternion quaternion) {
				float theta = Mathf.Atan2(quaternion.y, quaternion.w);
				return new Quaternion(0, Mathf.Sin(theta), 0, Mathf.Cos(theta));
			}
		#endregion
	}
}