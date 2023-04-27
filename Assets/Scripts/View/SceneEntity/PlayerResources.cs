using Model.DI;
using UnityEngine;
using ViewModel;

namespace Presenter.SceneEntity
{
	public class PlayerResources : MonoBehaviour
	{
		private IGameProgressViewModel _gameProgress;

		private void Awake()
		{
			_gameProgress = ServiceLocator.Container.GetSingle<IGameProgressViewModel>();
			
		}

		public void SellSand() => 
			_gameProgress.SellSand();
	}
}