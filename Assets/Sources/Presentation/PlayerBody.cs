using Sources.PresentationInterfaces;
using UnityEngine;

namespace Sources.Presentation
{
	public class PlayerBody : MonoBehaviour, IPlayer
	{
		public GameObject GameObject { get; private set; }

		private void Awake() =>
			GameObject = gameObject;
	}
}
