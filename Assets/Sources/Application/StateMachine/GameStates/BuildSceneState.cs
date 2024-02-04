using System;
using Sources.ApplicationServicesInterfaces.StateMachineInterfaces;
using Sources.Controllers;
using Sources.DomainInterfaces;
using Sources.Infrastructure.Factories;
using Sources.Infrastructure.Factories.Player;
using Sources.Infrastructure.Providers;
using Sources.InfrastructureInterfaces.Factory;
using Sources.InfrastructureInterfaces.Scene;
using Sources.PresentationInterfaces;
using Sources.PresentationInterfaces.Player;
using Sources.PresentersInterfaces;
using Sources.ServicesInterfaces;
using Sources.Utils.ConstantNames;
using UnityEngine;

namespace Sources.Application.StateMachine.GameStates
{
	public sealed class BuildSceneState : IGameState
	{
#region Fields

		private readonly GameStateMachine _gameStateMachine;

		private readonly IUIFactory _uiFactory;
		private readonly IPlayerStatsService _playerStats;
		private readonly ICameraFactory _cameraFactory;
		private readonly IPlayerFactory _playerFactory;
		private readonly IUpgradeWindowFactory _upgradeWindowFactory;
		private readonly IProgressLoadDataService _progressLoadDataService;
		private readonly ILevelConfigGetter _levelConfigGetter;
		private readonly ILevelProgressFacade _levelProgressFacade;
		private readonly IResourcesProgressPresenter _resourcesProgress;
		private readonly IPersistentProgressService _persistentProgress;
		private readonly IAssetResolver _assetResolver;
		private readonly ILevelChangerPresenter _levelChangerPresenter;
		private readonly CoroutineRunnerFactory _coroutineRunnerFactory;
		private readonly IUpgradeWindowPresenter _upgradeWindowPresenter;
		private readonly GameplayInterfaceProvider _gameplayInterfaceProvider;

		private SandContainerPresenter _sandContainerPresenter;

		private IGameplayInterfaceView GameplayInterface => _gameplayInterfaceProvider.Instance;

#endregion

		public BuildSceneState(

#region Params

			GameStateMachine gameStateMachine,
			IUIFactory uiFactory,
			IPlayerStatsService playerStats,
			ICameraFactory cameraFactory,
			IPlayerFactory playerFactory,
			IUpgradeWindowFactory upgradeWindowFactory,
			IProgressLoadDataService progressLoadDataService,
			ILevelConfigGetter levelConfigGetter,
			ILevelProgressFacade levelProgressFacade,
			IResourcesProgressPresenter resourcesProgress,
			IPersistentProgressService persistentProgress,
			IAssetResolver assetResolver,
			ILevelChangerPresenter levelChangerPresenter,
			CoroutineRunnerFactory coroutineRunnerFactory,
			IUpgradeWindowPresenter upgradeWindowPresenter,
			GameplayInterfaceProvider gameplayInterfaceProvider

#endregion

		)
		{
#region Construction

			_gameStateMachine = gameStateMachine ?? throw new ArgumentNullException(nameof(gameStateMachine));

			_uiFactory = uiFactory ?? throw new ArgumentNullException(nameof(uiFactory));
			_playerStats = playerStats ?? throw new ArgumentNullException(nameof(playerStats));
			_cameraFactory = cameraFactory ?? throw new ArgumentNullException(nameof(cameraFactory));
			_playerFactory = playerFactory ?? throw new ArgumentNullException(nameof(playerFactory));
			_upgradeWindowFactory
				= upgradeWindowFactory ?? throw new ArgumentNullException(nameof(upgradeWindowFactory));
			_progressLoadDataService = progressLoadDataService ??
				throw new ArgumentNullException(nameof(progressLoadDataService));
			_levelConfigGetter = levelConfigGetter ?? throw new ArgumentNullException(nameof(levelConfigGetter));
			_levelProgressFacade = levelProgressFacade ?? throw new ArgumentNullException(nameof(levelProgressFacade));
			_resourcesProgress = resourcesProgress ?? throw new ArgumentNullException(nameof(resourcesProgress));
			_persistentProgress = persistentProgress ?? throw new ArgumentNullException(nameof(persistentProgress));
			_assetResolver = assetResolver ?? throw new ArgumentNullException(nameof(assetResolver));
			_levelChangerPresenter
				= levelChangerPresenter ?? throw new ArgumentNullException(nameof(levelChangerPresenter));
			_coroutineRunnerFactory = coroutineRunnerFactory ??
				throw new ArgumentNullException(nameof(coroutineRunnerFactory));
			_upgradeWindowPresenter = upgradeWindowPresenter ??
				throw new ArgumentNullException(nameof(upgradeWindowPresenter));
			_gameplayInterfaceProvider = gameplayInterfaceProvider ??
				throw new ArgumentNullException(nameof(gameplayInterfaceProvider));

#endregion
		}

		public void Enter()
		{
			Build();

			_gameStateMachine.Enter<GameLoopState>();
		}

		private void Build()
		{
			GameObject initialPoint = GameObject.FindWithTag(ConstantNames.PlayerSpawnPointTag);

			InitializeGameplayInterface();

			GameObject playerGameObject = _playerFactory.Create(
				initialPoint,
				GameplayInterface.Joystick,
				_playerStats
			);

			ISandParticleSystem particleSystem = playerGameObject.GetComponentInChildren<ISandParticleSystem>();
			ISandContainerView sandContainerView = playerGameObject.GetComponent<ISandContainerView>();

			new SandContainerPresenterFactory(
				_persistentProgress.GameProgress.ResourcesModel,
				sandContainerView,
				_resourcesProgress as IResourceProgressEventHandler,
				particleSystem,
				_coroutineRunnerFactory
			).Create();

			new UpgradeWindowPresenterBuilder(
				_upgradeWindowFactory,
				_assetResolver,
				_progressLoadDataService
			).Create();

			_upgradeWindowPresenter.Enable();

			_cameraFactory.CreateVirtualCamera();
		}

		private void InitializeGameplayInterface()
		{
			_gameplayInterfaceProvider.Register(_uiFactory.Instantiate());
			_levelChangerPresenter.SetButton(GameplayInterface);
		}

		public void Exit() { }
	}

	public class SandContainerPresenterFactory
	{
		private readonly IResourcesModel _gameProgressResourcesModel;
		private readonly ISandContainerView _sandContainerView;
		private readonly IResourceProgressEventHandler _resourcesProgress;
		private readonly ISandParticleSystem _particleSystem;
		private readonly CoroutineRunnerFactory _coroutineRunnerFactory;

		public SandContainerPresenterFactory(
			IResourcesModel gameProgressResourcesModel,
			ISandContainerView sandContainerView,
			IResourceProgressEventHandler resourcesProgress,
			ISandParticleSystem particleSystem,
			CoroutineRunnerFactory coroutineRunnerFactory
		)
		{
			_gameProgressResourcesModel = gameProgressResourcesModel;
			_sandContainerView = sandContainerView;
			_resourcesProgress = resourcesProgress;
			_particleSystem = particleSystem;
			_coroutineRunnerFactory = coroutineRunnerFactory;
		}

		public void Create()
		{
			var coroutineRunner = _coroutineRunnerFactory.Create();

			new SandContainerPresenter(
				_gameProgressResourcesModel,
				_sandContainerView,
				_resourcesProgress,
				_particleSystem,
				coroutineRunner
			);
		}
	}
}