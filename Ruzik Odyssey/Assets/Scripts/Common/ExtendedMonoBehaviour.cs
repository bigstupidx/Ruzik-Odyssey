using UnityEngine;
using RuzikOdyssey.Level;

namespace RuzikOdyssey.Common
{
	public class ExtendedMonoBehaviour : MonoBehaviour
	{
		protected GameHelper Game
		{
			get { return GameHelper.Instance; }
		}

		protected GameModel GlobalModel
		{
			get { return GameModel.Instance; }
		}
	}
}