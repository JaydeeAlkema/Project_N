using NaughtyAttributes;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
namespace Assets.Scripts
{
	[RequireComponent(typeof(Rigidbody2D), typeof(CircleCollider2D))]
	public class PlayerController : MonoBehaviour
	{
		[SerializeField, BoxGroup("Components")] private Rigidbody2D _rigidbody2D = null;
		[SerializeField, BoxGroup("Components")] private CircleCollider2D _circleCollider2D = null;
		[SerializeField, BoxGroup("Components")] private PlayerInput _playerInput = null;

		[SerializeField, BoxGroup("Movement")] private float _moveSpeed = 8f;

		[SerializeField, BoxGroup("Pointer")] private Transform _pointerPivot;

		private PlayerControls _controls;

		private void Awake()
		{
			SetupInputs();
		}

		private void Update()
		{
			RotatePointerTowardsMouse();
		}

		private void OnDestroy()
		{
			DisableInputs();
		}

		#region Input
		private void SetupInputs()
		{
			_controls = new PlayerControls();
			_controls.Enable();

			_controls.Player.Enable();
			_controls.Player.Move.performed += Move_performed;
			_controls.Player.Move.canceled += Move_canceled;
		}
		private void DisableInputs()
		{
			_controls.Player.Move.performed -= Move_performed;
			_controls.Player.Move.canceled -= Move_canceled;
			_controls.Disable();
		}

		private void Move_performed(InputAction.CallbackContext obj)
		{
			if (_rigidbody2D == null) return;
			Vector2 movementInput = obj.ReadValue<Vector2>();

			if (movementInput != Vector2.zero) _rigidbody2D.velocity = movementInput.normalized * _moveSpeed;
		}
		private void Move_canceled(InputAction.CallbackContext obj)
		{
			if (_rigidbody2D == null) return;
			Vector2 movementInput = obj.ReadValue<Vector2>();

			if (movementInput == Vector2.zero) _rigidbody2D.velocity = Vector2.zero;
		}
		#endregion

		#region Pointer
		private void RotatePointerTowardsMouse()
		{
			if (_pointerPivot == null) return;

			Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			Vector2 direction = mousePos - (Vector2)_pointerPivot.position;

			float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
			Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);

			_pointerPivot.rotation = rotation;
		}
		#endregion
	}
}
