using Sources.DIService;
using Sources.ServicesInterfaces;
using UnityEngine;

namespace Sources.Services.PlayerServices
{
	public class PlayerResources : MonoBehaviour
	{
		private IResourcesProgressViewModel _resourcesProgress;

		private void Awake()
		{
			_resourcesProgress = GameServices.Container.Get<IResourcesProgressViewModel>();
		}

		public void SellSand() =>
			_resourcesProgress.SellSand();
	}
}