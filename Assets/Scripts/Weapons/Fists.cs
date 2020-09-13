using UnityEngine;

namespace Weapons
{
	public sealed class Fists : Weapon
	{
		[SerializeField, Min(0f)] private float _cooldown = 1f;

		public float Cooldown => _cooldown;

		public override bool IsBusy => Time.time < _lastAttackTime + _cooldown;

		protected override void UseAction()
		{
			_lastAttackTime = Time.time;
		}

		private float _lastAttackTime = Mathf.NegativeInfinity;
	}
}