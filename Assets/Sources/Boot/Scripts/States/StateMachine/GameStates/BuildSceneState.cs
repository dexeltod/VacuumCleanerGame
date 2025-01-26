using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Plugins.Joystick_Pack.Scripts.Base;
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
using Sources.Controllers.Services;
using Sources.ControllersInterfaces;
using Sources.ControllersInterfaces.Services;
using Sources.DomainInterfaces;
using Sources.DomainInterfaces.DomainServicesInterfaces;
using Sources.Infrastructure.Services;
using Sources.Presentation.UI;
using Sources.PresentationInterfaces;
using Sources.PresentationInterfaces.Common;
using Sources.PresentationInterfaces.Player;
using Sources.PresentationInterfaces.Triggers;
using Sources.Utils;
using UnityEngine;
using VContainer;

namespace Sources.Boot.Scripts.States.StateMachine.GameStates
{
	public sealed class BuildSceneState : IBuildSceneState
	{
		private readonly IAdvertisement _advertisement;
		private readonly IAssetLoader _assetLoader;
		private readonly ICoroutineRunner _coroutineRunner;
		private readonly IInjectableAssetLoader _injectableAssetLoader;
		private readonly ILeaderBoardService _leaderBoardService;
		private readonly ILevelConfigGetter _levelConfigGetter;
		private readonly ILevelProgressFacade _levelProgressFacade;
		private readonly IPersistentProgressService _persistentProgress;
		private readonly IPlayerModelRepository _playerModelRepository;
		private readonly IRepository<IPresenter> _presentersRepository;
		private readonly IProgressEntityRepository _progressEntityRepository;

		private readonly IProgressSaveLoadDataService _progressSaveLoadDataService;
		private readonly ISaveLoader _saveLoader;
		private readonly ISceneLoader _sceneLoader;
		private readonly IStateMachine _stateMachine;
		private readonly TranslatorService _translatorService;
		private readonly IRepository<IView> _viewsRepository;

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
			PresentersRepository presentersRepository,
			ViewsRepository viewsRepository,
			IPlayerModelRepository playerModelRepository,
			IProgressEntityRepository progressEntityRepository,
			ISaveLoader saveLoader,
			ILeaderBoardService leaderBoardService,
			IAdvertisement advertisement

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
			_presentersRepository = presentersRepository ?? throw new ArgumentNullException(nameof(presentersRepository));
			_viewsRepository = viewsRepository ?? throw new ArgumentNullException(nameof(viewsRepository));
			_playerModelRepository = playerModelRepository ?? throw new ArgumentNullException(nameof(playerModelRepository));
			_progressEntityRepository =
				progressEntityRepository ?? throw new ArgumentNullException(nameof(progressEntityRepository));
			_saveLoader = saveLoader ?? throw new ArgumentNullException(nameof(saveLoader));
			_leaderBoardService = leaderBoardService ?? throw new ArgumentNullException(nameof(leaderBoardService));
			_advertisement = advertisement ?? throw new ArgumentNullException(nameof(advertisement));

			#endregion
		}

		private GameObject SpawnPoint => ResourcesPrefabs.SceneGameObjects.SpawnPoint;
		private IResourcesPrefabs ResourcesPrefabs { get; }

		private GameObject SellTrigger => ResourcesPrefabs.Triggers.SellTrigger.gameObject;
		private string UIResourcesUI => ResourcesAssetPath.Scene.UIResources.UI;

		public async UniTask Enter(ILevelConfig payload)
		{
			Debug.Log("loading game scene");
			await _sceneLoader.LoadAsync("Game");
			Debug.Log("loaded");

			Build();
			Debug.Log("builded");

			_stateMachine.Enter<GameLoopState>();
		}

		public void Exit()
		{
		}

		private void Build()
		{
			SceneResourcesRepository sceneResourcesRepository = new();

			ICollection<IResourcePresentation> rocks = InstantiateRocks(sceneResourcesRepository);

			GameplayInterfaceView gameplayInterfaceView = LoadGameplayInterfaceView();
			_viewsRepository.Add(gameplayInterfaceView);

			IMonoPresenter player = CreatePlayer(gameplayInterfaceView.Joystick);
			GameObject playerGameObject = player.GameObject;

			var sendParticleView = playerGameObject.GetComponentInChildren<ISandParticleView>();

			FillMeshShader fillMeshShaderController = CreateFillMeshShaderController(playerGameObject);

			var trigger = _assetLoader.Instantiate(SellTrigger).GetComponent<ITriggerSell>();

			IResourcesProgressPresenter resourcesProgressPresenter = CreateResourcesProgressPresenter(
				rocks,
				fillMeshShaderController,
				sendParticleView,
				trigger,
				sceneResourcesRepository
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

			IGameplayInterfacePresenter gameplayInterfacePresenter =
				CreateGameplayInterfacePresenter(gameplayInterfaceView, levelChangerService);

			var shaderViewController = new DissolveShaderViewController(
				playerGameObject.GetComponent<IDissolveShaderView>(),
				_coroutineRunner
			);

			playerGameObject.GetComponent<IDissolveShaderView>().Construct(shaderViewController);

			_presentersRepository.AddRange(
				new List<IPresenter>
				{
					gameplayInterfacePresenter,
					shaderViewController,
					CreateUpgradeWindowPresenter(gameplayInterfaceView),
					new AdvertisementPresenter(_advertisement),
					player,
					resourcesProgressPresenter
				}
			);

			CreateCamera(playerGameObject);
		}

		private void CreateCamera(GameObject playerGameObject) =>
			new CameraFactory(_assetLoader, playerGameObject, ResourcesPrefabs).Create();

		private FillMeshShader CreateFillMeshShaderController(GameObject playerGameObject) =>
			new FillAreaShaderControllerFactory(playerGameObject).Create();

		private IGameplayInterfacePresenter CreateGameplayInterfacePresenter(
			GameplayInterfaceView gameplayInterfaceView,
			LevelChangerService levelChangerService) =>
			new GameplayInterfacePresenterFactory(
				gameplayInterfaceView,
				_translatorService,
				_persistentProgress,
				levelChangerService,
				_stateMachine,
				_coroutineRunner,
				_advertisement,
				_playerModelRepository
			).Create();

		private IMonoPresenter CreatePlayer(Joystick joystick) =>
			new PlayerFactory(
				_injectableAssetLoader,
				_playerModelRepository,
				joystick
			).Create(SpawnPoint);

		private IResourcesProgressPresenter CreateResourcesProgressPresenter(
			ICollection<IResourcePresentation> rocks,
			FillMeshShader fillMeshShader,
			ISandParticleView sendParticleView,
			ITriggerSell triggerSell,
			SceneResourcesRepository sceneResourcesRepository) =>
			new ResourcesProgressPresenterFactory(
				_persistentProgress,
				fillMeshShader,
				sendParticleView,
				_coroutineRunner,
				_playerModelRepository,
				triggerSell,
				rocks,
				sceneResourcesRepository
			).Create();

		private IUpgradeWindowPresenter CreateUpgradeWindowPresenter(IView gameplayInterface) =>
			new UpgradeWindowPresenterFactory(
				_assetLoader,
				_progressSaveLoadDataService,
				_persistentProgress,
				_translatorService,
				_progressEntityRepository,
				_playerModelRepository,
				_saveLoader,
				gameplayInterface
			).Create();

		private ICollection<IResourcePresentation> InstantiateRocks(SceneResourcesRepository newSceneResourcesRepository) =>
			new RockFactory(
				_assetLoader,
				_levelProgressFacade,
				_levelConfigGetter,
				newSceneResourcesRepository
			).Create();

		private GameplayInterfaceView LoadGameplayInterfaceView() =>
			_assetLoader.Instantiate(UIResourcesUI).GetComponent<GameplayInterfaceView>();
	}
}
