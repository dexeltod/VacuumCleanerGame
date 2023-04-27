using Cysharp.Threading.Tasks;
using Model.DI;
using Model.Infrastructure.Services;
using Model.Infrastructure.Services.Factories;
using UnityEngine;

namespace Model.Infrastructure.StateMachine.GameStates
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
			_upgradeWindowFactory = ServiceLocator.Container.GetSingle<IUpgradeWindowFactory>();

			_cameraFactory = ServiceLocator.Container.GetSingle<ICameraFactory>();
			_playerFactory = ServiceLocator.Container.GetSingle<IPlayerFactory>();
			_uiFactory = ServiceLocator.Container.GetSingle<IUIFactory>();
			_sceneLoad = ServiceLocator.Container.GetSingle<ISceneLoad>();
			_presenterFactory = ServiceLocator.Container.GetSingle<IPresenterFactory>();

			var provider = ServiceLocator.Container.GetSingle<IAssetProvider>();
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
			await _playerFactory.Instantiate(_initialPoint, _presenterFactory, _uiFactory.Joystick);
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

		private async UniTask InstantiateUpgradeWindowAsync()
		{
			GameObject upgradeWindow = await _upgradeWindowFactory.Create();
			upgradeWindow.SetActive(false);
		}
	}
}