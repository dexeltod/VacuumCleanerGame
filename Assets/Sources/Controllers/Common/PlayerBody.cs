using Sources.PresentationInterfaces;
using UnityEngine;

namespace Sources.Controllers.Common
{
	public class PlayerBody : PlayerPresenter, IPlayer
	{
		private void Awake() => GameObject = gameObject;

		public GameObject GameObject { get; private set; }
	}
}