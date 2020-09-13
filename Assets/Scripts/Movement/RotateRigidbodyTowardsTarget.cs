using Characters;
using UnityEngine;
using UnityUtilities;
using Validation;

namespace Movement
{
	public sealed class RotateRigidbodyTowardsTarget : MonoBehaviour
	{
		[SerializeField, Min(0f)] private float _angularSpeed = 360f;
		[SerializeField] private Transform _lookAt = default;
	
		private void FixedUpdate()
		{
			if (TooClose) return;

			var deltaRotation = _angularSpeed * Time.fixedDeltaTime;
			var newRotation = Quaternion.RotateTowards(_rigidbody.rotation, TargetRotation, deltaRotation);
		
			_rigidbody.rotation = newRotation;
		}

		private bool TooClose => Mathf.Approximately(OffsetToTarget.sqrMagnitude, 0f);
	
		private Quaternion TargetRotation => Quaternion.LookRotation(LookDirection, Vector3.up);

		private Vector3 LookDirection => OffsetToTarget.With(y: 0f).normalized;

		private Vector3 OffsetToTarget => _lookAt.position - _rigidbody.position;

		private void Awake()
		{
			this.RequireInParent(out Character character);
			character.Require(out _rigidbody);
		}

		private Rigidbody _rigidbody;
	}
}