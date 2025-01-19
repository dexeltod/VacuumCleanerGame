using System;
using Sources.Boot.Scripts.Factories.Presentation;
using Sources.Boot.Scripts.Factories.Presentation.Scene;
using Sources.Boot.Scripts.Factories.Presenters;
using Sources.BusinessLogic;
using Sources.BusinessLogic.Interfaces;
using Sources.BusinessLogic.Interfaces.Configs;
using Sources.BusinessLogic.Interfaces.Factory;
using Sources.BusinessLogic.Services;
using Sources.BusinessLogic.ServicesInterfaces;
using Sources.BusinessLogic.ServicesInterfaces.Advertisement;
using Sources.BusinessLogic.States;
using Sources.Controllers;
using Sources.ControllersInterfaces;
using Sources.DomainInterfaces;
using Sources.InfrastructureInterfaces.Configs.Scripts;
using Sources.PresentationInterfaces;
using Sources.PresentationInterfaces.Player;
using Sources.Utils;
using UnityEngine;
using VContainer;

namespace Sources.Boot.Scripts.States.StateMachine.GameStates
{
	public sealed class BuildSceneState : IBuildSceneState
	{
		#region Fields

		private readonly IGameStateChanger _gameStateMachine;

		private readonly IGameplayInterfacePresenterFactory _gameplayInterfacePresenterFactory;
		private readonly IPlayerFactory _playerFactory;
		private readonly IUpgradeWindowViewFactory _upgradeWindowViewFactory;
		private readonly IProgressSaveLoadDataService _progressSaveLoadDataService;
		private readonly IPersistentProgressService _persistentProgress;
		private readonly IAssetFactory _assetFactory;
		private readonly IInjectableAssetFactory _injectableAssetFactory;
		private readonly IUpgradeWindowPresenter _upgradeWindowPresenter;
		private readonly IResourcesProgressPresenter _resourcesProgressPresenter;
		private readonly IResourcesProgressPresenterFactory _resourcesProgressPresenterFactory;
		private readonly IResourcesPrefabs _resourcePathNameConfig;
		private readonly ICoroutineRunner _coroutineRunner;
		private readonly ISceneLoader _sceneLoader;
		private readonly ISandContainerView _sandCarContainerView;
		private readonly IDissolveShaderViewController _dissolveShaderViewController;
		private readonly IFillMeshShaderController _fillMeshShaderController;
		private readonly ISandParticleView _sandParticleView;
		private readonly IGameplayInterfacePresenter _gameplayInterfacePresenter;
		private readonly IGameMenuPresenter _gameMenuPresenter;
		private readonly TranslatorService _translatorService;
		private readonly ProgressionConfig _progressionConfig;
		private readonly IContainerBuilder _containerBuilder;
		private readonly IAdvertisement _advertisement;
		private readonly ILevelConfigGetter _levelConfigGetter;
		private readonly ILevelProgressFacade _levelProgressFacade;
		private readonly IGameStateChanger _gameStateChanger;

		#endregion

		[Inject]
		public BuildSceneState(

			#region Params

			IGameStateChanger gameStateMachine,
			IGameplayInterfacePresenterFactory uiFactory,
			IPlayerFactory playerFactory,
			IUpgradeWindowViewFactory upgradeWindowViewFactory,
			IProgressSaveLoadDataService progressSaveLoadDataService,
			ILevelConfigGetter levelConfigGetter,
			ILevelProgressFacade levelProgressFacade,
			IResourcesProgressPresenter resourcesProgress,
			IPersistentProgressService persistentProgress,
			IAssetFactory assetFactory,
			IInjectableAssetFactory injectableAssetFactory,
			IUpgradeWindowPresenter upgradeWindowPresenter,
			IResourcesProgressPresenter resourcesProgressPresenter,
			IResourcesProgressPresenterFactory resourcesProgressPresenterFactory,
			IResourcesPrefabs resourcePathNameConfig,
			ICoroutineRunner coroutineRunner,
			ISceneLoader sceneLoader,
			ISandContainerView sandCarContainerView,
			IDissolveShaderViewController dissolveShaderViewController,
			IFillMeshShaderController fillMeshShaderController,
			ISandParticleView sandParticleView,
			IGameplayInterfacePresenter gameplayInterfacePresenter,
			IGameMenuPresenter gameMenuPresenter,
			IGameStateChanger gameStateChanger,
			TranslatorService translatorService,
			ProgressionConfig progressionConfig,
			IContainerBuilder containerBuilder

			#endregion

		)
		{
			#region Construction

			_gameStateMachine = gameStateMachine ?? throw new ArgumentNullException(nameof(gameStateMachine));
			_gameplayInterfacePresenterFactory = uiFactory ?? throw new ArgumentNullException(nameof(uiFactory));

			_playerFactory = playerFactory ?? throw new ArgumentNullException(nameof(playerFactory));

			_upgradeWindowViewFactory =
				upgradeWindowViewFactory ?? throw new ArgumentNullException(nameof(upgradeWindowViewFactory));

			_progressSaveLoadDataService =
				progressSaveLoadDataService ?? throw new ArgumentNullException(nameof(progressSaveLoadDataService));

			_levelConfigGetter = levelConfigGetter ?? throw new ArgumentNullException(nameof(levelConfigGetter));

			_levelProgressFacade = levelProgressFacade ?? throw new ArgumentNullException(nameof(levelProgressFacade));

			_persistentProgress = persistentProgress ?? throw new ArgumentNullException(nameof(persistentProgress));

			_assetFactory = assetFactory ?? throw new ArgumentNullException(nameof(assetFactory));

			_injectableAssetFactory = injectableAssetFactory ?? throw new ArgumentNullException(nameof(injectableAssetFactory));

			_sceneLoader = sceneLoader ?? throw new ArgumentNullException(nameof(sceneLoader));

			_upgradeWindowPresenter =
				upgradeWindowPresenter ?? throw new ArgumentNullException(nameof(upgradeWindowPresenter));

			_resourcesProgressPresenter = resourcesProgressPresenter ??
			                              throw new ArgumentNullException(nameof(resourcesProgressPresenter));

			_resourcesProgressPresenterFactory = resourcesProgressPresenterFactory ??
			                                     throw new ArgumentNullException(nameof(resourcesProgressPresenterFactory));

			_resourcePathNameConfig =
				resourcePathNameConfig ?? throw new ArgumentNullException(nameof(resourcePathNameConfig));

			_coroutineRunner = coroutineRunner ?? throw new ArgumentNullException(nameof(coroutineRunner));

			_sandCarContainerView =
				sandCarContainerView ?? throw new ArgumentNullException(nameof(sandCarContainerView));

			_dissolveShaderViewController = dissolveShaderViewController ??
			                                throw new ArgumentNullException(nameof(dissolveShaderViewController));

			_fillMeshShaderController = fillMeshShaderController ??
			                            throw new ArgumentNullException(nameof(fillMeshShaderController));

			_sandParticleView =
				sandParticleView ?? throw new ArgumentNullException(nameof(sandParticleView));

			_gameplayInterfacePresenter = gameplayInterfacePresenter ??
			                              throw new ArgumentNullException(nameof(gameplayInterfacePresenter));

			_gameMenuPresenter =
				gameMenuPresenter ?? throw new ArgumentNullException(nameof(gameMenuPresenter));

			_gameStateChanger =
				gameStateChanger ?? throw new ArgumentNullException(nameof(gameStateChanger));

			_translatorService = translatorService ?? throw new ArgumentNullException(nameof(translatorService));
			_progressionConfig = progressionConfig ?? throw new ArgumentNullException(nameof(progressionConfig));
			_containerBuilder = containerBuilder ?? throw new ArgumentNullException(nameof(containerBuilder));

			#endregion
		}

		private GameObject SpawnPoint => _resourcePathNameConfig.SceneGameObjects.SpawnPoint;
		private IResourcesPrefabs ResourcesPrefabs => _resourcePathNameConfig;
		private GameObject SellTrigger => ResourcesPrefabs.Triggers.SellTrigger.gameObject;

		public async void Enter(ILevelConfig payload)
		{
			await _sceneLoader.Load("Game");
			Build();

			_gameStateMachine.Enter<GameLoopState>();
		}

		private void Build()
		{
			_injectableAssetFactory.Instantiate(SellTrigger);

			IGameplayInterfacePresenter huy = _gameplayInterfacePresenterFactory.Create();

			GameObject playerGameObject = _playerFactory.Create(SpawnPoint);

			ResourcesProgressPresenter resourcesProgressPresenter = _resourcesProgressPresenterFactory.Create();

			new FillAreaShaderControllerFactory(playerGameObject).Create();

			CreateSandInCar(playerGameObject);

			RegisterUpgradeWindowPresenter();

			new RockFactory(
				_assetFactory,
				_levelProgressFacade,
				_resourcesProgressPresenter,
				_levelConfigGetter
			).Create();

			new CameraFactory(_assetFactory, playerGameObject, _resourcePathNameConfig).Create();
		}

		private void CreateSandInCar(GameObject playerGameObject)
		{
			_sandParticleView.Register<ISandParticleView>(
				playerGameObject.GetComponentInChildren<ISandParticleView>()
			);

			RegisterShaderViewPresenter(playerGameObject);
		}

		private void RegisterShaderViewPresenter(GameObject player)
		{
			IDissolveShaderView shaderView = player.GetComponent<IDissolveShaderView>();
			DissolveShaderViewController shaderViewController =
				new DissolveShaderViewController(shaderView, _coroutineRunner);

			shaderView.Construct(shaderViewController);
		}

		private void RegisterUpgradeWindowPresenter()
		{
			IUpgradeWindowPresenter presenter = new UpgradeWindowPresenterFactory(
				_upgradeWindowViewFactory,
				_assetFactory,
				_progressSaveLoadDataService,
				_persistentProgress,
				_gameplayInterfacePresenter,
				_resourcesProgressPresenter,
				_translatorService
			).Create();
		}

		public void Exit()
		{
		}
	}
}