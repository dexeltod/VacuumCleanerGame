using System;
using System.Collections.Generic;
using Sources.Application.StateMachine;
using Sources.Application.StateMachine.GameStates;
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
using Sources.ServicesInterfaces;
using Sources.UseCases.Scene;
using VContainer;

namespace Sources.Application
{
	public class GameStateMachineFactory
	{
		private readonly ISceneLoader _sceneLoader;
		private readonly IAssetProvider _assetProvider;
		private readonly ICoroutineRunner _coroutineRunner;
		private readonly ILevelProgressFacade _levelProgressFacade;
		private readonly IGameStateMachine _gameStateMachine;
		private readonly ILevelConfigGetter _levelConfigGetter;
		private readonly ILeaderBoardService _leaderBoardService;
		private readonly IPlayerFactory _playerFactory;
		private readonly IUpgradeDataFactory _upgradeDataFactory;
		private readonly IPersistentProgressService _persistentProgressService;
		private readonly IShopProgressProvider _shopProgressProvider;
		private readonly IPlayerProgressProvider _playerProgressProvider;
		private readonly IResourcesProgressPresenter _resourcesProgressPresenter;
		private readonly IUIGetter _uiGetter;
		private readonly ICameraFactory _cameraFactory;
		private readonly ILocalizationService _localizationService;
		private readonly LoadingCurtain _loadingCurtain;

		[Inject]
		public GameStateMachineFactory(
			ISceneLoader sceneLoader,
			IAssetProvider assetProvider,
			LoadingCurtain loadingCurtain,
			ICoroutineRunner coroutineRunner,
			ILevelProgressFacade levelProgressFacade,
			IGameStateMachine gameStateMachine,
			ILevelConfigGetter levelConfigGetter,
			ILeaderBoardService leaderBoardService,
			IPlayerFactory playerFactory,
			IUpgradeDataFactory upgradeDataFactory,
			IPersistentProgressService persistentProgressService,
			IShopProgressProvider shopProgressProvider,
			IPlayerProgressProvider playerProgressProvider,
			IResourcesProgressPresenter resourcesProgressPresenter,
			IUIGetter uiGetter,
			ICameraFactory cameraFactory,
			ILocalizationService localizationService
		)
		{
			_sceneLoader = sceneLoader ?? throw new ArgumentNullException(nameof(sceneLoader));
			_assetProvider = assetProvider ?? throw new ArgumentNullException(nameof(assetProvider));
			_coroutineRunner = coroutineRunner ?? throw new ArgumentNullException(nameof(coroutineRunner));
			_levelProgressFacade = levelProgressFacade ?? throw new ArgumentNullException(nameof(levelProgressFacade));
			_gameStateMachine = gameStateMachine ?? throw new ArgumentNullException(nameof(gameStateMachine));
			_levelConfigGetter = levelConfigGetter ?? throw new ArgumentNullException(nameof(levelConfigGetter));
			_leaderBoardService = leaderBoardService ?? throw new ArgumentNullException(nameof(leaderBoardService));
			_playerFactory = playerFactory ?? throw new ArgumentNullException(nameof(playerFactory));
			_upgradeDataFactory = upgradeDataFactory ?? throw new ArgumentNullException(nameof(upgradeDataFactory));
			_persistentProgressService = persistentProgressService ??
				throw new ArgumentNullException(nameof(persistentProgressService));
			_shopProgressProvider
				= shopProgressProvider ?? throw new ArgumentNullException(nameof(shopProgressProvider));
			_playerProgressProvider = playerProgressProvider ??
				throw new ArgumentNullException(nameof(playerProgressProvider));
			_resourcesProgressPresenter = resourcesProgressPresenter ??
				throw new ArgumentNullException(nameof(resourcesProgressPresenter));
			_uiGetter = uiGetter ?? throw new ArgumentNullException(nameof(uiGetter));
			_cameraFactory = cameraFactory ?? throw new ArgumentNullException(nameof(cameraFactory));
			_localizationService = localizationService ?? throw new ArgumentNullException(nameof(localizationService));
			_loadingCurtain = loadingCurtain ? loadingCurtain : throw new ArgumentNullException(nameof(loadingCurtain));
		}

		public GameStateMachine Create()
		{
			GameStateMachine gameStateMachine = new GameStateMachine();

			gameStateMachine.Initialize
			(
				new Dictionary<Type, IExitState>()
				{
					[typeof(MenuState)] = new MenuState(
						_sceneLoader,
						_loadingCurtain,
						_assetProvider,
						_levelProgressFacade,
						_gameStateMachine,
						_levelConfigGetter,
						_leaderBoardService
					),

					[typeof(BuildSandState)] = new BuildSandState(
						gameStateMachine,
						_sceneLoader,
						_loadingCurtain
					),

					[typeof(InitializeServicesWithViewState)]
						= new InitializeServicesWithViewState(
							gameStateMachine,
							_assetProvider,
							_playerFactory,
							_upgradeDataFactory,
							_persistentProgressService,
							_shopProgressProvider,
							_playerProgressProvider,
							_resourcesProgressPresenter,
							_cameraFactory
						),

					// [typeof(BuildSceneState)] = new BuildSceneState(
					// 	gameStateMachine,
					// 	_coroutineRunner,
					// 	_localizationService,
					// ),

					[typeof(GameLoopState)] = new GameLoopState(
						gameStateMachine,
						_loadingCurtain,
						_uiGetter.GameplayInterface
					)
				}
			);

			return gameStateMachine;
		}
	}
}