using System.Collections;
using Cysharp.Threading.Tasks;
using Sources.Application.StateMachineInterfaces;
using Sources.Application.UnityApplicationServices;
using Sources.ApplicationServicesInterfaces;
using Sources.DIService;
using Sources.DomainInterfaces;
using Sources.Infrastructure.Factories;
using Sources.InfrastructureInterfaces.Factory;
using Sources.InfrastructureInterfaces.Scene;
using Sources.Services;
using Sources.Services.DomainServices;
using Sources.Services.Localization;
using Sources.ServicesInterfaces;
using Sources.Utils.Configs;
using Sources.View.SceneEntity;
using Unity.Services.Core;
using UnityEngine;
#if YANDEX_GAMES && !UNITY_EDITOR
using Sources.Application.YandexSDK;
#endif

namespace Sources.Application.StateMachine.GameStates
{
	public class InitializeServicesAndProgressState : IGameState
	{
		private readonly GameStateMachine _gameStateMachine;
		private readonly GameServices _gameServices;
		private readonly SceneLoader _sceneLoader;
		private readonly ICoroutineRunner _coroutineRunner;
		private readonly LoadingCurtain _loadingCurtain;
		private readonly MusicSetter _musicSetter;
		private readonly UnityServicesController _unityServicesController;
		private bool _isServicesRegistered;

		public InitializeServicesAndProgressState(GameStateMachine gameStateMachine, GameServices gameServices,
			SceneLoader sceneLoader, ICoroutineRunner coroutineRunner, LoadingCurtain loadingCurtain)
		{
			_sceneLoader = sceneLoader;
			_coroutineRunner = coroutineRunner;
			_loadingCurtain = loadingCurtain;
			_gameStateMachine = gameStateMachine;
			_gameServices = gameServices;
			_unityServicesController = new UnityServicesController(new InitializationOptions());
		}

		public void Exit()
		{
		}

		public async UniTask Enter()
		{
			await RegisterServices();
			await _sceneLoader.Load(ConstantNames.InitialScene);

			UniTask.WaitWhile(() => _isServicesRegistered == false);
			await _gameStateMachine.Enter<InitializeServicesWithProgressState>();
		}

		private async UniTask RegisterServices()
		{
			IAssetProvider provider = _gameServices.Register<IAssetProvider>(new AssetProvider());
			_gameServices.Register<ILocalizationService>(new LocalizationService());
#if YANDEX_GAMES && !UNITY_EDITOR
			_loadingCurtain.SetText("Start YANDEX SDK initialization");
			YandexGamesSdkController yandexGamesSdkController = new YandexGamesSdkController(_coroutineRunner, _loadingCurtain);
			await yandexGamesSdkController.Initialize();
			_gameServices.Register<IYandexSDKController>(yandexGamesSdkController);
#endif

#if !YANDEX_GAMES && TEST_BUILD && !UNITY_EDITOR
			_loadingCurtain.SetText("Start UNITY SERVICES initialization");
			await _unityServicesController.InitializeUnityServices();
#endif

			_loadingCurtain.SetText("Initialization services");
			_gameServices.Register<IGameStateMachine>(_gameStateMachine);
			_gameServices.Register<IPersistentProgressService>(new PersistentProgressService());

			_gameServices.Register<IUpgradeStatsProvider>(new UpgradeStatsProvider(provider));

			SceneLoadInformer sceneLoadInformer = new SceneLoadInformer();

			_gameServices.Register<IPresenterFactory>(new PresenterFactory());
			_gameServices.Register<ISceneLoadInformer>(sceneLoadInformer);
			_gameServices.Register<ISceneLoad>(sceneLoadInformer);

			CameraFactory cameraFactory = new CameraFactory();
			_gameServices.Register<ICameraFactory>(cameraFactory);
			_gameServices.Register<ICamera>(cameraFactory);
			_gameServices.Register<ISceneConfigGetter>(new SceneConfigGetter());

			_loadingCurtain.SetText("Register save load ");
			_gameServices.Register<ISaveLoadDataService>(new SaveLoadDataService(_coroutineRunner));
			_loadingCurtain.SetText("");

			_isServicesRegistered = true;
		}
	}
}