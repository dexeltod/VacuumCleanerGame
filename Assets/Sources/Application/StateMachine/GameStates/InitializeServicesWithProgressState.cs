using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Sources.Application.StateMachineInterfaces;
using Sources.Application.UI;
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

		public async UniTask Enter()
		{
			await RegisterServices();
			await _gameStateMachine.Enter<MenuState>();
		}

		public void Exit()
		{
		}

		private async UniTask RegisterServices()
		{
			IShopItemFactory shopItemFactory = new ShopItemFactory();
			ResourceServiceFactory resourceServiceFactory = new ResourceServiceFactory();

			Dictionary<ResourceType, IResource<int>> intResources = resourceServiceFactory.GetIntResources();
			Dictionary<ResourceType, IResource<float>> floatResources = resourceServiceFactory.GetFloatResources();

			_gameServices.Register<IResourceService>(new ResourcesService(intResources, floatResources));

			await InitProgress
			(_gameServices.Get<ISaveLoadDataService>(),
				_gameServices.Get<IPersistentProgressService>(), shopItemFactory
			);

			IPersistentProgressService progressService = _gameServices.Get<IPersistentProgressService>();

			PlayerStatsFactory statsFactory = new PlayerStatsFactory(shopItemFactory);

			_gameServices.Register<IPlayerStatsService>(statsFactory.CreatePlayerStats(progressService));

			_gameServices.Register<IPlayerProgressProvider>(new PlayerProgressProvider());
			_gameServices.Register<IResourcesProgressPresenter>(new ResourcesPresenter());
			_gameServices.Register<IShopProgressProvider>(new ShopProgressProvider());

			CreateUIServices();

			UpgradeWindowFactory upgradeWindowFactory = new(shopItemFactory);

			_gameServices.Register<IUpgradeWindowFactory>(upgradeWindowFactory);
			_gameServices.Register<IUpgradeWindowGetter>(upgradeWindowFactory);

			_gameServices.Register<IPlayerFactory>
			(
				new PlayerFactory
				(
					_gameServices.Get<IAssetProvider>()
				)
			);
		}

		private async UniTask InitProgress(ISaveLoadDataService saveLoadService,
			IPersistentProgressService persistentProgressService, IShopItemFactory shopItemFactory)
		{
			ProgressFactory progressFactory = new ProgressFactory
			(
				saveLoadService,
				persistentProgressService,
				shopItemFactory
			);
			
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