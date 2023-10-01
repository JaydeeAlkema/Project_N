using Assets.Scripts.Interfaces;
using NaughtyAttributes;
using UnityEngine;
using Assets.Scripts.CustomHurtboxAndHitbox;

namespace Assets.Scripts
{
	public enum AttackType
	{
		Melee,
		Ranged,
		True
	}

	public class Attack : MonoBehaviour, IHitboxResponder
	{
		[BoxGroup("Base"), SerializeField] protected AttackType _attackType;
		[BoxGroup("Base"), SerializeField] protected string _name;
		[BoxGroup("Base"), SerializeField, ResizableTextArea] protected string _description;

		[BoxGroup("Settings"), SerializeField] protected int _damage;

		[BoxGroup("References"), SerializeField] protected Hitbox _hitbox;

		public int Damage { get => _damage; set => _damage = value; }


		private void Awake()
		{
			OnAwake();
		}
		public virtual void OnAwake()
		{
			SetResponder();
		}

		private void Update()
		{
			OnUpdate();
		}
		public virtual void OnUpdate()
		{
			_hitbox.HitboxUpdate();
		}

		public void SetResponder()
		{
			_hitbox.Responder = this;
		}
		public virtual void CollisionedWith(Collider2D collider)
		{
		}
	}

}