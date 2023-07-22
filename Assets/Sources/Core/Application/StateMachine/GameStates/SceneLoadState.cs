using Cysharp.Threading.Tasks;
using Sources.Core.Application.StateMachineInterfaces;
using Sources.Core.DI;
using Sources.Core.Utils.Configs;
using Sources.DomainServices;
using Sources.Infrastructure.Factories.Player;
using Sources.Infrastructure.InfrastructureInterfaces;
using Sources.Infrastructure.InfrastructureInterfaces.Scene;
using Sources.Infrastructure.Services;
using Sources.Infrastructure.Services.Interfaces;
using Sources.View.Services.UI;
using UnityEngine;

namespace Sources.Core.Application.StateMachine.GameStates
{
	public class SceneLoadState : IPayloadState<string>
	{
		private readonly GameStateMachine _gameStateMachine;
		private readonly SceneLoader _sceneLoader;
		private readonly LoadingCurtain _loadingCurtain;
		private readonly ServiceLocator _serviceLocator;

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
			ServiceLocator serviceLocator)
		{
			_gameStateMachine = gameStateMachine;
			_sceneLoader = sceneLoader;
			_loadingCurtain = loadingCurtain;
			_serviceLocator = serviceLocator;
		}

		public void Enter(string levelName)
		{
			_upgradeWindowFactory = _serviceLocator.GetSingle<IUpgradeWindowFactory>();
			_cameraFactory = _serviceLocator.GetSingle<ICameraFactory>();
			_playerFactory = _serviceLocator.GetSingle<IPlayerFactory>();
			_uiFactory = _serviceLocator.GetSingle<IUIFactory>();
			_sceneLoad = _serviceLocator.GetSingle<ISceneLoad>();
			_presenterFactory = _serviceLocator.GetSingle<IPresenterFactory>();
			_playerStats = _serviceLocator.GetSingle<IPlayerStatsService>();

			var provider = _serviceLocator.GetSingle<IAssetProvider>();
			provider.CleanUp();
			_loadingCurtain.Show();
			_sceneLoader.Load(levelName, StartLoading);
		}

		private async void StartLoading()
		{
			await Load();
		}

		private async UniTask Load()
		{
			await OnLoaded();
		}

		private async UniTask OnLoaded()
		{
			await _uiFactory.CreateUI();

			_initialPoint = GameObject.FindWithTag(ConstantNames.PlayerSpawnPointTag);
			await _playerFactory.Instantiate(_initialPoint, _presenterFactory, _uiFactory.Joystick, _playerStats);
			await _cameraFactory.CreateVirtualCamera();

			await InstantiateUpgradeWindowAsync();

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

		private async UniTask InstantiateUpgradeWindowAsync() =>
			await _upgradeWindowFactory.Create();
	}
}