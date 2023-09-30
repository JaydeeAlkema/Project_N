using NaughtyAttributes;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

using Assets.Scripts.Interfaces;

namespace Assets.Scripts
{
	public enum HitboxState
	{
		Closed,
		Open,
		Colliding
	}

	public class Hitbox : MonoBehaviour
	{
		[SerializeField] private LayerMask _mask;
		[SerializeField] private Vector2 _size;
		[SerializeField] private bool _useSphere = false;
		[SerializeField] private float _radius;
		[Space]
		[SerializeField] private Color _enabledColor = new Color(0.4f, 1f, 0.15f, 0.18f);
		[SerializeField] private Color _disabledColor = new Color(1f, 0.65f, 0.2f, 0.18f);
		[SerializeField] private Color _collidingColor = new Color(1f, 0.15f, 0.15f, 0.18f);
		[Space]
		[SerializeField] private IHitboxResponder _responder;
		[SerializeField] private HitboxState _state;

		HashSet<Collider2D> _hurtboxes = new HashSet<Collider2D>();
		bool _uniqueHurtboxHit = false;

		public IHitboxResponder Responder { get => _responder; set => _responder = value; }
		public HashSet<Collider2D> Hurtboxes { get => _hurtboxes; private set => _hurtboxes = value; }

		[Button]
		public void StartCheckingCollision()
		{
			_state = HitboxState.Open;
		}
		[Button]
		public void StopCheckingCollision()
		{
			_state = HitboxState.Closed;
		}

		[Button]
		public void HitboxUpdate()
		{
			if (_state == HitboxState.Closed)
			{
				ClearHurtboxesHitHashSet();
				return;
			}

			Collider2D[] colliders = _useSphere ? Physics2D.OverlapCircleAll(transform.position, _radius * transform.parent.localScale.x, _mask) : Physics2D.OverlapBoxAll(transform.position, _size * transform.parent.localScale.x, transform.rotation.eulerAngles.z, _mask);
			_uniqueHurtboxHit = false;

			for (int i = 0; i < colliders.Length; i++)
			{
				Collider2D collider2D = colliders[i];
				if (!_hurtboxes.Add(collider2D)) continue;

				_uniqueHurtboxHit = true;
				_responder?.CollisionedWith(collider2D);
			}

			_state = _uniqueHurtboxHit ? HitboxState.Colliding : HitboxState.Open;
		}

		[Button]
		public void ClearHurtboxesHitHashSet()
		{
			_hurtboxes.Clear();
		}

		private void CheckGizmoColor()
		{
			switch (_state)
			{
				case HitboxState.Closed:
					Gizmos.color = _disabledColor;
#if UNITY_EDITOR
					Handles.color = _disabledColor;
#endif
					break;
				case HitboxState.Open:
					Gizmos.color = _enabledColor;
#if UNITY_EDITOR
					Handles.color = _enabledColor;
#endif
					break;
				case HitboxState.Colliding:
					Gizmos.color = _collidingColor;
#if UNITY_EDITOR
					Handles.color = _collidingColor;
#endif
					break;
				default:
					break;
			}
		}

		private void OnDrawGizmos()
		{
			CheckGizmoColor();
			Gizmos.matrix = transform.localToWorldMatrix;
#if UNITY_EDITOR
			Handles.matrix = transform.localToWorldMatrix;

			if (_useSphere) Handles.DrawSolidDisc(Vector3.zero, Vector3.forward, _radius);
			else Gizmos.DrawCube(Vector3.zero, new Vector3(_size.x, _size.y, 0));
#endif
		}
	}
}
