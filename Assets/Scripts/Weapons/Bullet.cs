using UnityEngine;
using Validation;

namespace Weapons
{
	public sealed class Bullet : MonoBehaviour
	{
		[SerializeField] private float _lifetime = 1f;

		public void StartMovingAlong(Vector3 velocity)
		{
			_rigidbody.velocity = velocity;
			_remainingLifetime = _lifetime;
		}

		private void Update()
		{
			_remainingLifetime -= Time.deltaTime;
			if (_remainingLifetime >= 0f) return;
			
			Destroy(gameObject);
		}

		private void Awake()
		{
			this.Require(out _rigidbody);
		}

		private Rigidbody _rigidbody;
		private float _remainingLifetime;
	}
}