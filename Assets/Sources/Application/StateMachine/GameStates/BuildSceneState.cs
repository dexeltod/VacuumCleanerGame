using System;
using Sources.ApplicationServicesInterfaces.StateMachineInterfaces;
using Sources.DomainInterfaces;
using Sources.Infrastructure.Factories.Player;
using Sources.Infrastructure.Presenters;
using Sources.InfrastructureInterfaces;
using Sources.InfrastructureInterfaces.Scene;
using Sources.PresentationInterfaces;
using Sources.Services.Interfaces;
using Sources.ServicesInterfaces;
using Sources.ServicesInterfaces.UI;
using Sources.UseCases.Scene;
using Sources.Utils.Configs.Scripts;
using UnityEngine;
using VContainer;

namespace Sources.Application.StateMachine.GameStates
{
	public sealed class BuildSceneState : IGameState
	{
#region Fields

		private readonly GameStateMachine _gameStateMachine;
		private readonly ICoroutineRunner _coroutineRunner;
		private readonly ILocalizationService _leanLocalization;

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
		private readonly LevelChangerPresenter _levelChangerPresenter;

#endregion

		[Inject]
		public BuildSceneState(

#region Params

			GameStateMachine gameStateMachine,
			ICoroutineRunner coroutineRunner,
			ILocalizationService leanLocalization,
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
			LevelChangerPresenter levelChangerPresenter

#endregion

		)
		{
#region Construction

			_gameStateMachine = gameStateMachine ?? throw new ArgumentNullException(nameof(gameStateMachine));
			_coroutineRunner = coroutineRunner ?? throw new ArgumentNullException(nameof(coroutineRunner));
			_leanLocalization = leanLocalization ?? throw new ArgumentNullException(nameof(leanLocalization));
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
			_levelChangerPresenter
				= levelChangerPresenter ?? throw new ArgumentNullException(nameof(levelChangerPresenter));

#endregion
		}

		public void Exit() { }

		public void Enter()
		{
			Build();
			_leanLocalization.UpdateTranslations();
			_gameStateMachine.Enter<GameLoopState>();
		}

		private void Build()
		{
			GameObject initialPoint = GameObject.FindWithTag(ConstantNames.PlayerSpawnPointTag);

			IGameplayInterfaceView gameplayInterfaceView = _uiFactory.Instantiate();

			_levelChangerPresenter.SetButton(gameplayInterfaceView);

			GameObject playerGameObject = _playerFactory.Create(
				initialPoint,
				_uiFactory.GameplayInterface.Joystick,
				_playerStats
			);

			ISandParticleSystem particleSystem = playerGameObject.GetComponentInChildren<ISandParticleSystem>();

			ISandContainerView sandContainerView = playerGameObject.GetComponent<ISandContainerView>();

			new SandContainerPresenter(
				_persistentProgress.GameProgress.ResourcesModel,
				sandContainerView,
				_resourcesProgress as IResourceProgressEventHandler,
				particleSystem,
				_coroutineRunner
			);

			_upgradeWindowFactory.Create();
			_cameraFactory.CreateVirtualCamera();
		}
	}
}