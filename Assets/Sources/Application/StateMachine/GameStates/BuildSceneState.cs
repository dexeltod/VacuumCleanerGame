using Sources.Application.StateMachineInterfaces;
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
using Sources.Utils.Configs;
using UnityEngine;

namespace Sources.Application.StateMachine.GameStates
{
	public sealed class BuildSceneState : IGameState
	{
		private readonly GameStateMachine _gameStateMachine;
		private readonly ServiceLocator _serviceLocator;

		private ILocalizationService _leanLocalization;
		private ISceneLoadInvoker _sceneLoadInvoker;
		private LevelChangerPresenter _levelChangerPresenter;

		public BuildSceneState(GameStateMachine gameStateMachine, ServiceLocator serviceLocator)
		{
			_gameStateMachine = gameStateMachine;
			_serviceLocator = serviceLocator;
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

			#endregion

			GameObject initialPoint = GameObject.FindWithTag(ConstantNames.PlayerSpawnPointTag);

			IGameplayInterfaceView gameplayInterfaceView = uiFactory.Instantiate();

			_levelChangerPresenter ??= new LevelChangerPresenter
			(
				levelProgressFacade,
				_gameStateMachine,
				levelConfigGetter,
				resourcesProgress,
				progressLoadDataService
			);

			_levelChangerPresenter.SetAction(gameplayInterfaceView);

			GameObject playerGameObject = playerFactory.Create
				(initialPoint, uiFactory.GameplayInterface.Joystick, playerStats);

			ISandContainerView sandContainerView = playerGameObject.GetComponent<ISandContainerView>();

			SandContainerPresenter sandContainerPresenter = new SandContainerPresenter
				(persistentProgress.GameProgress.ResourcesModel, sandContainerView);

			upgradeWindowFactory.Create();
			cameraFactory.CreateVirtualCamera();
		}
	}
}