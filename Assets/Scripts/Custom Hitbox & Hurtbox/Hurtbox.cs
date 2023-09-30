using Assets.Scripts.Interfaces;
using NaughtyAttributes;
using UnityEngine;

namespace Assets.Scripts
{
	public enum HurtboxState
	{
		Enabled,
		Disabled
	}

	[RequireComponent(typeof(BoxCollider2D))]
	public class Hurtbox : MonoBehaviour
	{
		[SerializeField] private BoxCollider2D _boxCollider;
		[Space]
		[SerializeField] private Color _enabledColor = new Color(0.4f, 1f, 0.15f, 0.35f);
		[SerializeField] private Color _disabledColor = new Color(1f, 0.2f, 0.2f, 0.35f);
		[Space]
		[SerializeField] private HurtboxState _state = HurtboxState.Enabled;

		public HurtboxState State { get => _state; }

		private void Awake()
		{
			_boxCollider.isTrigger = true;
		}

		[Button]
		public void EnableHurtbox()
		{
			_state = HurtboxState.Enabled;
			_boxCollider.enabled = true;
		}

		[Button]
		public void DisableHurtbox()
		{
			_state = HurtboxState.Disabled;
			_boxCollider.enabled = false;
		}

		public void GetHitBy(IDamageable damageable, int damage)
		{
			damageable.TakeDamage(damage);
		}

		private void CheckGizmoColor()
		{
			switch (_state)
			{
				case HurtboxState.Enabled:
					Gizmos.color = _enabledColor;
					break;
				case HurtboxState.Disabled:
					Gizmos.color = _disabledColor;
					break;
				default:
					break;
			}
		}

		private void OnDrawGizmos()
		{
			if (!_boxCollider) return;

			CheckGizmoColor();

			Vector3 position = transform.position;
			Vector2 offset = _boxCollider.offset;
			Vector2 size = _boxCollider.size;
			Vector3 localScale = transform.lossyScale;

			Vector3 newPos = new Vector3(position.x + offset.x, position.y + offset.y, position.z);
			Vector3 newScale = new Vector3(size.x * localScale.x, size.y * localScale.y, 0);

			Gizmos.DrawCube(newPos, newScale);
		}
	}
}
