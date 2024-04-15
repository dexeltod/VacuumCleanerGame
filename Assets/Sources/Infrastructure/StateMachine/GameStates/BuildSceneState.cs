using System;
using Sources.Controllers;
using Sources.ControllersInterfaces;
using Sources.DomainInterfaces;
using Sources.Infrastructure.Configs.Scripts;
using Sources.Infrastructure.Configs.Scripts.Level;
using Sources.Infrastructure.Factories;
using Sources.Infrastructure.Factories.Player;
using Sources.Infrastructure.Factories.Presenters;
using Sources.Infrastructure.Factories.Scene;
using Sources.Infrastructure.Factories.UI;
using Sources.Infrastructure.Providers;
using Sources.InfrastructureInterfaces;
using Sources.InfrastructureInterfaces.Configs;
using Sources.InfrastructureInterfaces.Factory;
using Sources.InfrastructureInterfaces.Providers;
using Sources.InfrastructureInterfaces.States;
using Sources.PresentationInterfaces;
using Sources.PresentationInterfaces.Player;
using Sources.Services.Localization;
using Sources.ServicesInterfaces;
using Sources.ServicesInterfaces.Advertisement;
using UnityEngine;
using VContainer;

namespace Sources.Infrastructure.StateMachine.GameStates
{
	public sealed class BuildSceneState : IBuildSceneState
	{
#region Fields

		private readonly IGameStateChangerProvider _gameStateMachine;

		private readonly GameplayInterfacePresenterFactory _gameplayInterfacePresenterFactory;
		private readonly IPlayerFactory _playerFactory;
		private readonly IUpgradeWindowViewFactory _upgradeWindowViewFactory;
		private readonly IProgressSaveLoadDataService _progressSaveLoadDataService;
		private readonly IPersistentProgressServiceProvider _persistentProgress;
		private readonly IAssetFactory _assetFactory;
		private readonly CoroutineRunnerFactory _coroutineRunnerFactory;
		private readonly UpgradeWindowPresenterProvider _upgradeWindowPresenterProvider;
		private readonly ResourcesProgressPresenterProvider _resourcesProgressPresenterProvider;
		private readonly ResourcesProgressPresenterFactory _resourcesProgressPresenterFactory;
		private readonly ResourcePathNameConfigProvider _resourcePathNameConfigProvider;
		private readonly ICoroutineRunnerProvider _coroutineRunnerProvider;
		private readonly ISceneLoader _sceneLoader;
		private readonly SandCarContainerViewProvider _sandCarContainerViewProvider;
		private readonly DissolveShaderViewControllerProvider _dissolveShaderViewControllerProvider;
		private readonly FillMeshShaderControllerProvider _fillMeshShaderControllerProvider;
		private readonly ISandParticleSystemProvider _sandParticleSystemProvider;
		private readonly GameplayInterfacePresenterProvider _gameplayInterfacePresenterProvider;
		private readonly GameMenuPresenterProvider _gameMenuPresenterProvider;
		private readonly GameStateChangerProvider _gameStateChangerProvider;
		private readonly ITranslatorService _translatorService;
		private readonly ProgressionConfig _progressionConfig;
		private readonly IAdvertisement _advertisement;
		private readonly ILevelConfigGetter _levelConfigGetter;
		private readonly ILevelProgressFacade _levelProgressFacade;

#endregion

		[Inject]
		public BuildSceneState(

#region Params

			IGameStateChangerProvider gameStateMachine,
			GameplayInterfacePresenterFactory uiFactory,
			IPlayerFactory playerFactory,
			IUpgradeWindowViewFactory upgradeWindowViewFactory,
			IProgressSaveLoadDataService progressSaveLoadDataService,
			ILevelConfigGetter levelConfigGetter,
			ILevelProgressFacade levelProgressFacade,
			IResourcesProgressPresenterProvider resourcesProgress,
			IPersistentProgressServiceProvider persistentProgress,
			IAssetFactory assetFactory,
			CoroutineRunnerFactory coroutineRunnerFactory,
			UpgradeWindowPresenterProvider upgradeWindowPresenterProvider,
			ResourcesProgressPresenterProvider resourcesProgressPresenterProvider,
			ResourcesProgressPresenterFactory resourcesProgressPresenterFactory,
			ResourcePathNameConfigProvider resourcePathNameConfigProvider,
			ICoroutineRunnerProvider coroutineRunnerProvider,
			ISceneLoader sceneLoader,
			SandCarContainerViewProvider sandCarContainerViewProvider,
			DissolveShaderViewControllerProvider dissolveShaderViewControllerProvider,
			FillMeshShaderControllerProvider fillMeshShaderControllerProvider,
			ISandParticleSystemProvider sandParticleSystemProvider,
			IPlayerStatsServiceProvider playerStatsServiceProvider,
			PlayerStatsFactory playerStatsFactory,
			GameplayInterfacePresenterProvider gameplayInterfacePresenterProvider,
			GameMenuPresenterProvider gameMenuPresenterProvider,
			GameStateChangerProvider gameStateChangerProvider,
			ITranslatorService translatorService,
			ProgressionConfig progressionConfig

#endregion

		)
		{
#region Construction

			_gameStateMachine = gameStateMachine ??
				throw new ArgumentNullException(nameof(gameStateMachine));
			_gameplayInterfacePresenterFactory = uiFactory ??
				throw new ArgumentNullException(nameof(uiFactory));
			_playerFactory = playerFactory ??
				throw new ArgumentNullException(nameof(playerFactory));
			_upgradeWindowViewFactory = upgradeWindowViewFactory ??
				throw new ArgumentNullException(nameof(upgradeWindowViewFactory));
			_progressSaveLoadDataService = progressSaveLoadDataService ??
				throw new ArgumentNullException(nameof(progressSaveLoadDataService));
			_levelConfigGetter = levelConfigGetter ??
				throw new ArgumentNullException(nameof(levelConfigGetter));
			_levelProgressFacade = levelProgressFacade ??
				throw new ArgumentNullException(nameof(levelProgressFacade));
			_persistentProgress = persistentProgress ??
				throw new ArgumentNullException(nameof(persistentProgress));
			_assetFactory = assetFactory ??
				throw new ArgumentNullException(nameof(assetFactory));
			_coroutineRunnerFactory = coroutineRunnerFactory ??
				throw new ArgumentNullException(nameof(coroutineRunnerFactory));
			_sceneLoader = sceneLoader ??
				throw new ArgumentNullException(nameof(sceneLoader));
			_upgradeWindowPresenterProvider = upgradeWindowPresenterProvider ??
				throw new ArgumentNullException(nameof(upgradeWindowPresenterProvider));
			_resourcesProgressPresenterProvider = resourcesProgressPresenterProvider ??
				throw new ArgumentNullException(nameof(resourcesProgressPresenterProvider));
			_resourcesProgressPresenterFactory = resourcesProgressPresenterFactory ??
				throw new ArgumentNullException(nameof(resourcesProgressPresenterFactory));
			_resourcePathNameConfigProvider = resourcePathNameConfigProvider ??
				throw new ArgumentNullException(nameof(resourcePathNameConfigProvider));
			_coroutineRunnerProvider = coroutineRunnerProvider ??
				throw new ArgumentNullException(nameof(coroutineRunnerProvider));
			_sandCarContainerViewProvider = sandCarContainerViewProvider ??
				throw new ArgumentNullException(nameof(sandCarContainerViewProvider));
			_dissolveShaderViewControllerProvider = dissolveShaderViewControllerProvider ??
				throw new ArgumentNullException(nameof(dissolveShaderViewControllerProvider));
			_fillMeshShaderControllerProvider = fillMeshShaderControllerProvider ??
				throw new ArgumentNullException(nameof(fillMeshShaderControllerProvider));
			_sandParticleSystemProvider = sandParticleSystemProvider ??
				throw new ArgumentNullException(nameof(sandParticleSystemProvider));
			_gameplayInterfacePresenterProvider = gameplayInterfacePresenterProvider ??
				throw new ArgumentNullException(nameof(gameplayInterfacePresenterProvider));
			_gameMenuPresenterProvider = gameMenuPresenterProvider ??
				throw new ArgumentNullException(nameof(gameMenuPresenterProvider));
			_gameStateChangerProvider = gameStateChangerProvider ??
				throw new ArgumentNullException(nameof(gameStateChangerProvider));
			_translatorService = translatorService ?? throw new ArgumentNullException(nameof(translatorService));
			_progressionConfig = progressionConfig ?? throw new ArgumentNullException(nameof(progressionConfig));

#endregion
		}

		private GameObject SpawnPoint => _resourcePathNameConfigProvider.Implementation.SceneGameObjects.SpawnPoint;
		private ResourcesPrefabs ResourcesPrefabs => _resourcePathNameConfigProvider.Implementation;
		private GameObject SellTrigger => ResourcesPrefabs.Triggers.SellTrigger;
		private IResourcesModel ResourcesModel => _persistentProgress.Implementation.GlobalProgress.ResourcesModel;

		public async void Enter(ILevelConfig payload)
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

			GameObject playerGameObject = _playerFactory.Create(SpawnPoint);

			ResourcesProgressPresenter resourcesProgressPresenter = _resourcesProgressPresenterFactory.Create();
			_resourcesProgressPresenterProvider.Register<IResourcesProgressPresenter>(resourcesProgressPresenter);

			_fillMeshShaderControllerProvider.Register<IFillMeshShaderController>(
				new FillAreaShaderControllerFactory(playerGameObject).Create()
			);

			CreateSandInCar(playerGameObject);

			RegisterUpgradeWindowPresenterProvider();

			new RockFactory(
				_assetFactory,
				_levelProgressFacade,
				_resourcesProgressPresenterProvider,
				_levelConfigGetter
			).Create();

			// IMeshModifiable meshModifiable = new SandFactory(
			// 	_assetFactory,
			// 	_levelProgressFacade,
			// 	_resourcesProgressPresenterProvider
			// ).Create();

			new CameraFactory(_assetFactory, _playerFactory.Player, _resourcePathNameConfigProvider).Create();
		}

		private void CreateSandInCar(GameObject playerGameObject)
		{
			_sandParticleSystemProvider.Register<ISandParticleSystem>(
				playerGameObject.GetComponentInChildren<ISandParticleSystem>()
			);

			RegisterShaderViewPresenterProvider(playerGameObject);
		}

		private void RegisterShaderViewPresenterProvider(GameObject player)
		{
			var shaderView = player.GetComponent<IDissolveShaderView>();
			var shaderViewController = new DissolveShaderViewController(shaderView, _coroutineRunnerProvider);

			shaderView.Construct(shaderViewController);

			_dissolveShaderViewControllerProvider.Register<IDissolveShaderViewController>(shaderViewController);
		}

		private void RegisterUpgradeWindowPresenterProvider()
		{
			IUpgradeWindowPresenter presenter = new UpgradeWindowPresenterFactory(
				_upgradeWindowViewFactory,
				_assetFactory,
				_progressSaveLoadDataService,
				_persistentProgress.Implementation,
				_gameplayInterfacePresenterProvider.Implementation,
				_resourcesProgressPresenterProvider,
				_translatorService
			).Create();

			_upgradeWindowPresenterProvider.Register(presenter);
		}

		public void Exit() { }
	}
}