using Assets.Scripts.Combat.PlayerAttacks;
using NaughtyAttributes;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts.Player
{
	public class PlayerCombat : MonoBehaviour
	{
		[SerializeField, BoxGroup("References")] private Animator _playerAnimator = default;
		[SerializeField, BoxGroup("References")] private Transform _attackPivot = default;
		[SerializeField, BoxGroup("References")] private PlayerControler _player;

		[SerializeField, BoxGroup("Melee")] private Transform _meleeAttackSpawnPoint = default;
		[Space]
		[SerializeField, BoxGroup("Melee")] private int _currentMeleeAttackIndex = 0;
		[SerializeField, BoxGroup("Melee")] private float _meleeComboTimeLimit = 0.5f;      // Countdown until combo is reset.
		[SerializeField, BoxGroup("Melee"), Expandable] private ScriptableFloat _timeBetweenMeleeAttacks;  // Attack Speed.
		[SerializeField, BoxGroup("Melee")] private float _meleeBufferDuration = 0.08f;  // Buffer time for melee input
		[SerializeField, BoxGroup("Melee")] private List<GameObject> _meleeAttacks = new List<GameObject>();

		[SerializeField, Foldout("Debugging")] private float _meleeComboTimer = 0;
		[SerializeField, Foldout("Debugging")] private float _timeBetweenMeleeAttacksTimer = 0;
		[SerializeField, Foldout("Debugging")] private bool _canMeleeAttack = false;
		[SerializeField, Foldout("Debugging")] private bool _meleeRequireInput;
		[SerializeField, Foldout("Debugging")] private float _meleeBufferTimer = 0f;

		private Controls _controls;

		#region Unity Callbacks
		private void Awake()
		{
			_controls = new Controls();
			_timeBetweenMeleeAttacksTimer = _timeBetweenMeleeAttacks.value;
			_meleeRequireInput = true;
		}
		private void OnEnable()
		{
			_controls.Combat.Melee.Enable();
		}
		private void Update()
		{
			HandleMeleeAttack(_meleeRequireInput);

			CountdownTimeMeleeInputBuffer();
			CountdownMeleeComboTimer();
			CountdownTimeBetweenMeleeAttacks();
		}
		private void OnDisable()
		{
			_controls.Combat.Melee.Disable();
		}
		private void OnDestroy()
		{
			_controls.Combat.Melee.Disable();
		}
		#endregion

		#region Input Callbacks
		private void HandleMeleeAttack(bool inputRequired)
		{
			if (!_canMeleeAttack || (inputRequired && _controls.Combat.Melee.ReadValue<float>() < 0.5f) || _meleeAttacks.Count == 0) return;

			_meleeBufferTimer = 0f;
			_meleeRequireInput = true;

			// Get melee attack component and store relevant references.
			PlayerMeleeAttack meleeAttack = _meleeAttacks[_currentMeleeAttackIndex].GetComponent<PlayerMeleeAttack>();

			// Read and play the animation clip for the attack
			if (meleeAttack.PlayerAnimationClip) _playerAnimator.Play(meleeAttack.PlayerAnimationClip.name);

			// Check if attack stays with player on spawn
			Transform attackParent = null;
			_meleeAttacks[_currentMeleeAttackIndex].TryGetComponent(out StickyAttack stickyAttack);
			if (stickyAttack != null)
			{
				attackParent = transform;
			}

			// Read the melee attack spawn settings and spawn the melee attack accordingly.
			AttackSpawn meleeAttackSpawn = meleeAttack.Spawn;
			switch (meleeAttackSpawn)
			{
				case AttackSpawn.Pivot:
					Instantiate(_meleeAttacks[_currentMeleeAttackIndex], _attackPivot.position, _attackPivot.rotation, attackParent);
					break;
				case AttackSpawn.Point:
					Instantiate(_meleeAttacks[_currentMeleeAttackIndex], _meleeAttackSpawnPoint.position, _meleeAttackSpawnPoint.rotation, attackParent);
					break;
				default:
					break;
			}

			// Instantiate melee attack at melee attack spawn transform.
			_currentMeleeAttackIndex++;
			if (_currentMeleeAttackIndex > _meleeAttacks.Count - 1) _currentMeleeAttackIndex = 0;

			// Start cooldown timer and time between attack timer.
			_meleeComboTimer = _meleeComboTimeLimit;
			_timeBetweenMeleeAttacksTimer = _timeBetweenMeleeAttacks.value;
			_canMeleeAttack = false;
		}
		#endregion

		private void CountdownMeleeComboTimer()
		{
			if (_meleeComboTimer < 0) _currentMeleeAttackIndex = 0;
			else _meleeComboTimer -= Time.deltaTime;
		}
		private void CountdownTimeBetweenMeleeAttacks()
		{
			if (_timeBetweenMeleeAttacksTimer < 0) _canMeleeAttack = true;
			else _timeBetweenMeleeAttacksTimer -= Time.deltaTime;
		}
		private void CountdownTimeMeleeInputBuffer()
		{
			if (_controls.Combat.Melee.ReadValue<float>() >= 0.5f) _meleeBufferTimer = _meleeBufferDuration;

			if (_meleeBufferTimer < 0)
			{
				_meleeRequireInput = true;
			}
			else
			{
				_meleeRequireInput = false;
				_meleeBufferTimer -= Time.deltaTime;
			}
		}
	}
}
