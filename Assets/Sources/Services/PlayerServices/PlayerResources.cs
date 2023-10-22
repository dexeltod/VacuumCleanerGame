using Sources.DIService;
using Sources.ServicesInterfaces;
using UnityEngine;

namespace Sources.Services.PlayerServices
{
	public class PlayerResources : MonoBehaviour
	{
		private IResourcesProgressPresenter _resourcesProgress;

		private void Awake()
		{
			_resourcesProgress = ServiceLocator.Container.Get<IResourcesProgressPresenter>();
		}

		public void SellSand() =>
			_resourcesProgress.SellSand();
	}
}