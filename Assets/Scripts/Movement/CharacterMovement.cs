using UnityEngine;

namespace Movement
{
	public sealed class CharacterMovement : MonoBehaviour
	{
		[SerializeField, Min(0f)] private float _speed = 1f;
		[SerializeField, Min(0f)] private float _accelerationNormalized = 1f;
		[SerializeField, Range(0f, 1f)] private float _sideSpeedMultiplier = 0.75f;
		[SerializeField, Range(0f, 1f)] private float _backSpeedMultiplier = 0.5f;

		private void FixedUpdate()
		{
			UpdateDirection(Time.fixedDeltaTime);
			UpdateVelocity();
		}

		private void UpdateDirection(float deltaTime)
		{
			var deltaDirection = _accelerationNormalized * deltaTime;
			_direction = Vector3.MoveTowards(_direction, TargetDirection, deltaDirection);
		}

		private Vector3 TargetDirection
		{
			get
			{
				var projectionOnXZ = Vector3.ProjectOnPlane(WorldSpaceDirection, Vector3.up);
				return projectionOnXZ.normalized;
			}
		}

		private void UpdateVelocity()
		{
			var velocity = ApplyVelocityModifiers(_direction * _speed);
			_rigidbody.velocity = velocity;
		}

		private Vector3 ApplyVelocityModifiers(Vector3 worldSpaceVelocity)
		{
			worldSpaceVelocity.y = _rigidbody.velocity.y;
		
			var localVelocity = transform.InverseTransformVector(worldSpaceVelocity);
			localVelocity.x *= _sideSpeedMultiplier;

			if (localVelocity.z < 0f)
				localVelocity.z *= _backSpeedMultiplier;

			return transform.TransformVector(localVelocity);
		}

		public Vector3 WorldSpaceDirection { get; set; }

		public Vector3 RelativeVelocity => transform.InverseTransformVector(_direction).normalized;

		private void Awake()
		{
			_rigidbody = GetComponent<Rigidbody>();
		}

		private void OnDisable()
		{
			_rigidbody.velocity = new Vector3(0f, _rigidbody.velocity.y, 0f);
		}

		private Rigidbody _rigidbody;
		private Vector3 _direction;
	}
}