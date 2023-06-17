using Model.DI;
using ViewModel.Infrastructure.Services;
using ViewModel.Infrastructure.Services.DataViewModel;
using ViewModel.Infrastructure.Services.Factories;
using ViewModel.Infrastructure.Services.Factories.Player;
using ViewModel.Infrastructure.StateMachine;
using ViewModel.Infrastructure.StateMachine.GameStates;

namespace ViewModel.Infrastructure
{
	public class InitializeServicesWithProgressState : IGameState
	{
		private readonly GameStateMachine _gameStateMachine;
		private readonly ServiceLocator _serviceLocator;

		public InitializeServicesWithProgressState(GameStateMachine gameStateMachine, ServiceLocator serviceLocator)
		{
			_gameStateMachine = gameStateMachine;
			_serviceLocator = serviceLocator;
		}

		public void Enter()
		{
			RegisterServices();
			_gameStateMachine.Enter<MenuState>();
		}

		public void Exit()
		{
		}

		private void RegisterServices()
		{
			_serviceLocator.RegisterAsSingle<IPlayerProgressViewModel>(new PlayerProgressViewModel());
			_serviceLocator.RegisterAsSingle<IResourcesProgressViewModel>(new ResourcesProgressViewModel());
			_serviceLocator.RegisterAsSingle<IShopProgressViewModel>(new ShopProgressViewModel());

			CreateUIServices();

			_serviceLocator.RegisterAsSingle<IInputService>(new InputService());

			UpgradeWindowFactory upgradeWindowFactory = new();

			_serviceLocator.RegisterAsSingle<IUpgradeWindowFactory>(upgradeWindowFactory);
			_serviceLocator.RegisterAsSingle<IUpgradeWindowGetter>(upgradeWindowFactory);
			_serviceLocator.RegisterAsSingle<IPlayerFactory>(
				new PlayerFactory(_serviceLocator.GetSingle<IAssetProvider>()));
		}

		private void CreateUIServices()
		{
			UIFactory uiFactory = new UIFactory();
			_serviceLocator.RegisterAsSingle<IUIFactory>(uiFactory);
			_serviceLocator.RegisterAsSingle<IUIGetter>(uiFactory);
		}
	}
}