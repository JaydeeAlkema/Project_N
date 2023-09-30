using UnityEngine;

namespace Assets.Scripts.Interfaces
{
	public interface IHitboxResponder
	{
		void CollisionedWith(Collider2D collider);
	}
}
