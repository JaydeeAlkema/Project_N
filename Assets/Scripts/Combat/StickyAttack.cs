using NaughtyAttributes;
using UnityEngine;

namespace Assets.Scripts
{
	public class StickyAttack : MonoBehaviour
	{
		[SerializeField] private float _stickTime;
		[SerializeField, ReadOnly] private float _stickTimer;
		void Start()
		{
			_stickTimer = _stickTime;
		}

		void Update()
		{
			if (_stickTimer < 0)
			{
				transform.parent = null;
			}
			else
			{
				_stickTimer -= Time.deltaTime;
			}
		}
	}
}