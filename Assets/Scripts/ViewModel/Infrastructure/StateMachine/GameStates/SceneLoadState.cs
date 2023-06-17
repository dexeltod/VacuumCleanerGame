using Cysharp.Threading.Tasks;
using Model;
using Model.Configs;
using Model.Data;
using Model.DI;
using UnityEngine;
using View.UI;
using ViewModel.Infrastructure.Services;
using ViewModel.Infrastructure.Services.Factories;
using ViewModel.Infrastructure.Services.Factories.Player;

namespace ViewModel.Infrastructure.StateMachine.GameStates
{
	public class SceneLoadState : IPayloadState<string>
	{
		private readonly GameStateMachine _gameStateMachine;
		private readonly SceneLoader _sceneLoader;
		private readonly LoadingCurtain _loadingCurtain;
		private readonly ServiceLocator _serviceLocator;

		private IPlayerFactory _playerFactory;
		private IUIFactory _uiFactory;

		private ISceneLoad _sceneLoad;

		private GameObject _initialPoint;
		private IPresenterFactory _presenterFactory;
		private ICameraFactory _cameraFactory;
		private IUpgradeWindowFactory _upgradeWindowFactory;
		private GameProgressModel _gameProgress;
		private ISaveLoadDataService _saveLoadService;

		public SceneLoadState(GameStateMachine gameStateMachine, SceneLoader sceneLoader, LoadingCurtain loadingCurtain,
			ServiceLocator serviceLocator)
		{
			_gameStateMachine = gameStateMachine;
			_sceneLoader = sceneLoader;
			_loadingCurtain = loadingCurtain;
			_serviceLocator = serviceLocator;
		}

		public async void Enter(string levelName)
		{
			_gameProgress = _serviceLocator.GetSingle<IPersistentProgressService>().GameProgress;
			_upgradeWindowFactory = _serviceLocator.GetSingle<IUpgradeWindowFactory>();
			_cameraFactory = _serviceLocator.GetSingle<ICameraFactory>();
			_playerFactory = _serviceLocator.GetSingle<IPlayerFactory>();
			_uiFactory = _serviceLocator.GetSingle<IUIFactory>();
			_sceneLoad = _serviceLocator.GetSingle<ISceneLoad>();
			_presenterFactory = _serviceLocator.GetSingle<IPresenterFactory>();

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
			await _playerFactory.Instantiate(_initialPoint, _presenterFactory, _uiFactory.Joystick, _gameProgress);
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