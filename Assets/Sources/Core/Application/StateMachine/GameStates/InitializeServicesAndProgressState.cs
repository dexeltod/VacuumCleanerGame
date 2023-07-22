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

		public InitializeServicesAndProgressState(GameStateMachine gameStateMachine, ServiceLocator serviceLocator,
			SceneLoader sceneLoader)
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

		private void InitServices()
		{
			_serviceLocator.Register<IGameStateMachine>(_gameStateMachine);

			_serviceLocator.Register<IPersistentProgressService>(new PersistentProgressService());
			_serviceLocator.Register<IAssetProvider>(new AssetProvider());

			SceneLoadInformer sceneLoadInformer = new SceneLoadInformer();

			_serviceLocator.Register<IPresenterFactory>(new PresenterFactory());
			_serviceLocator.Register<ISceneLoadInformer>(sceneLoadInformer);
			_serviceLocator.Register<ISceneLoad>(sceneLoadInformer);

			CameraFactory cameraFactory = new CameraFactory();
			_serviceLocator.Register<ICameraFactory>(cameraFactory);
			_serviceLocator.Register<ICamera>(cameraFactory);
			_serviceLocator.Register<ISceneConfigGetter>(new SceneConfigGetter());

			_serviceLocator.Register<ISaveLoadDataService>(new SaveLoadDataService());
		}
	}
}