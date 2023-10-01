using Assets.Scripts.CustomHurtboxAndHitbox;
using Assets.Scripts.Interfaces;
using NaughtyAttributes;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
	[SerializeField, BoxGroup("Stats")] private int _health = 100;

	[SerializeField, BoxGroup("Combat"), Required] private Hurtbox _hurtbox = null;

	public void TakeDamage(int damage)
	{
		if (_hurtbox == null)
		{
			Debug.LogError($"{name} has no Hurtbox reference assigned!");
			return;
		}
		_health -= damage;
		Debug.Log($"{name} hit for {damage}. New health: {_health}");
		if (_health <= 0) OnDeath();
	}

	private void OnDeath()
	{
		// Add some XP or currency to the player through a scriptable object.

		// Instantiate on death particles.

		// Destroy own GameObject.
		Destroy(gameObject);
	}
}
