using UnityEngine;

namespace Weapons
{
	public sealed class Gun : Weapon
	{
		[SerializeField] private Bullet _bulletPrefab = default;
		[SerializeField, Min(0f)] private float _speed = 10f;
		[SerializeField] private Transform _shootFrom = default;

		protected override void UseAction()
		{
			var bullet = Instantiate(_bulletPrefab, _shootFrom.position, _shootFrom.rotation);
			bullet.StartMovingAlong(_shootFrom.forward * _speed);
		}
	}
}