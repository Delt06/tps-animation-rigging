using System;
using Characters;
using Movement;
using UnityEngine;
using Validation;
using Weapons;

namespace Animations
{
	public sealed class CharacterPunchAnimation : MonoBehaviour
	{
		private void OnEnable()
		{
			_fists.Used += _onUsed;
		}

		private void OnDisable()
		{
			_fists.Used -= _onUsed;
		}

		private void Awake()
		{
			this.RequireInParent(out Character character);
			character.RequireInChildren(out _fists);
			character.RequireInChildren(out _animator);
			character.RequireInChildren(out _movementBlock);

			_onUsed = (sender, args) =>
			{
				_animator.SetTrigger(_punchTriggerId);
				_movementBlock.SetFor(_fists.Cooldown);
			};
		}

		private EventHandler _onUsed;
		private Animator _animator;
		private Fists _fists;
		private MovementBlock _movementBlock;
		private readonly int _punchTriggerId = Animator.StringToHash("Punched");
	}
}
