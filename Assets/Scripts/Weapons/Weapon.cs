using System;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Assertions;

namespace Weapons
{
	[DisallowMultipleComponent]
	public class Weapon : MonoBehaviour
	{
		[CanBeNull]
		public IWeaponHandle RightHandle { get; private set; }

		[CanBeNull]
		public IWeaponHandle LeftHandle { get; private set; }

		public bool RequiresHands => RightHandle != null || LeftHandle != null;

		public virtual bool IsBusy => false;
		public virtual bool CanBeUsed => !IsBusy;

		public void Use()
		{
			Assert.IsTrue(CanBeUsed);
			UseAction();
			OnUsed();
		}

		protected virtual void UseAction()
		{
			
		}

		protected virtual void OnUsed()
		{
			Used?.Invoke(this, EventArgs.Empty);
		}

		public event EventHandler Used;

		protected virtual void Awake()
		{
			RightHandle = TryFindHandle(Hand.Right);
			LeftHandle = TryFindHandle(Hand.Left);
		}

		private IWeaponHandle TryFindHandle(Hand hand)
		{
			return GetComponentsInChildren<IWeaponHandle>()
				.FirstOrDefault(h => h.Hand == hand);
		}
	}
}