using Assets.Scripts;
using NaughtyAttributes;
using UnityEngine;

namespace Assets.Scripts.Combat.PlayerAttacks
{
	public abstract class PlayerRangedAttack : PlayerAttack
	{
		[BoxGroup("Attack Animation"), SerializeField] protected Animator _attackAnimator;
		[BoxGroup("Attack Animation"), SerializeField] protected AnimationClip _attackAnimationClip;

		[BoxGroup("Player Animation"), SerializeField] protected AnimationClip _playerAnimationClip;

		[BoxGroup("Settings"), SerializeField] protected AttackSpawn _spawn;

		public AnimationClip PlayerAnimationClip { get => _playerAnimationClip; private set => _playerAnimationClip = value; }
		public AttackSpawn Spawn { get => _spawn; private set => _spawn = value; }
	}
}
