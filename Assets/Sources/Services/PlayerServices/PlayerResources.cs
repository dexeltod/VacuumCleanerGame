
using Sources.ServicesInterfaces;
using UnityEngine;
using VContainer;

namespace Sources.Services.PlayerServices
{
	public class PlayerResources : MonoBehaviour
	{
		private IResourcesProgressPresenter _resourcesProgress;

		[Inject]
		private void Construct(IResourcesProgressPresenter resourcesProgressPresenter) =>
			_resourcesProgress = resourcesProgressPresenter;

		public void SellSand() =>
			_resourcesProgress.SellSand();
	}
}