using System;
using Sources.Application.Bootstrapp;
using Sources.Controllers;
using Sources.Controllers.Mesh;
using Sources.ControllersInterfaces;
using Sources.DomainInterfaces;
using Sources.Infrastructure.Factories;
using Sources.Infrastructure.Factories.CoroutineRunner;
using Sources.Infrastructure.Factories.Presenters;
using Sources.Infrastructure.Factories.Scene;
using Sources.Infrastructure.Factories.UI;
using Sources.Infrastructure.Providers;
using Sources.InfrastructureInterfaces.Factory;
using Sources.InfrastructureInterfaces.Providers;
using Sources.InfrastructureInterfaces.Scene;
using Sources.InfrastructureInterfaces.States;
using Sources.Presentation.SceneEntity;
using Sources.Presentation.UI;
using Sources.PresentationInterfaces;
using Sources.PresentationInterfaces.Player;
using Sources.ServicesInterfaces;
using Sources.UseCases.Scene;
using Sources.Utils.Configs.Scripts;
using Sources.Utils.ConstantNames;
using UnityEngine;

namespace Sources.Infrastructure.StateMachine.GameStates
{
	public sealed class BuildSceneState : IBuildSceneState
	{
#region Fields

		private readonly IGameStateChangerProvider _gameStateMachine;

		private readonly GameplayInterfacePresenterFactory _gameplayInterfacePresenterFactory;
		private readonly IPlayerStatsService _playerStats;
		private readonly ICameraFactory _cameraFactory;
		private readonly IPlayerFactory _playerFactory;
		private readonly IUpgradeWindowViewFactory _upgradeWindowViewFactory;
		private readonly IProgressSaveLoadDataService _progressSaveLoadDataService;
		private readonly IResourcesProgressPresenterProvider _resourcesProgress;
		private readonly IPersistentProgressService _persistentProgress;
		private readonly IAssetFactory _assetFactory;
		private readonly CoroutineRunnerFactory _coroutineRunnerFactory;
		private readonly GameplayInterfaceProvider _gameplayInterfaceProvider;
		private readonly UpgradeWindowPresenterProvider _upgradeWindowPresenterProvider;
		private readonly ResourcesProgressPresenterProvider _resourcesProgressPresenterProvider;
		private readonly ResourcesProgressPresenterFactory _resourcesProgressPresenterFactory;
		private readonly ResourcePathConfigProvider _resourcePathConfigProvider;
		private readonly ICoroutineRunnerProvider _coroutineRunnerProvider;
		private readonly ISceneLoader _sceneLoader;
		private readonly SandCarContainerViewProvider _sandCarContainerViewProvider;
		private readonly ILevelConfigGetter _levelConfigGetter;
		private readonly ILevelProgressFacade _levelProgressFacade;

		private IUpgradeWindowPresenter UpgradeWindowPresenter => _upgradeWindowPresenterProvider.Implementation;

		private GameObject SpawnPoint => _resourcePathConfigProvider.Implementation.SceneGameObjects.SpawnPoint;

		private IGameplayInterfaceView GameplayInterface => _gameplayInterfaceProvider.Implementation;
		private ResourcesPrefabs ResourcesPrefabs => _resourcePathConfigProvider.Implementation;
		private GameObject SellTrigger => ResourcesPrefabs.Triggers.SellTrigger;

#endregion

		public BuildSceneState(

#region Params

			IGameStateChangerProvider gameStateMachine,
			GameplayInterfacePresenterFactory uiFactory,
			IPlayerStatsService playerStats,
			ICameraFactory cameraFactory,
			IPlayerFactory playerFactory,
			IUpgradeWindowViewFactory upgradeWindowViewFactory,
			IProgressSaveLoadDataService progressSaveLoadDataService,
			ILevelConfigGetter levelConfigGetter,
			ILevelProgressFacade levelProgressFacade,
			IResourcesProgressPresenterProvider resourcesProgress,
			IPersistentProgressService persistentProgress,
			IAssetFactory assetFactory,
			CoroutineRunnerFactory coroutineRunnerFactory,
			GameplayInterfaceProvider gameplayInterfaceProvider,
			UpgradeWindowPresenterProvider upgradeWindowPresenterProvider,
			ResourcesProgressPresenterProvider resourcesProgressPresenterProvider,
			ResourcesProgressPresenterFactory resourcesProgressPresenterFactory,
			ResourcePathConfigProvider resourcePathConfigProvider,
			ICoroutineRunnerProvider coroutineRunnerProvider,
			ISceneLoader sceneLoader,
			SandCarContainerViewProvider sandCarContainerViewProvider

#endregion

		)
		{
#region Construction

			_gameStateMachine = gameStateMachine ?? throw new ArgumentNullException(nameof(gameStateMachine));

			_gameplayInterfacePresenterFactory = uiFactory ?? throw new ArgumentNullException(nameof(uiFactory));
			_playerStats = playerStats ?? throw new ArgumentNullException(nameof(playerStats));
			_cameraFactory = cameraFactory ?? throw new ArgumentNullException(nameof(cameraFactory));
			_playerFactory = playerFactory ?? throw new ArgumentNullException(nameof(playerFactory));
			_upgradeWindowViewFactory
				= upgradeWindowViewFactory ?? throw new ArgumentNullException(nameof(upgradeWindowViewFactory));
			_progressSaveLoadDataService = progressSaveLoadDataService ??
				throw new ArgumentNullException(nameof(progressSaveLoadDataService));
			_levelConfigGetter = levelConfigGetter ?? throw new ArgumentNullException(nameof(levelConfigGetter));
			_levelProgressFacade = levelProgressFacade ?? throw new ArgumentNullException(nameof(levelProgressFacade));
			_resourcesProgress = resourcesProgress ?? throw new ArgumentNullException(nameof(resourcesProgress));
			_persistentProgress = persistentProgress ?? throw new ArgumentNullException(nameof(persistentProgress));
			_assetFactory = assetFactory ?? throw new ArgumentNullException(nameof(assetFactory));
			_coroutineRunnerFactory = coroutineRunnerFactory ??
				throw new ArgumentNullException(nameof(coroutineRunnerFactory));
			_gameplayInterfaceProvider = gameplayInterfaceProvider ??
				throw new ArgumentNullException(nameof(gameplayInterfaceProvider));
			_upgradeWindowPresenterProvider
				= upgradeWindowPresenterProvider ??
				throw new ArgumentNullException(nameof(upgradeWindowPresenterProvider));
			_resourcesProgressPresenterProvider = resourcesProgressPresenterProvider ??
				throw new ArgumentNullException(nameof(resourcesProgressPresenterProvider));
			_resourcesProgressPresenterFactory = resourcesProgressPresenterFactory ??
				throw new ArgumentNullException(nameof(resourcesProgressPresenterFactory));

			_resourcePathConfigProvider = resourcePathConfigProvider ??
				throw new ArgumentNullException(nameof(resourcePathConfigProvider));
			_coroutineRunnerProvider = coroutineRunnerProvider ??
				throw new ArgumentNullException(nameof(coroutineRunnerProvider));
			_sceneLoader = sceneLoader ?? throw new ArgumentNullException(nameof(sceneLoader));
			_sandCarContainerViewProvider = sandCarContainerViewProvider ??
				throw new ArgumentNullException(nameof(sandCarContainerViewProvider));

#endregion
		}

		public async void Enter(LevelConfig payload)
		{
			await _sceneLoader.Load("Game");
			Build();

			_gameStateMachine.Implementation.Enter<GameLoopState>();
		}

		private void Build()
		{
			_assetFactory.Instantiate(SellTrigger);
			_coroutineRunnerProvider.Register(_coroutineRunnerFactory.Create());

			_gameplayInterfacePresenterFactory.Create();

			GameObject playerGameObject = _playerFactory.Create(
				SpawnPoint,
				GameplayInterface.Joystick,
				_playerStats
			);

			ResourcesProgressPresenter resourcesProgressPresenter = _resourcesProgressPresenterFactory.Create();
			_resourcesProgressPresenterProvider.Register<IResourcesProgressPresenter>(resourcesProgressPresenter);

			SandCarContainerView sandContainerView = playerGameObject.GetComponent<SandCarContainerView>();
			_sandCarContainerViewProvider.Register<ISandContainerView>(sandContainerView);

			ISandParticleSystem particleSystem = playerGameObject.GetComponentInChildren<ISandParticleSystem>();

			RegisterUpgradeWindowPresenterProvider();

			IMeshModifiable meshModifiable = new SandFactory(_assetFactory).Create();
			IMeshDeformationPresenter presenter = new MeshDeformationController(
				meshModifiable,
				resourcesProgressPresenter
			);

			_cameraFactory.CreateVirtualCamera();
		}

		private void RegisterUpgradeWindowPresenterProvider()
		{
			IUpgradeWindowPresenter presenter = new UpgradeWindowPresenterFactory(
				_upgradeWindowViewFactory,
				_assetFactory,
				_progressSaveLoadDataService,
				_persistentProgress
			).Create();

			_upgradeWindowPresenterProvider.Register(presenter);

			UpgradeWindowPresenter.Enable();
		}

		public void Exit() { }
	}
}