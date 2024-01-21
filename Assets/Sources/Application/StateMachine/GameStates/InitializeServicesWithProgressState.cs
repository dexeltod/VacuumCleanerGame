using System;
using Sources.ApplicationServicesInterfaces.StateMachineInterfaces;
using Sources.DomainInterfaces;
using Sources.Infrastructure;
using Sources.Infrastructure.DataViewModel;
using Sources.Infrastructure.Factories.Player;
using Sources.Infrastructure.Shop;
using Sources.InfrastructureInterfaces;
using Sources.InfrastructureInterfaces.DTO;
using Sources.InfrastructureInterfaces.Factory;
using Sources.InfrastructureInterfaces.Scene;
using Sources.Presentation.SceneEntity;
using Sources.ServicesInterfaces;
using VContainer;

namespace Sources.Application.StateMachine.GameStates
{
	// public sealed class InitializeServicesWithProgressState : IGameState
	// {
	// 	private readonly IGameStateMachine _gameStateMachine;
	// 	private readonly IObjectResolver _serviceLocator;
	// 	private readonly IContainerBuilder _builder;
	// 	private readonly LoadingCurtain _loadingCurtain;
	//
	// 	public InitializeServicesWithProgressState(
	// 		IGameStateMachine gameStateMachine,
	// 		LoadingCurtain loadingCurtain
	// 	)
	// 	{
	// 		_gameStateMachine = gameStateMachine ?? throw new ArgumentNullException(nameof(gameStateMachine));
	// 		_loadingCurtain = loadingCurtain ? loadingCurtain : throw new ArgumentNullException(nameof(loadingCurtain));
	// 	}
	//
	// 	public void Enter()
	// 	{
	// 		RegisterServiceAndResources();
	// 		_gameStateMachine.Enter<MenuState>();
	// 	}
	//
	// 	public void Exit() { }
	//
	// 	private void RegisterServiceAndResources()
	// 	{
	// 		#region GettingServices
	//
	// 		IAssetProvider assetProvider = _serviceLocator.Resolve<IAssetProvider>();
	// 		IUpgradeDataFactory upgradeDataFactory = _serviceLocator.Resolve<IUpgradeDataFactory>();
	// 		IPersistentProgressService progressService = _serviceLocator.Resolve<IPersistentProgressService>();
	// 		IProgressLoadDataService progressLoadDataService = _serviceLocator.Resolve<IProgressLoadDataService>();
	//
	// 		#endregion
	//
	// 		#region RegisterServices
	//
	// 		PlayerStatsFactory statsFactory = new PlayerStatsFactory(upgradeDataFactory, _loadingCurtain);
	//
	// 		_builder.RegisterInstance<IPlayerStatsService>(statsFactory.CreatePlayerStats(progressService));
	//
	// 		_builder.Register<IPlayerProgressProvider, PlayerProgressProvider>(Lifetime.Singleton);
	//
	// 		_builder.RegisterInstance<ILevelConfigGetter>(new LevelConfigGetter(assetProvider));
	//
	// 		_builder.RegisterInstance<IShopProgressProvider>
	// 		(
	// 			new ShopProgressProvider(
	// 				progressService.GameProgress.ShopProgress,
	// 				progressLoadDataService
	// 			)
	// 		);
	//
	// 		_builder.RegisterInstance<IPlayerFactory>
	// 		(
	// 			new PlayerFactory
	// 			(
	// 				_serviceLocator.Resolve<IAssetProvider>()
	// 			)
	// 		);
	//
	// 		_builder.RegisterInstance<ILevelProgressFacade>(
	// 			new LevelProgressFacade(
	// 				progressService.GameProgress.LevelProgress
	// 			)
	// 		);
	//
	// 		#endregion
	// 	}
	// }
}