using System;
using Cysharp.Threading.Tasks;
using Sources.Application.StateMachineInterfaces;
using Sources.Application.UI;
using Sources.Application.Utils.Configs;
using Sources.DIService;
using Sources.Infrastructure.Factories.Player;
using Sources.InfrastructureInterfaces;
using Sources.InfrastructureInterfaces.Factory;
using Sources.InfrastructureInterfaces.Scene;
using Sources.Services.Interfaces;
using Sources.ServicesInterfaces;
using Sources.ServicesInterfaces.UI;
using Sources.View.SceneEntity;
using UnityEngine;

namespace Sources.Application.StateMachine.GameStates
{
	public class SceneLoadState : IPayloadState<string>, IDisposable
	{
		private readonly GameStateMachine _gameStateMachine;
		private readonly SceneLoader _sceneLoader;
		private readonly LoadingCurtain _loadingCurtain;
		private readonly GameServices _gameServices;

		private GameObject _initialPoint;
		
		private IPlayerFactory _playerFactory;
		private IUIFactory _uiFactory;

		private ISceneLoad _sceneLoad;

		private IPresenterFactory _presenterFactory;
		private ICameraFactory _cameraFactory;
		private IUpgradeWindowFactory _upgradeWindowFactory;
		private IPlayerStatsService _playerStats;
		private ISaveLoadDataService _saveLoadService;

		public SceneLoadState(GameStateMachine gameStateMachine, SceneLoader sceneLoader, LoadingCurtain loadingCurtain,
			GameServices gameServices)
		{
			_gameStateMachine = gameStateMachine;
			_sceneLoader = sceneLoader;
			_loadingCurtain = loadingCurtain;
			_gameServices = gameServices;
		}

		public void Dispose()
		{
			_sceneLoad.SceneLoaded -= OnSceneLoaded;
		}
		
		public void Enter(string levelName)
		{
			_upgradeWindowFactory = _gameServices.Get<IUpgradeWindowFactory>();
			_cameraFactory = _gameServices.Get<ICameraFactory>();
			_playerFactory = _gameServices.Get<IPlayerFactory>();
			_uiFactory = _gameServices.Get<IUIFactory>();
			_sceneLoad = _gameServices.Get<ISceneLoad>();
			_presenterFactory = _gameServices.Get<IPresenterFactory>();
			_playerStats = _gameServices.Get<IPlayerStatsService>();
			
			_loadingCurtain.Show();
			_sceneLoader.Load(levelName, OnLoaded);
		}
		private void OnLoaded()
		{
			_uiFactory.CreateUI();

			_initialPoint = GameObject.FindWithTag(ConstantNames.PlayerSpawnPointTag);
			_playerFactory.Instantiate(_initialPoint, _presenterFactory, _uiFactory.Joystick, _playerStats);
			_cameraFactory.CreateVirtualCamera();

			InstantiateUpgradeWindow();

			_sceneLoad.SceneLoaded += OnSceneLoaded;
			_sceneLoad.InvokeSceneLoaded();
			_loadingCurtain.Hide();
		}

		private void OnSceneLoaded()
		{
			_gameStateMachine.Enter<GameLoopState>();
			_sceneLoad.SceneLoaded -= OnSceneLoaded;
		}
 
		public void Exit() =>
			_loadingCurtain.Hide();

		private void InstantiateUpgradeWindow() =>
			_upgradeWindowFactory.Create();

		
	}
}