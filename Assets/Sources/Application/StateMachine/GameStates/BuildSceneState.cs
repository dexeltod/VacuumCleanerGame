using System;
using Sources.ApplicationServicesInterfaces;
using Sources.ApplicationServicesInterfaces.StateMachineInterfaces;
using Sources.DIService;
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

namespace Sources.Application.StateMachine.GameStates
{
	public sealed class BuildSceneState : IGameState
	{
		private readonly GameStateMachine _gameStateMachine;
		private readonly ServiceLocator _serviceLocator;
		private readonly ICoroutineRunner _coroutineRunner;

		private ILocalizationService _leanLocalization;
		private ISceneLoadInvoker _sceneLoadInvoker;
		private LevelChangerPresenter _levelChangerPresenter;

		public BuildSceneState(
			GameStateMachine gameStateMachine,
			ServiceLocator serviceLocator,
			ICoroutineRunner coroutineRunner
		)
		{
			_gameStateMachine = gameStateMachine ?? throw new ArgumentNullException(nameof(gameStateMachine));
			_serviceLocator = serviceLocator ?? throw new ArgumentNullException(nameof(serviceLocator));
			_coroutineRunner = coroutineRunner ?? throw new ArgumentNullException(nameof(coroutineRunner));
		}

		public void Exit() { }

		public void Enter()
		{
			Build();

			_sceneLoadInvoker.Invoke();
			_leanLocalization.UpdateTranslations();
			_gameStateMachine.Enter<GameLoopState>();
		}

		private void Build()
		{
			#region GettingServices

			_leanLocalization = _serviceLocator.Get<ILocalizationService>();
			_sceneLoadInvoker = _serviceLocator.Get<ISceneLoadInvoker>();

			IUIFactory uiFactory = _serviceLocator.Get<IUIFactory>();
			IPlayerStatsService playerStats = _serviceLocator.Get<IPlayerStatsService>();
			ICameraFactory cameraFactory = _serviceLocator.Get<ICameraFactory>();
			IPlayerFactory playerFactory = _serviceLocator.Get<IPlayerFactory>();
			IUpgradeWindowFactory upgradeWindowFactory = _serviceLocator.Get<IUpgradeWindowFactory>();
			IProgressLoadDataService progressLoadDataService = _serviceLocator.Get<IProgressLoadDataService>();

			ILevelConfigGetter levelConfigGetter = _serviceLocator.Get<ILevelConfigGetter>();
			ILevelProgressFacade levelProgressFacade = _serviceLocator.Get<ILevelProgressFacade>();
			IResourcesProgressPresenter resourcesProgress = _serviceLocator.Get<IResourcesProgressPresenter>();
			IPersistentProgressService persistentProgress = _serviceLocator.Get<IPersistentProgressService>();

			IYandexSDKController yandexSDKController = _serviceLocator.Get<IYandexSDKController>();

			#endregion

			GameObject initialPoint = GameObject.FindWithTag(ConstantNames.PlayerSpawnPointTag);

			IGameplayInterfaceView gameplayInterfaceView = uiFactory.Instantiate();

			_levelChangerPresenter ??= new LevelChangerPresenter(
				levelProgressFacade,
				_gameStateMachine,
				levelConfigGetter,
				resourcesProgress,
				progressLoadDataService,
				yandexSDKController
			);

			_levelChangerPresenter.SetButton(gameplayInterfaceView);

			GameObject playerGameObject = playerFactory.Create(
				initialPoint,
				uiFactory.GameplayInterface.Joystick,
				playerStats
			);

			ISandParticleSystem particleSystem = playerGameObject.GetComponentInChildren<ISandParticleSystem>();

			ISandContainerView sandContainerView = playerGameObject.GetComponent<ISandContainerView>();

			new SandContainerPresenter(
				persistentProgress.GameProgress.ResourcesModel,
				sandContainerView,
				resourcesProgress as IResourceProgressEventHandler,
				particleSystem,
				_coroutineRunner
			);

			upgradeWindowFactory.Create();
			cameraFactory.CreateVirtualCamera();
		}
	}
}