using Model.DI;
using UnityEngine;
using ViewModel.Infrastructure;
using ViewModel.Infrastructure.Services.DataViewModel;

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