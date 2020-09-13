using Characters;
using Movement;
using UnityEngine;
using UnityUtilities;
using Validation;

namespace Animations
{
	public sealed class CharacterMovementAnimations : MonoBehaviour
	{
		[SerializeField, Min(0f)] private float _animationAcceleration = 1f;
	
		private void Update()
		{
			var deltaSpeed = _animationAcceleration * Time.deltaTime;
			AnimationVelocity = Vector2.MoveTowards(AnimationVelocity, TargetAnimationVelocity, deltaSpeed);
		}

		private Vector2 AnimationVelocity
		{
			get => new Vector2(_animator.GetFloat(_speedXId), _animator.GetFloat(_speedYId));
			set
			{
				_animator.SetFloat(_speedXId, value.x);
				_animator.SetFloat(_speedYId, value.y);
			}
		}

		private Vector2 TargetAnimationVelocity
		{
			get
			{
				var (targetSpeedX, _, targetSpeedY) = _movement.RelativeVelocity;
				return new Vector2(targetSpeedX, targetSpeedY);
			}
		}
	
		private void Awake()
		{
			this.RequireInParent(out Character character);
			character.RequireInChildren(out _animator);
			character.RequireInChildren(out _movement);
		}

		private CharacterMovement _movement;
		private Animator _animator;

		private readonly int _speedXId = Animator.StringToHash("SpeedX");
		private readonly int _speedYId = Animator.StringToHash("SpeedY");
	}
}