using Cysharp.Threading.Tasks;
using Sources.Core.Application.StateMachineInterfaces;
using Sources.Core.DI;
using Sources.DomainServices;
using Sources.DomainServices.Interfaces;
using Sources.Infrastructure.DataViewModel;
using Sources.Infrastructure.Factories;
using Sources.Infrastructure.Factories.Player;
using Sources.Infrastructure.InfrastructureInterfaces;
using Sources.Infrastructure.Services;
using Sources.Infrastructure.Services.Interfaces;
using Sources.Infrastructure.Shop;
using Sources.View.Services.UI;

namespace Sources.Core.Application.StateMachine.GameStates
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

		public async void Enter()
		{
			await RegisterServices();
			_gameStateMachine.Enter<MenuState>();
		}

		public void Exit()
		{
		}

		private async UniTask RegisterServices()
		{
			await InitProgress(_serviceLocator.GetSingle<ISaveLoadDataService>(), _serviceLocator.GetSingle<IPersistentProgressService>());
			
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

		private async UniTask InitProgressAsync(ISaveLoadDataService saveLoadService, IPersistentProgressService persistentProgressService)
		{
			var progressFactory = new ProgressFactory(saveLoadService, persistentProgressService);
			await progressFactory.InitProgress();
		}

		private async UniTask InitProgress(ISaveLoadDataService saveLoadService, IPersistentProgressService persistentProgressService)
		{
			await InitProgressAsync(saveLoadService, persistentProgressService);
		}
		
		private void CreateUIServices()
		{
			UIFactory uiFactory = new UIFactory();
			_serviceLocator.RegisterAsSingle<IUIFactory>(uiFactory);
			_serviceLocator.RegisterAsSingle<IUIGetter>(uiFactory);
		}
	}
}