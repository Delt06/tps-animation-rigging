using System;
using Characters;
using UnityEngine;
using Validation;

namespace Movement
{
	public sealed class MovementBlock : MonoBehaviour
	{
		public void SetFor(float duration)
		{
			if (duration < 0f) throw new ArgumentOutOfRangeException(nameof(duration));
			_remainingTime = Mathf.Max(duration, _remainingTime);
			_characterMovement.enabled = false;
		}

		private void Update()
		{
			if (_remainingTime < 0f) return;

			_remainingTime -= Time.deltaTime;
			if (_remainingTime >= 0f) return;

			_characterMovement.enabled = true;
		}

		private void Awake()
		{
			this.RequireInParent(out Character character);
			character.RequireInChildren(out _characterMovement);
		}

		private float _remainingTime = -1f;
		private CharacterMovement _characterMovement;
	}
}