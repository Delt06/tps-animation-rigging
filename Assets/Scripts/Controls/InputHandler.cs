using Characters;
using Movement;
using UnityEngine;
using UnityUtilities;
using Validation;
using Weapons;

namespace Controls
{
	public sealed class InputHandler : MonoBehaviour
	{
		[SerializeField] private Transform _cameraNode = default;
		[SerializeField, Range(-180f, 180f)] private float _minZAngle = -180f;
		[SerializeField, Range(-180f, 180f)] private float _maxZAngle = 180f;
		[SerializeField] private Character _character = default;

		public float HorizontalAxis
		{
			get => _horizontalAxis;
			set => _horizontalAxis = Mathf.Clamp(value, -1f, 1f);
		}

		public float VerticalAxis
		{
			get => _verticalAxis;
			set => _verticalAxis = Mathf.Clamp(value, -1f, 1f);
		}

		public void ShiftEquippedWeapon(int offset)
		{
			if (_arsenal.EquippedWeapon.IsBusy) return;
			
			var newWeaponIndex = (_arsenal.EquippedWeaponIndex + offset) % _arsenal.WeaponsCount;
			_arsenal.EquippedWeaponIndex = newWeaponIndex;
		}

		public void UseEquippedWeaponIfCan()
		{
			var weapon = _arsenal.EquippedWeapon;
			
			if (weapon.CanBeUsed)
				weapon.Use();
		}

		public void RotateCamera(float sideAxis, float verticalAxis)
		{
			(sideAxis, verticalAxis, _) = _camera.ScreenToViewportPoint(new Vector3(sideAxis, verticalAxis));
			var deltaEulerAngles = new Vector3(sideAxis, verticalAxis, 0f);
			var newAngles = _cameraNode.eulerAngles + deltaEulerAngles;
			newAngles.z = Mathf.Clamp(NormalizedAngle(newAngles.z), _minZAngle, _maxZAngle);
			_cameraNode.eulerAngles = newAngles;
		}

		private static float NormalizedAngle(float angle)
		{
			while (angle < -180f)
			{
				angle += 360f;
			}

			while (angle > 180f)
			{
				angle -= 360f;
			}

			return angle;
		}

		private void Update()
		{
			HorizontalAxis += Input.GetAxisRaw("Horizontal");
			VerticalAxis += Input.GetAxisRaw("Vertical");

			_movement.WorldSpaceDirection = _cameraNode.forward * VerticalAxis +
			                                _cameraNode.right * HorizontalAxis;	
		}

		private void Awake()
		{
			_character.RequireInChildren(out _movement);
			_character.RequireInChildren(out _arsenal);
			_camera = Camera.main;
		}

		private float _horizontalAxis;
		private float _verticalAxis;
		private Camera _camera;
		private CharacterMovement _movement;
		private CharacterArsenal _arsenal;
	}
}