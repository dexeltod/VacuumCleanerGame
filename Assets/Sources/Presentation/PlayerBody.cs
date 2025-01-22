using Sources.PresentationInterfaces;
using UnityEngine;

namespace Sources.Presentation
{
	public class PlayerBody : MonoBehaviour, IPlayer
	{
		private void Awake() =>
			GameObject = gameObject;

		public GameObject GameObject { get; private set; }
	}
}