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
using Sources.InfrastructureInterfaces.Scene;
using Sources.PresentationInterfaces;
using Sources.Services;
using Sources.Services.Interfaces;
using Sources.ServicesInterfaces;
using Sources.ServicesInterfaces.UI;
using Sources.View.SceneEntity;

namespace Sources.Application.StateMachine.GameStates
{
	public sealed class InitializeServicesWithProgressState : IGameState
	{
		private readonly IGameStateMachine _gameStateMachine;
		private readonly ServiceLocator _serviceLocator;
		private readonly LoadingCurtain _loadingCurtain;

		public InitializeServicesWithProgressState
		(
			IGameStateMachine gameStateMachine,
			ServiceLocator serviceLocator,
			LoadingCurtain loadingCurtain
		)
		{
			_gameStateMachine = gameStateMachine;
			_serviceLocator = serviceLocator;
			_loadingCurtain = loadingCurtain;
		}

		public void Enter()
		{
			RegisterServiceAndResources();
			_gameStateMachine.Enter<MenuState>();
		}

		public void Exit() { }

		private void RegisterServiceAndResources()
		{
			#region GettingServices

			IAssetProvider assetProvider = _serviceLocator.Get<IAssetProvider>();
			IUpgradeDataFactory upgradeDataFactory = _serviceLocator.Get<IUpgradeDataFactory>();
			IPersistentProgressService progressService = _serviceLocator.Get<IPersistentProgressService>();
			IProgressLoadDataService progressLoadDataService = _serviceLocator.Get<IProgressLoadDataService>();

			#endregion

			#region RegisterServices

			PlayerStatsFactory statsFactory = new PlayerStatsFactory(upgradeDataFactory, _loadingCurtain);

			_serviceLocator.Register<IPlayerStatsService>(statsFactory.CreatePlayerStats(progressService));

			_serviceLocator.Register<IPlayerProgressProvider>
			(
				new PlayerProgressProvider
				(
					ServiceLocator.Container.Get<IPlayerStatsService>()
				)
			);

			_serviceLocator.Register<ILevelConfigGetter>(new LevelConfigGetter(assetProvider));

			_serviceLocator.Register<IShopProgressProvider>
			(
				new ShopProgressProvider
				(
					progressService.GameProgress.ShopProgress,
					progressLoadDataService
				)
			);

			_serviceLocator.Register<IPlayerFactory>
			(
				new PlayerFactory
				(
					_serviceLocator.Get<IAssetProvider>()
				)
			);

			ILevelProgressFacade levelProgressFacade =
				_serviceLocator.Register<ILevelProgressFacade>
				(
					new LevelProgressFacade
					(
						progressService.GameProgress.LevelProgress
					)
				);

			#endregion

			_loadingCurtain.SetText("");
		}
	}
}