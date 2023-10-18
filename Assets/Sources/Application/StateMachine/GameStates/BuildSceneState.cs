using Cysharp.Threading.Tasks;
using Sources.Application.StateMachineInterfaces;
using Sources.DIService;
using Sources.DomainInterfaces;
using Sources.Infrastructure.Factories;
using Sources.Infrastructure.Factories.Player;
using Sources.InfrastructureInterfaces.Scene;
using Sources.PresentationInterfaces;
using Sources.Services;
using Sources.Services.Interfaces;
using Sources.ServicesInterfaces;
using Sources.ServicesInterfaces.UI;
using Sources.Utils.Configs;
using Sources.View.SceneEntity;
using UnityEngine;

namespace Sources.Application.StateMachine.GameStates
{
	public class BuildSceneState : IPayloadState<LevelConfig>
	{
		private readonly GameStateMachine _gameStateMachine;
		private readonly LoadingCurtain   _loadingCurtain;
		private readonly GameServices     _gameServices;
		private readonly SceneLoader      _sceneLoader;

		private IPlayerFactory _playerFactory;
		private IUIFactory     _uiFactory;

		private ISceneLoad _sceneLoad;

		private IResourcesProgressPresenter _resourcesProgressPresenter;
		private IUpgradeWindowFactory       _upgradeWindowFactory;
		private ILocalizationService        _leanLocalization;
		private IProgressLoadDataService    _progressLoadService;
		private IPlayerStatsService         _playerStats;
		private ICameraFactory              _cameraFactory;
		private IAssetProvider              _assetProvider;

		private LevelConfig _levelConfig;

		public BuildSceneState
		(
			GameStateMachine gameStateMachine,
			SceneLoader      sceneLoader,
			LoadingCurtain   loadingCurtain,
			GameServices     gameServices
		)
		{
			_gameStateMachine = gameStateMachine;
			_sceneLoader      = sceneLoader;
			_loadingCurtain   = loadingCurtain;
			_gameServices     = gameServices;
		}

		public async UniTask Enter(LevelConfig levelConfig)
		{
			_levelConfig = levelConfig;

			_resourcesProgressPresenter = _gameServices.Get<IResourcesProgressPresenter>();
			_upgradeWindowFactory       = _gameServices.Get<IUpgradeWindowFactory>();

			_leanLocalization = _gameServices.Get<ILocalizationService>();
			_cameraFactory    = _gameServices.Get<ICameraFactory>();
			_playerFactory    = _gameServices.Get<IPlayerFactory>();
			_assetProvider    = _gameServices.Get<IAssetProvider>();
			_playerStats      = _gameServices.Get<IPlayerStatsService>();
			_uiFactory        = _gameServices.Get<IUIFactory>();
			_sceneLoad        = _gameServices.Get<ISceneLoad>();

			_loadingCurtain.Show();
			await _sceneLoader.Load(levelConfig.LevelName);
			await Create();

			OnSceneLoaded();
		}

		private async UniTask Create()
		{
			IMeshModifiable meshModifiable = new SandFactory(_assetProvider).Create();

			IMeshDeformationPresenter meshDeformationPresenter
				= new MeshDeformationPresenter(meshModifiable, _resourcesProgressPresenter);

			new MeshPresenter(meshDeformationPresenter, _resourcesProgressPresenter);

			await _uiFactory.Instantiate();

			GameObject initialPoint = GameObject.FindWithTag(ConstantNames.PlayerSpawnPointTag);

			_playerFactory.Instantiate(initialPoint, _uiFactory.GameplayInterface.Joystick, _playerStats);
			_cameraFactory.CreateVirtualCamera();

			InstantiateUpgradeWindow();

			_sceneLoad.InvokeSceneLoaded();

			_loadingCurtain.HideSlow();
			_leanLocalization.UpdateTranslations();
		}

		private void OnSceneLoaded() =>
			_gameStateMachine.Enter<GameLoopState>();

		public void Exit() =>
			_loadingCurtain.HideSlow();

		private void InstantiateUpgradeWindow() =>
			_upgradeWindowFactory.Create();
	}
}