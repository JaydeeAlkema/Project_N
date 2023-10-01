using Assets.Scripts;
using NaughtyAttributes;
using UnityEngine;
using Assets.Scripts.CustomHurtboxAndHitbox;

namespace Assets.Scripts.Combat.PlayerAttacks
{
	public class PlayerProjectile : PlayerRangedAttack
	{
		[SerializeField, BoxGroup("Settings")] private float _speed = 10f;
		[SerializeField, BoxGroup("Settings")] private float _lifetime = 7f;

		[SerializeField, BoxGroup("References")] private ScriptableFloat _pierceAmount;
		[SerializeField, BoxGroup("References")] private GameObject _onSpawnParticles = default;
		[SerializeField, BoxGroup("References")] private GameObject _onDestroyExplosion = default;
		[SerializeField, BoxGroup("References")] private Transform _onDestroyExplosionLocation = default;
		[SerializeField, BoxGroup("References")] private GameObject[] _trailParticles = default;
		[SerializeField, BoxGroup("References")] private Hitbox _environmentHitbox = default;
		[SerializeField, BoxGroup("References")] private float _chargeScale = 1.0f;

		private int _piercesLeft;

		public float ChargeScale { get => _chargeScale; set => _chargeScale = value; }

		public override void OnAwake()
		{
			base.OnAwake();
			_piercesLeft = (int)_pierceAmount.value;
			_environmentHitbox.Responder = this;
			if (_onSpawnParticles) Instantiate(_onSpawnParticles, transform.position + transform.up, transform.rotation);
		}

		public override void OnUpdate()
		{
			base.OnUpdate();

			if (_lifetime > 0) transform.position = Vector2.MoveTowards(transform.position, transform.position + transform.up * (_speed * _lifetime), _speed * Time.deltaTime);

			CountdownLifetime();
			_environmentHitbox.HitboxUpdate();
		}

		public override void CollisionedWith(Collider2D collider)
		{
			if (collider == null) return;
			Hurtbox hurtbox = collider.GetComponentInParent<Hurtbox>();
			if ((hurtbox != null && hurtbox.State == HurtboxState.Disabled)) return;

			if (_piercesLeft > 0 && hurtbox != null)
			{
				base.CollisionedWith(collider);
				_piercesLeft--;
			}
			else
			{
				UnparentTrailParticlesBeforeDestroy();
				SpawnExplosion();
				Destroy(gameObject);
			}
		}

		private void CountdownLifetime()
		{
			if (_lifetime <= 0)
			{
				UnparentTrailParticlesBeforeDestroy();
				SpawnExplosion();
				Destroy(gameObject);
			}
			else
			{
				_lifetime -= Time.deltaTime;
			}
		}

		private void UnparentTrailParticlesBeforeDestroy()
		{
			foreach (GameObject trailParticles in _trailParticles)
			{
				trailParticles.transform.SetParent(null);
			}
		}

		private void SpawnExplosion()
		{
			if (_onDestroyExplosion)
			{
				GameObject explosion = Instantiate(_onDestroyExplosion, _onDestroyExplosionLocation.position, transform.rotation);
				explosion.transform.localScale *= _chargeScale;
				explosion.GetComponentInChildren<Attack>().Damage = Damage;
			}
		}
	}

}