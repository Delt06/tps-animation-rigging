using System;
using UnityEngine;

namespace Weapons
{
	public sealed class CharacterArsenal : MonoBehaviour
	{
		[SerializeField] private Transform _weaponRoot = default;

		public int EquippedWeaponIndex
		{
			get => _equippedWeaponIndex;
			set
			{
				if (value < 0 || value >= WeaponsCount) throw new ArgumentOutOfRangeException(nameof(value));
				if (_equippedWeaponIndex == value) return;
				
				_equippedWeaponIndex = value;
				OnEquippedWeaponChanged();
				
			}
		}

		public int WeaponsCount => Weapons.Length;

		public Weapon EquippedWeapon => Weapons[EquippedWeaponIndex];
		public Weapon[] Weapons { get; private set; } = Array.Empty<Weapon>();

		private void OnEquippedWeaponChanged()
		{
			UpdateWeaponObjects();
			EquippedWeaponUpdated?.Invoke(this, EventArgs.Empty);
		}

		public event EventHandler EquippedWeaponUpdated;

		private void UpdateWeaponObjects()
		{
			for (var index = 0; index < WeaponsCount; index++)
			{
				Weapons[index].gameObject.SetActive(index == _equippedWeaponIndex);
			}
		}

		private void Awake()
		{
			Weapons = _weaponRoot.GetComponentsInChildren<Weapon>();
			Array.Sort(Weapons, _weaponOrderingRule);
			UpdateWeaponObjects();
		}

		private int _equippedWeaponIndex;

		private readonly Comparison<Weapon> _weaponOrderingRule =
			(w1, w2) => w1.transform.GetSiblingIndex() - w2.transform.GetSiblingIndex();
	}
}