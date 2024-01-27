using System;
using System.Collections.Generic;
using Sources.Application.StateMachine;
using Sources.Application.StateMachine.GameStates;
using Sources.Application.YandexSDK;
using Sources.ApplicationServicesInterfaces;
using Sources.ApplicationServicesInterfaces.StateMachineInterfaces;
using Sources.DomainInterfaces;
using Sources.Infrastructure.Factories.Player;
using Sources.InfrastructureInterfaces;
using Sources.InfrastructureInterfaces.DTO;
using Sources.InfrastructureInterfaces.Factory;
using Sources.InfrastructureInterfaces.Scene;
using Sources.Presentation.SceneEntity;
using Sources.PresentationInterfaces;
using Sources.Services.Interfaces;
using Sources.Services.Localization;
using Sources.ServicesInterfaces;
using Sources.ServicesInterfaces.UI;
using Sources.UseCases.Scene;
using VContainer;

namespace Sources.Application
{
	public class GameStateMachineFactory
	{
#region Fields

		private readonly ISceneLoader _sceneLoader;
		private readonly IAssetProvider _assetProvider;
		private readonly ICoroutineRunner _coroutineRunner;
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
		private readonly LoadingCurtain _loadingCurtain;

#endregion

		[Inject]
		public GameStateMachineFactory(

#region Params

			ISceneLoader sceneLoader,
			IAssetProvider assetProvider,
			LoadingCurtain loadingCurtain,
			ICoroutineRunner coroutineRunner,
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
			ITranslatorService translatorService

#endregion

		)
		{
#region Constructor

			_sceneLoader = sceneLoader ?? throw new ArgumentNullException(nameof(sceneLoader));
			_assetProvider = assetProvider ?? throw new ArgumentNullException(nameof(assetProvider));
			_coroutineRunner = coroutineRunner ?? throw new ArgumentNullException(nameof(coroutineRunner));
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
						_assetProvider,
						_levelProgressFacade,
						_gameStateMachine,
						_levelConfigGetter,
						_leaderBoardService,
						_registerWindowLoader,
						_advertisement,
						_translatorService
					),

					[typeof(BuildSandState)] = new BuildSandState(
						gameStateMachine,
						_sceneLoader,
						_loadingCurtain,
						_resourcesProgressPresenter,
						_assetProvider
					),

					[typeof(InitializeServicesWithViewState)]
						= new InitializeServicesWithViewState(
							gameStateMachine,
							_assetProvider,
							_playerFactory,
							_progressUpgradeFactory,
							_persistentProgressService,
							_shopProgressProvider,
							_playerProgressProvider,
							_resourcesProgressPresenter,
							_cameraFactory
						),

					[typeof(BuildSceneState)] = new BuildSceneState(
						gameStateMachine,
						_coroutineRunner,
						_localizationService,
						_uiFactory,
						_playerStatsService,
						_cameraFactory,
						_playerFactory,
						_upgradeWindowFactory,
						_progressLoadDataService,
						_levelConfigGetter,
						_levelProgressFacade,
						_resourcesProgressPresenter,
						_persistentProgressService
					),

					[typeof(GameLoopState)] = new GameLoopState(
						gameStateMachine,
						_loadingCurtain,
						_uiFactory.GameplayInterface
					)
				}
			);

			return gameStateMachine;
		}
	}
}