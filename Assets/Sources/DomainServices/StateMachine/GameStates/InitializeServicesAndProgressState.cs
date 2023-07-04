using Application;
using Application.Configs;
using Application.DI;
using Cysharp.Threading.Tasks;
using DomainServices;
using Infrastructure.Factories;
using Infrastructure.Services;
using InfrastructureInterfaces;

namespace Infrastructure.StateMachine.GameStates
{
	public class InitializeServicesAndProgressState : IGameState
	{
		private readonly GameStateMachine _gameStateMachine;
		private readonly ServiceLocator _serviceLocator;
		private readonly SceneLoader _sceneLoader;
		private readonly MusicSetter _musicSetter;

		public InitializeServicesAndProgressState(GameStateMachine gameStateMachine, ServiceLocator serviceLocator,
			MusicSetter musicSetter, SceneLoader sceneLoader)
		{
			_sceneLoader = sceneLoader;
			_musicSetter = musicSetter;
			_gameStateMachine = gameStateMachine;
			_serviceLocator = serviceLocator;
			InitServices();
		}

		public void Exit()
		{
		}

		public void Enter()
		{
			_sceneLoader.Load(ConstantNames.InitialScene, OnSceneLoaded);
		}

		private void OnSceneLoaded() =>
			_gameStateMachine.Enter<InitializeServicesWithProgressState>();

		private async void InitServices()
		{
			_serviceLocator.RegisterAsSingle<IGameStateMachine>(_gameStateMachine);
			IPersistentProgressService persistentProgress =
				_serviceLocator.RegisterAsSingle<IPersistentProgressService>(new PersistentProgressService());
			_serviceLocator.RegisterAsSingle<IAssetProvider>(new AssetProvider());

			_serviceLocator.RegisterAsSingle<IMusicService>(new MusicService(_musicSetter,
				_serviceLocator.GetSingle<IAssetProvider>()));

			SceneLoadInformer sceneLoadInformer = new SceneLoadInformer();

			_serviceLocator.RegisterAsSingle<IPresenterFactory>(new PresenterFactory());
			_serviceLocator.RegisterAsSingle<ISceneLoadInformer>(sceneLoadInformer);
			_serviceLocator.RegisterAsSingle<ISceneLoad>(sceneLoadInformer);

			CameraFactory cameraFactory = new CameraFactory();
			_serviceLocator.RegisterAsSingle<ICameraFactory>(cameraFactory);
			_serviceLocator.RegisterAsSingle<ICamera>(cameraFactory);
			_serviceLocator.RegisterAsSingle<ISceneConfigGetter>(new SceneConfigGetter());

			ISaveLoadDataService saveLoadService =
				_serviceLocator.RegisterAsSingle<ISaveLoadDataService>(new SaveLoadDataService());

			//================================================================
			//Loading after initialization saveLoadService

			await InitProgress(saveLoadService, persistentProgress);
		}

		private async UniTask InitProgress(ISaveLoadDataService saveLoadService, IPersistentProgressService persistentProgressService)
		{
			var progressFactory = new ProgressFactory(saveLoadService, persistentProgressService);
			await progressFactory.UpdateProgress();
		}
	}
}