using System;
using System.Collections.Generic;
using Sources.Boot.Scripts.Factories.Presentation;
using Sources.Boot.Scripts.Factories.Presentation.Scene;
using Sources.Boot.Scripts.Factories.Presentation.UI;
using Sources.Boot.Scripts.Factories.Presenters;
using Sources.BusinessLogic;
using Sources.BusinessLogic.Interfaces;
using Sources.BusinessLogic.Interfaces.Configs;
using Sources.BusinessLogic.Interfaces.Factory;
using Sources.BusinessLogic.Repository;
using Sources.BusinessLogic.Services;
using Sources.BusinessLogic.ServicesInterfaces;
using Sources.BusinessLogic.ServicesInterfaces.Advertisement;
using Sources.BusinessLogic.States;
using Sources.Controllers;
using Sources.ControllersInterfaces;
using Sources.ControllersInterfaces.Services;
using Sources.DomainInterfaces;
using Sources.DomainInterfaces.DomainServicesInterfaces;
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

		private readonly IPlayerFactory _playerFactory;
		private readonly IProgressSaveLoadDataService _progressSaveLoadDataService;
		private readonly IPersistentProgressService _persistentProgress;
		private readonly IAssetFactory _assetFactory;
		private readonly IInjectableAssetFactory _injectableAssetFactory;
		private readonly IResourcesPrefabs _resourcePathNameConfig;
		private readonly ICoroutineRunner _coroutineRunner;
		private readonly ISceneLoader _sceneLoader;
		private readonly TranslatorService _translatorService;
		private readonly IPresentersContainerRepository _presentersContainerRepository;
		private readonly IPlayerModelRepository _playerModelRepository;
		private readonly IAdvertisementPresenter _advertisementPresenter;
		private readonly ILevelChangerService _levelChangerService;
		private readonly IProgressEntityRepository _progressEntityRepository;
		private readonly ISaveLoader _saveLoader;
		private readonly IAdvertisement _advertisement;
		private readonly ILevelConfigGetter _levelConfigGetter;
		private readonly ILevelProgressFacade _levelProgressFacade;

		#endregion

		[Inject]
		public BuildSceneState(

			#region Params

			IGameStateChanger gameStateMachine,
			IPlayerFactory playerFactory,
			IProgressSaveLoadDataService progressSaveLoadDataService,
			ILevelConfigGetter levelConfigGetter,
			ILevelProgressFacade levelProgressFacade,
			IPersistentProgressService persistentProgress,
			IAssetFactory assetFactory,
			IInjectableAssetFactory injectableAssetFactory,
			IResourcesProgressPresenterFactory resourcesProgressPresenterFactory,
			IResourcesPrefabs resourcePathNameConfig,
			ICoroutineRunner coroutineRunner,
			ISceneLoader sceneLoader,
			ISandContainerView sandCarContainerView,
			IFillMeshShaderController fillMeshShaderController,
			ISandParticleView sandParticleView,
			TranslatorService translatorService,
			IPresentersContainerRepository presentersContainerRepository,
			IPlayerModelRepository playerModelRepository,
			IAdvertisementPresenter advertisementPresenter,
			ILevelChangerService levelChangerService,
			IProgressEntityRepository progressEntityRepository,
			ISaveLoader saveLoader

			#endregion

		)
		{
			#region Construction

			_gameStateMachine = gameStateMachine ?? throw new ArgumentNullException(nameof(gameStateMachine));

			_playerFactory = playerFactory ?? throw new ArgumentNullException(nameof(playerFactory));

			_progressSaveLoadDataService =
				progressSaveLoadDataService ?? throw new ArgumentNullException(nameof(progressSaveLoadDataService));

			_levelConfigGetter = levelConfigGetter ?? throw new ArgumentNullException(nameof(levelConfigGetter));

			_levelProgressFacade = levelProgressFacade ?? throw new ArgumentNullException(nameof(levelProgressFacade));

			_persistentProgress = persistentProgress ?? throw new ArgumentNullException(nameof(persistentProgress));

			_assetFactory = assetFactory ?? throw new ArgumentNullException(nameof(assetFactory));

			_injectableAssetFactory = injectableAssetFactory ?? throw new ArgumentNullException(nameof(injectableAssetFactory));

			_sceneLoader = sceneLoader ?? throw new ArgumentNullException(nameof(sceneLoader));

			_resourcePathNameConfig =
				resourcePathNameConfig ?? throw new ArgumentNullException(nameof(resourcePathNameConfig));

			_coroutineRunner = coroutineRunner ?? throw new ArgumentNullException(nameof(coroutineRunner));

			_translatorService = translatorService ?? throw new ArgumentNullException(nameof(translatorService));
			_presentersContainerRepository = presentersContainerRepository ??
			                                 throw new ArgumentNullException(nameof(presentersContainerRepository));
			_playerModelRepository = playerModelRepository ?? throw new ArgumentNullException(nameof(playerModelRepository));
			_advertisementPresenter = advertisementPresenter ?? throw new ArgumentNullException(nameof(advertisementPresenter));
			_levelChangerService = levelChangerService ?? throw new ArgumentNullException(nameof(levelChangerService));
			_progressEntityRepository =
				progressEntityRepository ?? throw new ArgumentNullException(nameof(progressEntityRepository));
			_saveLoader = saveLoader ?? throw new ArgumentNullException(nameof(saveLoader));

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
			// ILoadingCurtain loadingCurtain,
			// 	ILocalizationService localizationService,
			// IUpgradeWindowPresenter upgradeWindowPresenter,
			// 	IGameplayInterfacePresenter gameplayInterfacePresenter,
			// IResourcesProgressPresenter resourcesProgressPresenter,
			// 	IDissolveShaderViewController dissolveShaderViewController,
			// IGameMenuPresenter gameMenuPresenter,(???)
			// 	IAdvertisementPresenter advertisementHandler,
			// IPersistentProgressService persistentProgressService

			_injectableAssetFactory.Instantiate(SellTrigger);

			GameObject playerGameObject = _playerFactory.Create(SpawnPoint);

			var fillMeshShaderController = new FillAreaShaderControllerFactory(playerGameObject).Create();

			new CameraFactory(_assetFactory, playerGameObject, _resourcePathNameConfig).Create();

			var sendParticleView = playerGameObject.GetComponentInChildren<ISandParticleView>();

			var shaderViewController = new DissolveShaderViewController(
				playerGameObject.GetComponent<IDissolveShaderView>(),
				_coroutineRunner
			);

			playerGameObject.GetComponent<IDissolveShaderView>().Construct(shaderViewController);

			var gameplayInterfacePresenter = CreateGameplayInterfacePresenter();

			IResourcesProgressPresenter resourcesProgressPresenter = CreateResourcesProgressPresenter(
				gameplayInterfacePresenter,
				fillMeshShaderController,
				sendParticleView
			);

			var upgradeWindowPresenter = new UpgradeWindowPresenterFactory(
				_presentersContainerRepository,
				_assetFactory,
				_progressSaveLoadDataService,
				_persistentProgress,
				gameplayInterfacePresenter,
				resourcesProgressPresenter,
				_translatorService,
				_progressEntityRepository,
				_playerModelRepository,
				_saveLoader
			).Create();

			_presentersContainerRepository.AddRange(
				new List<IPresenter>
				{
					gameplayInterfacePresenter,
					shaderViewController,
					_advertisementPresenter,
					resourcesProgressPresenter,
					upgradeWindowPresenter
				}
			);
			InstantiateRocks(resourcesProgressPresenter);
		}

		private void InstantiateRocks(IResourcesProgressPresenter resourcesProgressPresenter)
		{
			new RockFactory(
				_assetFactory,
				_levelProgressFacade,
				resourcesProgressPresenter,
				_levelConfigGetter
			).Create();
		}

		private IResourcesProgressPresenter CreateResourcesProgressPresenter(
			IGameplayInterfacePresenter gameplayInterfacePresenter,
			FillMeshShaderController fillMeshShaderController,
			ISandParticleView sendParticleView) =>
			new ResourcesProgressPresenterFactory(
				gameplayInterfacePresenter,
				_persistentProgress,
				fillMeshShaderController,
				sendParticleView,
				_coroutineRunner,
				_playerModelRepository
			).Create();

		private IGameplayInterfacePresenter CreateGameplayInterfacePresenter() =>
			new GameplayInterfacePresenterFactory(
				_translatorService,
				_assetFactory,
				_persistentProgress,
				_levelChangerService,
				_gameStateMachine,
				_coroutineRunner,
				_advertisement,
				_playerModelRepository
			).Create();

		public void Exit()
		{
		}
	}
}
