using System;
using Cysharp.Threading.Tasks;
using Sources.Application.StateMachineInterfaces;
using Sources.DIService;
using Sources.DomainInterfaces;
using Sources.Infrastructure.Factories.Player;
using Sources.InfrastructureInterfaces.Factory;
using Sources.InfrastructureInterfaces.Scene;
using Sources.Services.Interfaces;
using Sources.ServicesInterfaces;
using Sources.ServicesInterfaces.UI;
using Sources.Utils.Configs;
using Sources.View.SceneEntity;
using UnityEngine;

namespace Sources.Application.StateMachine.GameStates
{
	public class SceneLoadState : IPayloadState<string>
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
		private ILocalizationService _leanLocalization;

		private bool _isSceneLoaded;

		public SceneLoadState(GameStateMachine gameStateMachine, SceneLoader sceneLoader, LoadingCurtain loadingCurtain,
			GameServices gameServices)
		{
			_gameStateMachine = gameStateMachine;
			_sceneLoader = sceneLoader;
			_loadingCurtain = loadingCurtain;
			_gameServices = gameServices;
		}

		public async UniTask Enter(string levelName)
		{
			_leanLocalization = _gameServices.Get<ILocalizationService>();
			_upgradeWindowFactory = _gameServices.Get<IUpgradeWindowFactory>();
			_cameraFactory = _gameServices.Get<ICameraFactory>();
			_playerFactory = _gameServices.Get<IPlayerFactory>();
			_uiFactory = _gameServices.Get<IUIFactory>();
			_sceneLoad = _gameServices.Get<ISceneLoad>();
			_presenterFactory = _gameServices.Get<IPresenterFactory>();
			_playerStats = _gameServices.Get<IPlayerStatsService>();

			_loadingCurtain.Show();
			await _sceneLoader.Load(levelName);
			await Create();
			await OnSceneLoaded();
		}

		private async UniTask Create()
		{
			_loadingCurtain.SetText("Create UI");
			await _uiFactory.CreateUI();

			_initialPoint = GameObject.FindWithTag(ConstantNames.PlayerSpawnPointTag);
			_loadingCurtain.SetText("Instantiate Player");
			_playerFactory.Instantiate(_initialPoint, _presenterFactory, _uiFactory.Joystick, _playerStats);
			_cameraFactory.CreateVirtualCamera();

			InstantiateUpgradeWindow();

			_sceneLoad.InvokeSceneLoaded();

			_loadingCurtain.HideLazy();
			_leanLocalization.UpdateTranslations();

			_isSceneLoaded = true;
		}

		private async UniTask OnSceneLoaded()
		{
			await _gameStateMachine.Enter<GameLoopState>();
			_loadingCurtain.SetText("");
		}

		public void Exit() =>
			_loadingCurtain.HideLazy();

		private void InstantiateUpgradeWindow() =>
			_upgradeWindowFactory.Create();
	}
}