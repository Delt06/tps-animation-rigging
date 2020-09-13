using UnityEngine;

namespace Weapons
{
	public interface IWeaponHandle
	{
		Transform Transform { get; }
		Hand Hand { get; }
	}

	public sealed class WeaponHandle : MonoBehaviour, IWeaponHandle
	{
		[SerializeField] private Hand _hand = Hand.Right;

		public Transform Transform => transform;
		public Hand Hand => _hand;
	}
}