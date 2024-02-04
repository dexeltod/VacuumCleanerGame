using System;
using System.Collections.Generic;
using Sources.Application.StateMachine;
using Sources.Application.StateMachine.GameStates;
using Sources.ApplicationServicesInterfaces;
using Sources.ApplicationServicesInterfaces.StateMachineInterfaces;
using Sources.DomainInterfaces;
using Sources.Infrastructure.Factories;
using Sources.Infrastructure.Factories.Player;
using Sources.Infrastructure.Providers;
using Sources.InfrastructureInterfaces.Factory;
using Sources.InfrastructureInterfaces.Scene;
using Sources.Presentation.SceneEntity;
using Sources.PresentersInterfaces;
using Sources.Services.Localization;
using Sources.ServicesInterfaces;
using Sources.ServicesInterfaces.Advertisement;
using Sources.ServicesInterfaces.DTO;
using VContainer;

namespace Sources.Application.Services
{
	public class GameStateMachineFactory
	{
#region Fields

		private readonly ISceneLoader _sceneLoader;
		private readonly IAssetResolver _assetResolver;
		private readonly ILevelProgressFacade _levelProgressFacade;
		private readonly IGameStateMachine _gameStateMachine;
		private readonly ILevelConfigGetter _levelConfigGetter;
		private readonly ILeaderBoardService _leaderBoardService;
		private readonly IPlayerFactory _playerFactory;
		private readonly IProgressUpgradeFactory _progressUpgradeFactory;
		private readonly IPersistentProgressService _persistentProgressService;
		private readonly IShopProgressProvider _shopProgressProvider;
		private readonly IPlayerProgressProvider _playerProgressProvider;
		private readonly IResourcesProgressPresenter _resourcesProgressPresenter;
		private readonly IUIFactory _uiFactory;
		private readonly ICameraFactory _cameraFactory;
		private readonly ILocalizationService _localizationService;
		private readonly IRegisterWindowLoader _registerWindowLoader;
		private readonly IAdvertisement _advertisement;
		private readonly IPlayerStatsService _playerStatsService;
		private readonly IUpgradeWindowFactory _upgradeWindowFactory;
		private readonly IProgressLoadDataService _progressLoadDataService;
		private readonly ITranslatorService _translatorService;
		private readonly ILevelChangerPresenter _levelChangerPresenter;
		private readonly CoroutineRunnerFactory _coroutineRunnerFactory;
		private readonly GameplayInterfaceProvider _gameplayInterfaceProvider;
		private readonly IUpgradeWindowPresenter _upgradeWindowPresenter;
		private readonly LoadingCurtain _loadingCurtain;

#endregion

		[Inject]
		public GameStateMachineFactory(

#region Params

			ISceneLoader sceneLoader,
			IAssetResolver assetResolver,
			LoadingCurtain loadingCurtain,
			ILevelProgressFacade levelProgressFacade,
			IGameStateMachine gameStateMachine,
			ILevelConfigGetter levelConfigGetter,
			ILeaderBoardService leaderBoardService,
			IPlayerFactory playerFactory,
			IProgressUpgradeFactory progressUpgradeFactory,
			IPersistentProgressService persistentProgressService,
			IShopProgressProvider shopProgressProvider,
			IPlayerProgressProvider playerProgressProvider,
			IResourcesProgressPresenter resourcesProgressPresenter,
			IUIFactory uiFactory,
			ICameraFactory cameraFactory,
			ILocalizationService localizationService,
			IRegisterWindowLoader registerWindowLoader,
			IAdvertisement advertisement,
			IPlayerStatsService playerStatsService,
			IUpgradeWindowFactory upgradeWindowFactory,
			IProgressLoadDataService progressLoadDataService,
			ITranslatorService translatorService,
			CoroutineRunnerFactory coroutineRunnerFactory,
			GameplayInterfaceProvider gameplayInterfaceProvider

#endregion

		)
		{
#region Constructor

			_sceneLoader = sceneLoader ?? throw new ArgumentNullException(nameof(sceneLoader));
			_assetResolver = assetResolver ?? throw new ArgumentNullException(nameof(assetResolver));
			_levelProgressFacade = levelProgressFacade ?? throw new ArgumentNullException(nameof(levelProgressFacade));
			_gameStateMachine = gameStateMachine ?? throw new ArgumentNullException(nameof(gameStateMachine));
			_levelConfigGetter = levelConfigGetter ?? throw new ArgumentNullException(nameof(levelConfigGetter));
			_leaderBoardService = leaderBoardService ?? throw new ArgumentNullException(nameof(leaderBoardService));
			_playerFactory = playerFactory ?? throw new ArgumentNullException(nameof(playerFactory));

			_progressUpgradeFactory = progressUpgradeFactory ??
				throw new ArgumentNullException(nameof(progressUpgradeFactory));

			_persistentProgressService = persistentProgressService ??
				throw new ArgumentNullException(nameof(persistentProgressService));

			_shopProgressProvider
				= shopProgressProvider ?? throw new ArgumentNullException(nameof(shopProgressProvider));

			_playerProgressProvider = playerProgressProvider ??
				throw new ArgumentNullException(nameof(playerProgressProvider));

			_resourcesProgressPresenter = resourcesProgressPresenter ??
				throw new ArgumentNullException(nameof(resourcesProgressPresenter));
			_uiFactory = uiFactory ?? throw new ArgumentNullException(nameof(uiFactory));

			_cameraFactory = cameraFactory ?? throw new ArgumentNullException(nameof(cameraFactory));
			_localizationService = localizationService ?? throw new ArgumentNullException(nameof(localizationService));
			_loadingCurtain = loadingCurtain ? loadingCurtain : throw new ArgumentNullException(nameof(loadingCurtain));

			_registerWindowLoader
				= registerWindowLoader ?? throw new ArgumentNullException(nameof(registerWindowLoader));
			_advertisement = advertisement ?? throw new ArgumentNullException(nameof(advertisement));
			_playerStatsService = playerStatsService ?? throw new ArgumentNullException(nameof(playerStatsService));
			_upgradeWindowFactory = upgradeWindowFactory;
			_progressLoadDataService = progressLoadDataService ??
				throw new ArgumentNullException(nameof(progressLoadDataService));
			_translatorService = translatorService ?? throw new ArgumentNullException(nameof(translatorService));
			_coroutineRunnerFactory = coroutineRunnerFactory ??
				throw new ArgumentNullException(nameof(coroutineRunnerFactory));
			_gameplayInterfaceProvider = gameplayInterfaceProvider ?? throw new ArgumentNullException(nameof(gameplayInterfaceProvider));

#endregion
		}

		public GameStateMachine Create(GameStateMachine gameStateMachine)
		{
			gameStateMachine.Initialize
			(
				new Dictionary<Type, IExitState>
				{
					[typeof(MenuState)] = new MenuState(
						_sceneLoader,
						_loadingCurtain,
						_assetResolver,
						_levelProgressFacade,
						_gameStateMachine,
						_levelConfigGetter,
						_leaderBoardService,
						_registerWindowLoader,
						_advertisement,
						_translatorService,
						_progressLoadDataService
					),

					[typeof(BuildSandState)] = new BuildSandState(
						gameStateMachine,
						_sceneLoader,
						_loadingCurtain,
						_resourcesProgressPresenter,
						_assetResolver
					),
					[typeof(BuildSceneState)] = new BuildSceneState(
						gameStateMachine,
						_uiFactory,
						_playerStatsService,
						_cameraFactory,
						_playerFactory,
						_upgradeWindowFactory,
						_progressLoadDataService,
						_levelConfigGetter,
						_levelProgressFacade,
						_resourcesProgressPresenter,
						_persistentProgressService,
						_assetResolver,
						_levelChangerPresenter,
						_coroutineRunnerFactory,
						_upgradeWindowPresenter,
						_gameplayInterfaceProvider
						
					),

					[typeof(GameLoopState)] = new GameLoopState(
						gameStateMachine,
						_loadingCurtain,
						_gameplayInterfaceProvider,
						_upgradeWindowPresenter,
						_levelChangerPresenter,
						_localizationService
					)
				}
			);

			return gameStateMachine;
		}
	}
}