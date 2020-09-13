using System;
using Characters;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using Validation;
using Weapons;

namespace Animations
{
	public sealed class CharacterGunRecoilAnimation : MonoBehaviour
	{
		[SerializeField] private Rig _recoilRig = default;
		[SerializeField, Min(0f)] private float _restoreSpeed = 1f;

		private void Update()
		{
			if (Mathf.Approximately(_recoilRig.weight, 0f)) return;

			var deltaWeight = _restoreSpeed * Time.deltaTime;
			_recoilRig.weight = Mathf.MoveTowards(_recoilRig.weight, 0f, deltaWeight);
		}

		private void OnEnable()
		{
			foreach (var gun in _guns)
			{
				gun.Used += _onUsed;
			}
		}

		private void OnDisable()
		{
			foreach (var gun in _guns)
			{
				gun.Used -= _onUsed;
			}
		}

		private void Awake()
		{
			this.RequireInParent(out Character character);

			_guns = character.GetComponentsInChildren<Gun>();
			_onUsed = (sender, args) =>
			{
				_recoilRig.weight = 1f;
			};
		}

		private Gun[] _guns;
		private EventHandler _onUsed;
	}
}