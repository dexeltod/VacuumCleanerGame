using Sources.Core.DI;
using Sources.Infrastructure.InfrastructureInterfaces;
using UnityEngine;

namespace Sources.View.SceneEntity
{
	public class PlayerResources : MonoBehaviour
	{
		private IResourcesProgressViewModel _resourcesProgress;

		private void Awake()
		{
			_resourcesProgress = ServiceLocator.Container.Get<IResourcesProgressViewModel>();
		}

		public void SellSand() =>
			_resourcesProgress.SellSand();
	}
}