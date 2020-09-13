using Characters;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using Validation;
using Weapons;

namespace Animations
{
	public sealed class CharacterWeaponEquipmentAnimation : MonoBehaviour
	{
		[SerializeField] private Rig[] _onHands = default;
		[SerializeField] private Rig[] _onNoHands = default;
		[SerializeField, Min(0f)] private float _weightChangeSpeed = 1f;

		private void Update()
		{
			var deltaWeight = _weightChangeSpeed * Time.deltaTime;
			
			foreach (var rig in _onHands)
			{
				UpdateWeight(rig, deltaWeight, true);
			}

			foreach (var rig in _onNoHands)
			{
				UpdateWeight(rig, deltaWeight, false);
			}
		}

		private void UpdateWeight(Rig rig, float deltaWeight, bool hands)
		{
			var targetWeight = _arsenal.EquippedWeapon.RequiresHands == hands ? 1f : 0f;
			rig.weight = Mathf.MoveTowards(rig.weight, targetWeight, deltaWeight);
		}

		private void Awake()
		{
			this.RequireInParent(out Character character);
			character.RequireInChildren(out _arsenal);
		}

		private CharacterArsenal _arsenal;
	}
}