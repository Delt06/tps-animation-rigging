using UnityEngine;
using UnityEngine.EventSystems;
using UnityUtilities;

namespace Controls
{
	public class Joystick : MonoBehaviour, IDragHandler,
		IPointerDownHandler, IPointerExitHandler, IPointerUpHandler
	{
		[SerializeField] private RectTransform _stick = default;
		[SerializeField] private RectTransform _hinge = default;
		[SerializeField] private InputHandler _inputHandler = default;

		private void Update()
		{
			float horizontal;
			float vertical;

			if (_pointerId.HasValue)
			{
				(horizontal, vertical, _) = StickOffset / _stickMaxOffset;
			}
			else
			{
				(horizontal, vertical) = (0f, 0f);
			}

			_inputHandler.HorizontalAxis = horizontal;
			_inputHandler.VerticalAxis = vertical;
		}

		private Vector3 StickOffset => StickWorldAnchor - (Vector2) _hinge.position;
	
		private Vector2 StickWorldAnchor => _stick.TransformPoint(_stick.anchoredPosition);

		void IPointerDownHandler.OnPointerDown(PointerEventData eventData) => TryPress(eventData);
	
		void IDragHandler.OnDrag(PointerEventData eventData) => TryHold(eventData);

		void IPointerExitHandler.OnPointerExit(PointerEventData eventData) => TryRelease(eventData);

		void IPointerUpHandler.OnPointerUp(PointerEventData eventData) => TryRelease(eventData);

		private void TryPress(PointerEventData eventData)
		{
			if (_pointerId.HasValue) return;
		
			_pointerId = eventData.pointerId;
			_hinge.position = eventData.position;

			UpdateStickPosition(eventData);
		}

		private void UpdateStickPosition(PointerEventData eventData)
		{
			Vector2 origin = _hinge.position;
			var offset = eventData.position - origin;
			offset = Vector3.ClampMagnitude(offset, _stickMaxOffset);
			_stick.position = origin + offset;
		}

		private void TryHold(PointerEventData eventData)
		{
			if (_pointerId != eventData.pointerId) return;

			UpdateStickPosition(eventData);
		}

		private void TryRelease(PointerEventData eventData)
		{
			if (_pointerId != eventData.pointerId) return;

			ResetPointer();
		}

		private void OnEnable() => ResetPointer();

		private void OnDisable() => ResetPointer();

		private void ResetPointer()
		{
			_hinge.position = _initialHingePosition;
			_stick.anchoredPosition = Vector2.zero;
			_pointerId = null;
		}

		private void Awake()
		{
			_stickMaxOffset = _stick.anchoredPosition.magnitude;
			_initialHingePosition = _hinge.position;
		}

		private int? _pointerId;
		private float _stickMaxOffset;
		private Vector3 _initialHingePosition;
	}
}