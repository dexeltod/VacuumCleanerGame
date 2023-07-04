using Application.DI;
using InfrastructureInterfaces;
using UnityEngine;

namespace View.SceneEntity
{
	public class PlayerResources : MonoBehaviour
	{
		private IResourcesProgressViewModel _resourcesProgress;

		private void Awake()
		{
			_resourcesProgress = ServiceLocator.Container.GetSingle<IResourcesProgressViewModel>();
		}

		public void SellSand() =>
			_resourcesProgress.SellSand();
	}
}