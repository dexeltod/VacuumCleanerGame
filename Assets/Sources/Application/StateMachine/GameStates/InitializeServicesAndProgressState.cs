using Cysharp.Threading.Tasks;
using Sources.Application.StateMachineInterfaces;
using Sources.Application.UnityApplicationServices;
using Sources.Application.Utils.Configs;
using Sources.DIService;
using Sources.Infrastructure.Factories;
using Sources.InfrastructureInterfaces.Factory;
using Sources.InfrastructureInterfaces.Scene;
using Sources.Services;
using Sources.Services.DomainServices;
using Sources.Services.Localization;
using Sources.ServicesInterfaces;
using Unity.Services.Core;

namespace Sources.Application.StateMachine.GameStates
{
	public class InitializeServicesAndProgressState : IGameState
	{
		private readonly GameStateMachine _gameStateMachine;
		private readonly GameServices _gameServices;
		private readonly SceneLoader _sceneLoader;
		private readonly MusicSetter _musicSetter;
		private readonly UnityServicesController _unityServicesController;

		public InitializeServicesAndProgressState(GameStateMachine gameStateMachine, GameServices gameServices,
			SceneLoader sceneLoader)
		{
			_sceneLoader = sceneLoader;
			_gameStateMachine = gameStateMachine;
			_gameServices = gameServices;
			_unityServicesController = new UnityServicesController(new InitializationOptions());
		}

		public void Exit()
		{
		}

		public async UniTask Enter()
		{
			await InitServices();
			_sceneLoader.Load(ConstantNames.InitialScene, OnSceneLoaded);
		}

		private async void OnSceneLoaded() =>
			await _gameStateMachine.Enter<InitializeServicesWithProgressState>();

		private async UniTask InitServices()
		{
			await _unityServicesController.InitializeUnityServices();

			_gameServices.Register<IGameStateMachine>(_gameStateMachine);

			_gameServices.Register<IPersistentProgressService>(new PersistentProgressService());
			IAssetProvider provider = _gameServices.Register<IAssetProvider>(new AssetProvider());
			_gameServices.Register<IUpgradeStatsProvider>(new UpgradeStatsProvider(provider));
			_gameServices.Register<ILocalizationService>(new LocalizationService());

			SceneLoadInformer sceneLoadInformer = new SceneLoadInformer();

			_gameServices.Register<IPresenterFactory>(new PresenterFactory());
			_gameServices.Register<ISceneLoadInformer>(sceneLoadInformer);
			_gameServices.Register<ISceneLoad>(sceneLoadInformer);

			CameraFactory cameraFactory = new CameraFactory();
			_gameServices.Register<ICameraFactory>(cameraFactory);
			_gameServices.Register<ICamera>(cameraFactory);
			_gameServices.Register<ISceneConfigGetter>(new SceneConfigGetter());

			_gameServices.Register<ISaveLoadDataService>(new SaveLoadDataService());
		}
	}
}