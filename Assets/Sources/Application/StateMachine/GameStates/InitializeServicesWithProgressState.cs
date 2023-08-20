using Sources.Application.StateMachineInterfaces;
using Sources.Application.UI;
using Sources.DIService;
using Sources.Infrastructure.DataViewModel;
using Sources.Infrastructure.Factories;
using Sources.Infrastructure.Factories.Player;
using Sources.Infrastructure.Factories.UpgradeShop;
using Sources.Infrastructure.ScriptableObjects;
using Sources.Infrastructure.Shop;
using Sources.InfrastructureInterfaces;
using Sources.InfrastructureInterfaces.Factory;
using Sources.Services;
using Sources.Services.Interfaces;
using Sources.ServicesInterfaces;
using Sources.ServicesInterfaces.UI;

namespace Sources.Application.StateMachine.GameStates
{
	public class InitializeServicesWithProgressState : IGameState
	{
		private readonly GameStateMachine _gameStateMachine;
		private readonly GameServices _gameServices;

		public InitializeServicesWithProgressState(GameStateMachine gameStateMachine, GameServices gameServices)
		{
			_gameStateMachine = gameStateMachine;
			_gameServices = gameServices;
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
			IShopItemFactory shopItemFactory = new ShopItemFactory();

			InitProgress(_gameServices.Get<ISaveLoadDataService>(),
				_gameServices.Get<IPersistentProgressService>(), shopItemFactory
			);

			IPersistentProgressService progressService = _gameServices.Get<IPersistentProgressService>();
			_gameServices.Register<IPlayerStatsService>(new PlayerStatsService(progressService, shopItemFactory));

			_gameServices.Register<IPlayerProgressViewModel>(new PlayerProgressViewModel());
			_gameServices.Register<IResourcesProgressViewModel>(new ResourcesViewModel());
			_gameServices.Register<IShopProgressViewModel>(new ShopProgressViewModel());

			CreateUIServices();

			UpgradeWindowFactory upgradeWindowFactory = new(shopItemFactory);

			_gameServices.Register<IUpgradeWindowFactory>(upgradeWindowFactory);
			_gameServices.Register<IUpgradeWindowGetter>(upgradeWindowFactory);
			_gameServices.Register<IPlayerFactory>(new PlayerFactory(_gameServices.Get<IResourceProvider>()));
		}

		private void InitProgress(ISaveLoadDataService saveLoadService,
			IPersistentProgressService persistentProgressService, IShopItemFactory shopItemFactory)
		{
			var progressFactory = new ProgressFactory(saveLoadService, persistentProgressService, shopItemFactory);
			progressFactory.InitProgress();
		}

		private void CreateUIServices()
		{
			UIFactory uiFactory = new UIFactory();
			_gameServices.Register<IUIFactory>(uiFactory);
			_gameServices.Register<IUIGetter>(uiFactory);
		}
	}
}