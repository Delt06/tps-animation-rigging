using UnityEngine;
using UnityEngine.EventSystems;
using UnityUtilities;

namespace Controls
{
	public sealed class CameraControls : MonoBehaviour, IDragHandler
	{
		[SerializeField] private InputHandler _inputHandler = default;
		[SerializeField] private Vector2 _sensitivity = Vector2.one;
	
		public void OnDrag(PointerEventData eventData)
		{
			var (rotationY, rotationX) = _sensitivity * eventData.delta;
			_inputHandler.RotateCamera(rotationX, rotationY);
		}
	}
}