
using Model.DI;
using Model.Infrastructure.Camera;
using Model.Infrastructure.Data;
using Model.Infrastructure.Services;
using Model.Infrastructure.Services.Factories;
using ViewModel.Infrastructure;

namespace Model.Infrastructure.StateMachine.GameStates
{
	public class InitializeServicesState : IGameState
	{
		private readonly GameStateMachine _gameStateMachine;
		private readonly ServiceLocator _serviceLocator;
		private readonly SceneLoader _sceneLoader;
		private readonly MusicSetter _musicSetter;

		public InitializeServicesState(GameStateMachine gameStateMachine, ServiceLocator serviceLocator, MusicSetter musicSetter,  SceneLoader sceneLoader)
		{
			_sceneLoader = sceneLoader;
			_musicSetter = musicSetter;
			_gameStateMachine = gameStateMachine;
			_serviceLocator = serviceLocator;
			RegisterServices();
		}

		public void Exit()
		{
		}

		public void Enter()
		{
			_sceneLoader.Load(ConstantNamesConfig.InitialScene, OnSceneLoaded);
		}

		private void OnSceneLoaded() =>
			_gameStateMachine.Enter<LoadProgressState>();
		
		private void RegisterServices()
		{
			_serviceLocator.RegisterAsSingle<IGameStateMachine>(_gameStateMachine);
			_serviceLocator.RegisterAsSingle<IPersistentProgressService>(new PersistentProgressService());
			_serviceLocator.RegisterAsSingle<IAssetProvider>(new AssetProvider());
			_serviceLocator.RegisterAsSingle<ISaveLoadDataService>(new SaveLoadDataService(new GameProgressFactory()));
			_serviceLocator.RegisterAsSingle<IMusicService>(new MusicService(_musicSetter, _serviceLocator.GetSingle<IAssetProvider>()));
			
			SceneLoadInformer sceneLoadInformer = new SceneLoadInformer();
	        
			_serviceLocator.RegisterAsSingle<IPresenterFactory>(new PresenterFactory());
			_serviceLocator.RegisterAsSingle<ISceneLoadInformer>(sceneLoadInformer);
			_serviceLocator.RegisterAsSingle<ISceneLoad>(sceneLoadInformer);
			_serviceLocator.RegisterAsSingle<IUIFactory>(new UIFactory());
			
			CameraFactory cameraFactory = new CameraFactory();
			_serviceLocator.RegisterAsSingle<ICameraFactory>(cameraFactory);
			_serviceLocator.RegisterAsSingle<ICamera>(cameraFactory);
			_serviceLocator.RegisterAsSingle<ISceneConfigGetter>(new SceneConfigGetter());
		}
	}
}