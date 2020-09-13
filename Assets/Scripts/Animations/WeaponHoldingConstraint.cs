using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Animations.Rigging;
using Validation;
using Weapons;

namespace Animations
{
	public sealed class WeaponHoldingConstraint : MonoBehaviour
	{
		[SerializeField] private Hand _hand = Hand.Right;
		
		private void Start()
		{
			Refresh();
		}

		private void OnEnable()
		{
			Refresh();
			_arsenal.EquippedWeaponUpdated += _onUpdated;
		}

		private void OnDisable()
		{
			_arsenal.EquippedWeaponUpdated -= _onUpdated;
		}

		private void Awake()
		{
			this.Require(out _parentConstraint);
			this.Require(out _ikConstraint);
			this.RequireInParent(out _arsenal);

			_onUpdated = (sender, args) => Refresh();
		}

		private void Refresh()
		{
			if (Handle != null)
			{
				_ikConstraint.weight = 1f;
				_parentConstraint.constraintActive = true;
				_sources.Clear();
				_sources.Add(new ConstraintSource { sourceTransform = Handle.Transform, weight = 1f } );
				_parentConstraint.SetSources(_sources);
			}
			else
			{
				_ikConstraint.weight = 0f;
				_parentConstraint.constraintActive = false;
			}
		}

		[CanBeNull]
		private IWeaponHandle Handle
		{
			get
			{
				if (_arsenal.WeaponsCount == 0) return null;
				
				var weapon = _arsenal.EquippedWeapon;
				
				switch (_hand)
				{
					case Hand.Right: return weapon.RightHandle;
					case Hand.Left: return weapon.LeftHandle;
					default: throw new ArgumentOutOfRangeException();
				}
			}
		}

		private TwoBoneIKConstraint _ikConstraint;
		private ParentConstraint _parentConstraint;
		private CharacterArsenal _arsenal;
		private EventHandler _onUpdated;
		private readonly List<ConstraintSource> _sources = new List<ConstraintSource>();
	}
}