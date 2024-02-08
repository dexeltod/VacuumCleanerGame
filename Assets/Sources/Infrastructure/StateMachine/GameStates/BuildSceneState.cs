using System;
using Sources.Controllers;
using Sources.Controllers.Mesh;
using Sources.ControllersInterfaces;
using Sources.DomainInterfaces;
using Sources.Infrastructure.Factories;
using Sources.Infrastructure.Factories.Presenters;
using Sources.Infrastructure.Factories.Scene;
using Sources.Infrastructure.Providers;
using Sources.InfrastructureInterfaces.Factory;
using Sources.InfrastructureInterfaces.Presenters;
using Sources.InfrastructureInterfaces.Providers;
using Sources.InfrastructureInterfaces.Scene;
using Sources.InfrastructureInterfaces.States;
using Sources.PresentationInterfaces;
using Sources.PresentationInterfaces.Player;
using Sources.ServicesInterfaces;
using Sources.Utils.Configs.Scripts;
using Sources.Utils.ConstantNames;
using UnityEngine;

namespace Sources.Infrastructure.StateMachine.GameStates
{
	public sealed class BuildSceneState : IBuildSceneState
	{
#region Fields

		private readonly IGameStateChangerProvider _gameStateMachine;

		private readonly IUIFactory _uiFactory;
		private readonly IPlayerStatsService _playerStats;
		private readonly ICameraFactory _cameraFactory;
		private readonly IPlayerFactory _playerFactory;
		private readonly IUpgradeWindowViewFactory _upgradeWindowViewFactory;
		private readonly IProgressSaveLoadDataService _progressSaveLoadDataService;
		private readonly ILevelConfigGetter _levelConfigGetter;
		private readonly ILevelProgressFacade _levelProgressFacade;
		private readonly IResourcesProgressPresenter _resourcesProgress;
		private readonly IPersistentProgressService _persistentProgress;
		private readonly IAssetFactory _assetFactory;
		private readonly CoroutineRunnerFactory _coroutineRunnerFactory;
		private readonly GameplayInterfaceProvider _gameplayInterfaceProvider;
		private readonly UpgradeWindowPresenterProvider _upgradeWindowPresenterProvider;
		private readonly SandContainerPresenterProvider _sandContainerPresenterProvider;
		private readonly ResourcesProgressPresenterProvider _resourcesProgressPresenterProvider;
		private readonly ResourcesProgressPresenterFactory _resourcesProgressPresenterFactory;

		private SandContainerPresenter _sandContainerPresenter;

		private IUpgradeWindowPresenter UpgradeWindowPresenter => _upgradeWindowPresenterProvider.Instance;
		private IGameplayInterfaceView GameplayInterface => _gameplayInterfaceProvider.Instance;

#endregion

		public BuildSceneState(

#region Params

			IGameStateChangerProvider gameStateMachine,
			IUIFactory uiFactory,
			IPlayerStatsService playerStats,
			ICameraFactory cameraFactory,
			IPlayerFactory playerFactory,
			IUpgradeWindowViewFactory upgradeWindowViewFactory,
			IProgressSaveLoadDataService progressSaveLoadDataService,
			ILevelConfigGetter levelConfigGetter,
			ILevelProgressFacade levelProgressFacade,
			IResourcesProgressPresenter resourcesProgress,
			IPersistentProgressService persistentProgress,
			IAssetFactory assetFactory,
			CoroutineRunnerFactory coroutineRunnerFactory,
			GameplayInterfaceProvider gameplayInterfaceProvider,
			UpgradeWindowPresenterProvider upgradeWindowPresenterProvider,
			SandContainerPresenterProvider sandContainerPresenterProvider,
			ResourcesProgressPresenterProvider resourcesProgressPresenterProvider,
			ResourcesProgressPresenterFactory resourcesProgressPresenterFactory

#endregion

		)
		{
#region Construction

			_gameStateMachine = gameStateMachine ?? throw new ArgumentNullException(nameof(gameStateMachine));

			_uiFactory = uiFactory ?? throw new ArgumentNullException(nameof(uiFactory));
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
			_sandContainerPresenterProvider = sandContainerPresenterProvider ??
				throw new ArgumentNullException(nameof(sandContainerPresenterProvider));
			_resourcesProgressPresenterProvider = resourcesProgressPresenterProvider ??
				throw new ArgumentNullException(nameof(resourcesProgressPresenterProvider));
			_resourcesProgressPresenterFactory = resourcesProgressPresenterFactory ??
				throw new ArgumentNullException(nameof(resourcesProgressPresenterFactory));

#endregion
		}

		public void Enter(LevelConfig payload)
		{
			Build();

			_gameStateMachine.Instance.Enter<GameLoopState>();
		}

		private void Build()
		{
			//TODO: need to refactor. Need to load spawn point from resources. 
			GameObject spawnPoint = GameObject.FindWithTag(ConstantNames.PlayerSpawnPointTag);

			InitializeGameplayInterface();

			GameObject playerGameObject = _playerFactory.Create(
				spawnPoint,
				GameplayInterface.Joystick,
				_playerStats
			);

			ISandParticleSystem particleSystem = playerGameObject.GetComponentInChildren<ISandParticleSystem>();
			ISandContainerView sandContainerView = playerGameObject.GetComponent<ISandContainerView>();

			RegisterSandContainerProvider(sandContainerView, particleSystem);

			RegisterUpgradeWindowPresenterProvider();

			_resourcesProgressPresenterProvider.Register(_resourcesProgressPresenterFactory.Create());

			IMeshModifiable meshModifiable = new SandFactory(_assetFactory).Create();
			IMeshDeformationPresenter presenter = new MeshDeformationPresenter(meshModifiable, _resourcesProgress);

			new MeshPresenter(presenter, _resourcesProgressPresenterProvider.Instance);

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

		private void RegisterSandContainerProvider(
			ISandContainerView sandContainerView,
			ISandParticleSystem particleSystem
		)
		{
			SandContainerPresenter presenter = new SandContainerPresenterFactory(
				_persistentProgress.GameProgress.ResourcesModel,
				sandContainerView,
				_resourcesProgress as IResourceProgressEventHandler,
				particleSystem,
				_coroutineRunnerFactory
			).Create();

			_sandContainerPresenterProvider.Register(presenter);
		}

		private void InitializeGameplayInterface() =>
			_gameplayInterfaceProvider.Register(_uiFactory.Create());

		public void Exit() { }
	}
}