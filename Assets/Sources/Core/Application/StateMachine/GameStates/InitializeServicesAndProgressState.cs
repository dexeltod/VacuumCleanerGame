using Cysharp.Threading.Tasks;
using Sources.Core.Application.StateMachineInterfaces;
using Sources.Core.DI;
using Sources.Core.Utils.Configs;
using Sources.DomainServices;
using Sources.DomainServices.Interfaces;
using Sources.Infrastructure.Factories;
using Sources.Infrastructure.InfrastructureInterfaces;
using Sources.Infrastructure.InfrastructureInterfaces.Scene;
using Sources.Infrastructure.Services;
using Sources.Infrastructure.Services.Interfaces;

namespace Sources.Core.Application.StateMachine.GameStates
{
	public class InitializeServicesAndProgressState : IGameState
	{
		private readonly GameStateMachine _gameStateMachine;
		private readonly ServiceLocator _serviceLocator;
		private readonly SceneLoader _sceneLoader;
		private readonly MusicSetter _musicSetter;

		public InitializeServicesAndProgressState(GameStateMachine gameStateMachine, ServiceLocator serviceLocator, SceneLoader sceneLoader)
		{
			_sceneLoader = sceneLoader;
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

			// _serviceLocator.RegisterAsSingle<IMusicService>(new MusicService(_musicSetter,
			// 	_serviceLocator.GetSingle<IAssetProvider>()));

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

			//===================================================================================================//
			//Loading after initialization saveLoadService

			
		}

		
	}
}