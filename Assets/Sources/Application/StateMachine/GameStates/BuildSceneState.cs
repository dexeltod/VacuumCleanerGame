using System;
using Cysharp.Threading.Tasks;
using Sources.ApplicationServicesInterfaces.StateMachineInterfaces;
using Sources.DomainInterfaces;
using Sources.Infrastructure.Factories;
using Sources.Infrastructure.Factories.Player;
using Sources.Infrastructure.Presenters;
using Sources.InfrastructureInterfaces;
using Sources.InfrastructureInterfaces.Scene;
using Sources.Presentation;
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
		private readonly IAssetProvider _assetProvider;
		private readonly LevelChangerPresenter _levelChangerPresenter;
		private readonly CoroutineRunnerFactory _coroutineRunnerFactory;
		private IUpgradeWindowPresenter _upgradeWindowPresenter;

#endregion

		public BuildSceneState(

#region Params

			GameStateMachine gameStateMachine,
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
			IAssetProvider assetProvider,
			LevelChangerPresenter levelChangerPresenter,
			CoroutineRunnerFactory coroutineRunnerFactory

#endregion

		)
		{
#region Construction

			_gameStateMachine = gameStateMachine ?? throw new ArgumentNullException(nameof(gameStateMachine));
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
			_assetProvider = assetProvider ?? throw new ArgumentNullException(nameof(assetProvider));
			_levelChangerPresenter
				= levelChangerPresenter ?? throw new ArgumentNullException(nameof(levelChangerPresenter));
			_coroutineRunnerFactory = coroutineRunnerFactory ??
				throw new ArgumentNullException(nameof(coroutineRunnerFactory));

#endregion
		}

		public void Enter()
		{
			Build();
			_leanLocalization.UpdateTranslations();
			_gameStateMachine.Enter<GameLoopState>();
		}

		public void Exit() =>
			_upgradeWindowPresenter.Disable();

		private void Build()
		{
			var coroutineRunner = _coroutineRunnerFactory.Create();
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
				coroutineRunner
			);

			IUpgradeWindow upgradeWindow = _upgradeWindowFactory.Create();

			_upgradeWindowPresenter = new UpgradeWindowPresenterFactory(
				_assetProvider,
				upgradeWindow,
				_progressLoadDataService
			).Create();

			_upgradeWindowPresenter.Enable();

			_cameraFactory.CreateVirtualCamera();
		}
	}

	public class UpgradeWindowPresenterFactory
	{
		private readonly IAssetProvider _assetProvider;
		private readonly IUpgradeWindow _upgradeWindow;
		private readonly IProgressLoadDataService _progressLoadDataService;

		public UpgradeWindowPresenterFactory(
			IAssetProvider assetProvider,
			IUpgradeWindow upgradeWindow,
			IProgressLoadDataService progressLoadDataService
		)
		{
			_assetProvider = assetProvider ?? throw new ArgumentNullException(nameof(assetProvider));
			_upgradeWindow = upgradeWindow ?? throw new ArgumentNullException(nameof(upgradeWindow));
			_progressLoadDataService = progressLoadDataService ??
				throw new ArgumentNullException(nameof(progressLoadDataService));
		}

		public IUpgradeWindowPresenter Create()
		{
			UpgradeTriggerObserver upgradeTrigger = _assetProvider.InstantiateAndGetComponent<UpgradeTriggerObserver>(
				ResourcesAssetPath.GameObjects.UpgradeTrigger
			);

			return new UpgradeWindowPresenter(
				upgradeTrigger,
				_upgradeWindow,
				_progressLoadDataService
			);
		}
	}

	public interface IUpgradeWindowPresenter
	{
		public void Enable();
		public void Disable();
	}

	public class UpgradeWindowPresenter : IUpgradeWindowPresenter, IDisposable
	{
		private readonly IUpgradeWindowGetter _upgradeWindowGetter;
		private readonly IUpgradeWindow _upgradeWindow;
		private readonly IProgressLoadDataService _progressLoadService;
		private readonly UpgradeTriggerObserver _observer;
		private bool _isCanSave;

		public UpgradeWindowPresenter(
			UpgradeTriggerObserver observer,
			IUpgradeWindow upgradeWindow,
			IProgressLoadDataService progressLoadDataService
		)
		{
			_upgradeWindow = upgradeWindow ?? throw new ArgumentNullException(nameof(upgradeWindow));
			_progressLoadService = progressLoadDataService ??
				throw new ArgumentNullException(nameof(progressLoadDataService));
			_observer = observer ? observer : throw new ArgumentNullException(nameof(observer));
		}

		public void Enable() =>
			_observer.TriggerEntered += OnTriggerEnter;

		private async void OnTriggerEnter(bool isEntered)
		{
			_upgradeWindow.SetActiveYesNoButtons(isEntered);

			if (isEntered != false || _isCanSave == false)
				return;

			await _progressLoadService.SaveToCloud(() => _isCanSave = true);
		}

		public void Disable() =>
			_observer.TriggerEntered -= OnTriggerEnter;

		public void Dispose() =>
			Disable();
	}
}