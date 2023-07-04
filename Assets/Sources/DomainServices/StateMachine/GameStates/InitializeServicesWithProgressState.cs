using Application.DI;
using Infrastructure.Factories;
using Infrastructure.Factories.Player;
using Infrastructure.Services;
using Infrastructure.Services.DataViewModel;
using Infrastructure.Shop;
using InfrastructureInterfaces;

namespace Infrastructure.StateMachine.GameStates
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
			_serviceLocator.RegisterAsSingle<IPlayerStatsService>(new PlayerStats());
			_serviceLocator.RegisterAsSingle<IPlayerProgressViewModel>(new PlayerProgressViewModel());
			_serviceLocator.RegisterAsSingle<IResourcesProgressViewModel>(new ResourcesViewModel());
			_serviceLocator.RegisterAsSingle<IShopProgressViewModel>(new ShopProgressViewModel());

			CreateUIServices();

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