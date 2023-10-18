using Sources.Application.StateMachineInterfaces;
using Sources.Application.UI;
using Sources.DIService;
using Sources.DomainInterfaces;
using Sources.Infrastructure;
using Sources.Infrastructure.DataViewModel;
using Sources.Infrastructure.Factories.Player;
using Sources.Infrastructure.Shop;
using Sources.InfrastructureInterfaces;
using Sources.InfrastructureInterfaces.DTO;
using Sources.InfrastructureInterfaces.Factory;
using Sources.PresentationInterfaces;
using Sources.Services;
using Sources.Services.Interfaces;
using Sources.ServicesInterfaces;
using Sources.ServicesInterfaces.UI;
using Sources.View.SceneEntity;

namespace Sources.Application.StateMachine.GameStates
{
	public class InitializeServicesWithProgressState : IGameState
	{
		private readonly IGameStateMachine _gameStateMachine;
		private readonly GameServices      _gameServices;
		private readonly LoadingCurtain    _loadingCurtain;

		public InitializeServicesWithProgressState
		(
			IGameStateMachine gameStateMachine,
			GameServices      gameServices,
			LoadingCurtain    loadingCurtain
		)
		{
			_gameStateMachine = gameStateMachine;
			_gameServices     = gameServices;
			_loadingCurtain   = loadingCurtain;
		}

		public void Enter()
		{
			RegisterServiceAndResources();
			_gameStateMachine.Enter<MenuState>();
		}

		public void Exit() { }

		private void RegisterServiceAndResources()
		{
			IAssetProvider             assetProvider   = _gameServices.Get<IAssetProvider>();
			IShopItemFactory           shopItemFactory = _gameServices.Get<IShopItemFactory>();
			IPersistentProgressService progressService = _gameServices.Get<IPersistentProgressService>();

			IGameplayInterfaceView gameplayInterfaceView = CreateUIServices().GameplayInterface;

			PlayerStatsFactory statsFactory = new PlayerStatsFactory(shopItemFactory, _loadingCurtain);

			_gameServices.Register<IPlayerStatsService>(statsFactory.CreatePlayerStats(progressService));

			_gameServices.Register<IPlayerProgressProvider>
			(
				new PlayerProgressProvider
				(
					GameServices.Container.Get<IPlayerStatsService>()
				)
			);

			IResourcesProgressPresenter resourcesProgressPresenter =
				_gameServices.Register<IResourcesProgressPresenter>
				(
					new ResourcesProgressPresenter(progressService.GameProgress.ResourcesModel, gameplayInterfaceView)
				);

			ILevelProgressPresenter levelProgressPresenter =
				_gameServices.Register<ILevelProgressPresenter>
				(
					new LevelProgressPresenter
						(progressService.GameProgress.LevelProgress, gameplayInterfaceView)
				);

			// LevelChanger levelChanger = new LevelChanger
			// 	(gameplayInterfaceView, levelProgressPresenter, _gameStateMachine);

			_gameServices.Register<IShopProgressProvider>(new ShopProgressProvider());

			CreateUpgradeWindow
			(
				assetProvider,
				shopItemFactory,
				resourcesProgressPresenter,
				progressService
			);

			_gameServices.Register<IPlayerFactory>
			(
				new PlayerFactory
				(
					_gameServices.Get<IAssetProvider>()
				)
			);

			_loadingCurtain.SetText("");
		}

		private void CreateUpgradeWindow
		(
			IAssetProvider              assetProvider,
			IShopItemFactory            shopItemFactory,
			IResourcesProgressPresenter resourcesProgressPresenter,
			IPersistentProgressService  progressService
		)
		{
			UpgradeWindowFactory upgradeWindowFactory = new
			(
				assetProvider,
				shopItemFactory,
				resourcesProgressPresenter,
				progressService.GameProgress
			);

			_gameServices.Register<IUpgradeWindowFactory>(upgradeWindowFactory);
			_gameServices.Register<IUpgradeWindowGetter>(upgradeWindowFactory);
		}

		private IUIGetter CreateUIServices()
		{
			UIFactory uiFactory = new UIFactory();
			_gameServices.Register<IUIFactory>(uiFactory);
			return _gameServices.Register<IUIGetter>(uiFactory);
		}
	}
}