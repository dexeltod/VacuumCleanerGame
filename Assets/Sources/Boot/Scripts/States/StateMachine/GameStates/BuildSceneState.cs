using System;
using System.Collections.Generic;
using Sources.Boot.Scripts.Factories.Player;
using Sources.Boot.Scripts.Factories.Presentation;
using Sources.Boot.Scripts.Factories.Presentation.Scene;
using Sources.Boot.Scripts.Factories.Presentation.UI;
using Sources.Boot.Scripts.Factories.Presenters;
using Sources.BusinessLogic;
using Sources.BusinessLogic.Interfaces;
using Sources.BusinessLogic.Interfaces.Configs;
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
using Sources.Infrastructure.Services;
using Sources.Presentation.UI;
using Sources.PresentationInterfaces;
using Sources.PresentationInterfaces.Player;
using Sources.PresentationInterfaces.Triggers;
using Sources.Utils;
using UnityEngine;
using VContainer;

namespace Sources.Boot.Scripts.States.StateMachine.GameStates
{
	public sealed class BuildSceneState : IBuildSceneState
	{
		private readonly IStateMachine _stateMachine;

		private readonly IProgressSaveLoadDataService _progressSaveLoadDataService;
		private readonly IPersistentProgressService _persistentProgress;
		private readonly IAssetLoader _assetLoader;
		private readonly IInjectableAssetLoader _injectableAssetLoader;
		private readonly ICoroutineRunner _coroutineRunner;
		private readonly ISceneLoader _sceneLoader;
		private readonly TranslatorService _translatorService;
		private readonly IPresentersContainerRepository _presentersContainerRepository;
		private readonly IPlayerModelRepository _playerModelRepository;
		private readonly IProgressEntityRepository _progressEntityRepository;
		private readonly ISaveLoader _saveLoader;
		private readonly ILeaderBoardService _leaderBoardService;
		private readonly IAdvertisement _advertisement;
		private readonly ILevelConfigGetter _levelConfigGetter;
		private readonly ILevelProgressFacade _levelProgressFacade;

		private GameObject _playerGameObject;

		[Inject]
		public BuildSceneState(

			#region Params

			IStateMachine stateMachine,
			IProgressSaveLoadDataService progressSaveLoadDataService,
			ILevelConfigGetter levelConfigGetter,
			ILevelProgressFacade levelProgressFacade,
			IPersistentProgressService persistentProgress,
			IAssetLoader assetLoader,
			IInjectableAssetLoader injectableAssetLoader,
			IResourcesPrefabs resourcePathNameConfig,
			ICoroutineRunner coroutineRunner,
			ISceneLoader sceneLoader,
			TranslatorService translatorService,
			IPresentersContainerRepository presentersContainerRepository,
			IPlayerModelRepository playerModelRepository,
			IProgressEntityRepository progressEntityRepository,
			ISaveLoader saveLoader,
			ILeaderBoardService leaderBoardService

			#endregion

		)
		{
			#region Construction

			_stateMachine = stateMachine ?? throw new ArgumentNullException(nameof(stateMachine));

			_progressSaveLoadDataService =
				progressSaveLoadDataService ?? throw new ArgumentNullException(nameof(progressSaveLoadDataService));

			_levelConfigGetter = levelConfigGetter ?? throw new ArgumentNullException(nameof(levelConfigGetter));

			_levelProgressFacade = levelProgressFacade ?? throw new ArgumentNullException(nameof(levelProgressFacade));

			_persistentProgress = persistentProgress ?? throw new ArgumentNullException(nameof(persistentProgress));

			_assetLoader = assetLoader ?? throw new ArgumentNullException(nameof(assetLoader));

			_injectableAssetLoader = injectableAssetLoader ?? throw new ArgumentNullException(nameof(injectableAssetLoader));

			_sceneLoader = sceneLoader ?? throw new ArgumentNullException(nameof(sceneLoader));

			ResourcesPrefabs =
				resourcePathNameConfig ?? throw new ArgumentNullException(nameof(resourcePathNameConfig));

			_coroutineRunner = coroutineRunner ?? throw new ArgumentNullException(nameof(coroutineRunner));

			_translatorService = translatorService ?? throw new ArgumentNullException(nameof(translatorService));
			_presentersContainerRepository = presentersContainerRepository ??
			                                 throw new ArgumentNullException(nameof(presentersContainerRepository));
			_playerModelRepository = playerModelRepository ?? throw new ArgumentNullException(nameof(playerModelRepository));
			_progressEntityRepository =
				progressEntityRepository ?? throw new ArgumentNullException(nameof(progressEntityRepository));
			_saveLoader = saveLoader ?? throw new ArgumentNullException(nameof(saveLoader));
			_leaderBoardService = leaderBoardService ?? throw new ArgumentNullException(nameof(leaderBoardService));

			#endregion
		}

		private GameObject SpawnPoint => ResourcesPrefabs.SceneGameObjects.SpawnPoint;
		private IResourcesPrefabs ResourcesPrefabs { get; }

		private GameObject SellTrigger => ResourcesPrefabs.Triggers.SellTrigger.gameObject;
		private string UIResourcesUI => ResourcesAssetPath.Scene.UIResources.UI;

		public async void Enter(ILevelConfig payload)
		{
			await _sceneLoader.Load("Game");
			Build();

			_stateMachine.Enter<GameLoopState>();
		}

		public void Exit()
		{
		}

		private void Build()
		{
			var gameplayInterfaceView = LoadGameplayInterfaceView();

			IGameplayInterfacePresenter gameplayInterfacePresenter = CreateGameplayInterfacePresenter(levelChangerService);

			_playerGameObject = CreatePlayer(gameplayInterfacePresenter);

			var sendParticleView = _playerGameObject.GetComponentInChildren<ISandParticleView>();

			FillMeshShader fillMeshShaderController = CreateFillMeshShaderController();
			IResourcesProgressPresenter resourcesProgressPresenter = CreateResourcesProgressPresenter(
				gameplayInterfaceView,
				fillMeshShaderController,
				sendParticleView
			);

			LevelChangerService levelChangerService =
				new(
					_levelProgressFacade,
					_stateMachine,
					_levelConfigGetter,
					resourcesProgressPresenter,
					_progressSaveLoadDataService,
					_advertisement,
					_leaderBoardService,
					_persistentProgress
				);

			var shaderViewController = new DissolveShaderViewController(
				_playerGameObject.GetComponent<IDissolveShaderView>(),
				_coroutineRunner
			);

			_playerGameObject.GetComponent<IDissolveShaderView>().Construct(shaderViewController);

			_presentersContainerRepository.AddRange(
				new List<IPresenter>
				{
					gameplayInterfacePresenter,
					shaderViewController,
					resourcesProgressPresenter,
					CreateUpgradeWindowPresenter(gameplayInterfacePresenter, resourcesProgressPresenter),
					new AdvertisementPresenter(_advertisement)
				}
			);
			CreateCamera();
			InstantiateRocks(resourcesProgressPresenter);
		}

		private GameplayInterfaceView LoadGameplayInterfaceView() =>
			_assetLoader.Instantiate(UIResourcesUI).GetComponent<GameplayInterfaceView>();

		private void CreateCamera() =>
			new CameraFactory(_assetLoader, _playerGameObject, ResourcesPrefabs).Create();

		private FillMeshShader CreateFillMeshShaderController() =>
			new FillAreaShaderControllerFactory(_playerGameObject).Create();

		private IGameplayInterfacePresenter CreateGameplayInterfacePresenter(LevelChangerService levelChangerService)
		{
			return new GameplayInterfacePresenterFactory(
				_translatorService,
				_assetLoader,
				_persistentProgress,
				levelChangerService,
				_stateMachine,
				_coroutineRunner,
				_advertisement,
				_playerModelRepository
			).Create();
		}

		private GameObject CreatePlayer(IGameplayInterfacePresenter gameplayInterfacePresenter) =>
			new PlayerFactory(
				_injectableAssetLoader,
				_playerModelRepository,
				gameplayInterfacePresenter
			).Create(SpawnPoint);

		private IResourcesProgressPresenter CreateResourcesProgressPresenter(
			IGameplayInterfacePresenter gameplayInterfacePresenter,
			FillMeshShader fillMeshShader,
			ISandParticleView sendParticleView) =>
			new ResourcesProgressPresenterFactory(
				gameplayInterfacePresenter,
				_persistentProgress,
				fillMeshShader,
				sendParticleView,
				_coroutineRunner,
				_playerModelRepository,
				_assetLoader.Instantiate(SellTrigger).GetComponent<ITriggerSell>()
			).Create();

		private IUpgradeWindowPresenter CreateUpgradeWindowPresenter(IGameplayInterfacePresenter gameplayInterfacePresenter,
			IResourcesProgressPresenter resourcesProgressPresenter) =>
			new UpgradeWindowPresenterFactory(
				_presentersContainerRepository,
				_assetLoader,
				_progressSaveLoadDataService,
				_persistentProgress,
				gameplayInterfacePresenter,
				resourcesProgressPresenter,
				_translatorService,
				_progressEntityRepository,
				_playerModelRepository,
				_saveLoader
			).Create();

		private void InstantiateRocks(IResourcesProgressPresenter resourcesProgressPresenter) =>
			new RockFactory(
				_assetLoader,
				_levelProgressFacade,
				resourcesProgressPresenter,
				_levelConfigGetter
			).Create();
	}
}
