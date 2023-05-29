using Model.DI;
using UnityEngine;
using ViewModel.Infrastructure;

namespace View.SceneEntity
{
	public class PlayerResources : MonoBehaviour
	{
		private IPlayerProgressViewModel _playerProgress;

		private void Awake()
		{
			_playerProgress = ServiceLocator.Container.GetSingle<IPlayerProgressViewModel>();
		}

		public void SellSand() =>
			_playerProgress.SellSand();
	}
}