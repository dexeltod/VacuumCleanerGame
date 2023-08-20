using System.ComponentModel.Design;
using Sources.Application.StateMachineInterfaces;
using Sources.Application.Utils.Configs;
using Sources.DIService;
using Sources.Infrastructure.Factories;
using Sources.InfrastructureInterfaces;
using Sources.InfrastructureInterfaces.Factory;
using Sources.InfrastructureInterfaces.Scene;
using Sources.Services;
using Sources.Services.DomainServices;
using Sources.Services.Interfaces;
using Sources.ServicesInterfaces;

namespace Sources.Application.StateMachine.GameStates
{
	public class InitializeServicesAndProgressState : IGameState
	{
		private readonly GameStateMachine _gameStateMachine;
		private readonly GameServices _gameServices;
		private readonly SceneLoader _sceneLoader;
		private readonly MusicSetter _musicSetter;

		public InitializeServicesAndProgressState(GameStateMachine gameStateMachine, GameServices gameServices,
			SceneLoader sceneLoader)
		{
			_sceneLoader = sceneLoader;
			_gameStateMachine = gameStateMachine;
			_gameServices = gameServices;
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
			_gameServices.Register<IGameStateMachine>(_gameStateMachine);

			_gameServices.Register<IPersistentProgressService>(new PersistentProgressService());
			_gameServices.Register<IResourceProvider>(new ResourceProvider());

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