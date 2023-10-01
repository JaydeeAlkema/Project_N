using Assets.Scripts.Interfaces;
using NaughtyAttributes;
using System.Collections;
using UnityEngine;
using Assets.Scripts.CustomHurtboxAndHitbox;

namespace Assets.Scripts
{
	public enum AttackSpawn
	{
		Pivot,
		Point
	}

	public class PlayerAttack : Attack
	{
		[BoxGroup("Settings"), SerializeField] protected ScriptableFloat _critChance;
		[BoxGroup("Settings"), SerializeField] protected ScriptableFloat _critDamageMultiplier;

		public override void OnAwake()
		{
			base.OnAwake();
		}

		public override void CollisionedWith(Collider2D collider)
		{
			if (collider == null) return;
			Hurtbox hurtbox = collider.GetComponentInParent<Hurtbox>();
			if (hurtbox == null || hurtbox.State == HurtboxState.Disabled) return;
			IDamageable damageable = hurtbox.transform.GetComponentInParent<IDamageable>();
			if (damageable == null) return;

			hurtbox.GetHitBy(damageable, _damage);
			Debug.Log($"{hurtbox.transform.parent.name} hit for {_damage}");

			return;
		}
	}
}