using Assets.Scripts;
using NaughtyAttributes;
using UnityEngine;

namespace Assets.Scripts.Combat.PlayerAttacks
{
	public class PlayerMeleeAttack : PlayerAttack
	{
		[BoxGroup("Attack Animation"), SerializeField] private Animator _attackAnimator;
		[BoxGroup("Attack Animation"), SerializeField] private AnimationClip _attackAnimationClip;

		[BoxGroup("Player Animation"), SerializeField] private AnimationClip _playerAnimationClip;

		[BoxGroup("Settings"), SerializeField] private AttackSpawn _spawn;

		public AttackSpawn Spawn { get => _spawn; private set => _spawn = value; }
		public AnimationClip PlayerAnimationClip { get => _playerAnimationClip; private set => _playerAnimationClip = value; }

		public override void OnAwake()
		{
			base.OnAwake();
			if (_attackAnimator == null || _attackAnimationClip == null) return;
			_attackAnimator.Play(_attackAnimationClip.name);
			Destroy(gameObject, _attackAnimator.GetCurrentAnimatorStateInfo(0).length);
		}
	}
}
