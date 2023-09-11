using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Sources.Application.StateMachineInterfaces;
using Sources.Application.UI;
using Sources.Application.UnityApplicationServices;
using Sources.ApplicationServicesInterfaces;
using Sources.DIService;
using Sources.DomainInterfaces;
using Sources.DomainInterfaces.DomainServicesInterfaces;
using Sources.Infrastructure;
using Sources.Infrastructure.DataViewModel;
using Sources.Infrastructure.Factories;
using Sources.Infrastructure.Factories.Player;
using Sources.Infrastructure.Factories.UpgradeShop;
using Sources.Infrastructure.Shop;
using Sources.InfrastructureInterfaces.DTO;
using Sources.InfrastructureInterfaces.Factory;
using Sources.Services;
using Sources.Services.DomainServices;
using Sources.Services.Interfaces;
using Sources.ServicesInterfaces;
using Sources.ServicesInterfaces.UI;
using Sources.Utils;
using Sources.View.SceneEntity;
using Unity.Services.Core;

namespace Sources.Application.StateMachine.GameStates
{
	public class InitializeServicesWithProgressState : IGameState
	{
		private readonly GameStateMachine _gameStateMachine;
		private readonly GameServices _gameServices;
		private readonly LoadingCurtain _loadingCurtain;

		public InitializeServicesWithProgressState(GameStateMachine gameStateMachine, GameServices gameServices,
			LoadingCurtain loadingCurtain)
		{
			_gameStateMachine = gameStateMachine;
			_gameServices = gameServices;
			_loadingCurtain = loadingCurtain;
		}

		public async UniTask Enter()
		{
			await RegisterServiceAndResources();
			_loadingCurtain.SetText("Loading menu");
			await _gameStateMachine.Enter<MenuState>();
		}

		public void Exit()
		{
		}

		private async UniTask RegisterServiceAndResources()
		{
			ResourceServiceFactory resourceServiceFactory = new ResourceServiceFactory();
			Dictionary<ResourceType, IResource<int>> intResources = resourceServiceFactory.GetIntResources();
			Dictionary<ResourceType, IResource<float>> floatResources = resourceServiceFactory.GetFloatResources();

			IYandexSDKController yandexSDKController = _gameServices.Get<IYandexSDKController>();
			IPersistentProgressService progressService = _gameServices.Get<IPersistentProgressService>();

			IShopItemFactory shopItemFactory = new ShopItemFactory();

			_gameServices.Register<IResourceService>(new ResourcesService(intResources, floatResources));

			ISaveLoadDataService saveLoadDataService = _gameServices.Get<ISaveLoadDataService>();
			IPersistentProgressService persistentProgressService = _gameServices.Get<IPersistentProgressService>();

			ISaveLoader saveLoader = await GetSaveLoader(yandexSDKController, progressService);

			await InitProgress(saveLoader, saveLoadDataService, persistentProgressService, shopItemFactory);

			PlayerStatsFactory statsFactory = new PlayerStatsFactory(shopItemFactory, _loadingCurtain);

			_gameServices.Register<IPlayerStatsService>(statsFactory.CreatePlayerStats(progressService));

			_gameServices.Register<IPlayerProgressProvider>(new PlayerProgressProvider());
			_gameServices.Register<IResourcesProgressPresenter>(new ResourcesPresenter());
			_gameServices.Register<IShopProgressProvider>(new ShopProgressProvider());

			CreateUIServices();

			UpgradeWindowFactory upgradeWindowFactory = new(shopItemFactory);

			_gameServices.Register<IUpgradeWindowFactory>(upgradeWindowFactory);
			_gameServices.Register<IUpgradeWindowGetter>(upgradeWindowFactory);

			_gameServices.Register<IPlayerFactory>(new PlayerFactory(_gameServices.Get<IAssetProvider>()));
		}

		private async UniTask<ISaveLoader> GetSaveLoader
		(
			IYandexSDKController sdkController,
			IPersistentProgressService progressService
		)
		{
#if YANDEX_GAMES && !UNITY_EDITOR
			return new YandexSaveLoader(sdkController);
#endif
#if UNITY_EDITOR
			EditorSaveLoader saveLoader = await GetEditorSaveLoader(progressService);
			return saveLoader;
#endif
		}

		private async Task<EditorSaveLoader> GetEditorSaveLoader(IPersistentProgressService progressService)
		{
			IUnityServicesController controller = new UnityServicesController(new InitializationOptions());

			await controller.InitializeUnityServices();

			EditorSaveLoader saveLoader = new EditorSaveLoader(progressService, controller);
			await saveLoader.Initialize();
			return saveLoader;
		}

		private async UniTask InitProgress(ISaveLoader saveLoader, ISaveLoadDataService saveLoadService,
			IPersistentProgressService persistentProgressService, IShopItemFactory shopItemFactory)
		{
			ProgressFactory progressFactory = new ProgressFactory
			(
				saveLoader,
				persistentProgressService,
				shopItemFactory
			);

			_loadingCurtain.SetText("Initialization progress");
			await progressFactory.InitProgress();
		}

		private void CreateUIServices()
		{
			UIFactory uiFactory = new UIFactory();
			_gameServices.Register<IUIFactory>(uiFactory);
			_gameServices.Register<IUIGetter>(uiFactory);
		}
	}
}