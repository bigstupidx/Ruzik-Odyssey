using UnityEngine;
using RuzikOdyssey.Common;

namespace RuzikOdyssey.Level
{
	public class PickUp : MonoBehaviour
	{
		public int amount = 1;
		public PickUpType type;


		private void Start()
		{
			Invoke("MoveWithEnvironment", 0.3f);
		}

		private void MoveWithEnvironment()
		{
			gameObject.rigidbody2D.drag = 0.0f;
			gameObject.rigidbody2D.velocity = GameEnvironment.ForegroundSpeed;
		}
	}
}

