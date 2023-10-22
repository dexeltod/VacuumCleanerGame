using Cysharp.Threading.Tasks;
using Sources.Application.StateMachineInterfaces;
using Sources.DIService;
using Sources.DomainInterfaces;
using Sources.Infrastructure.Factories;
using Sources.Infrastructure.Factories.Player;
using Sources.InfrastructureInterfaces.Scene;
using Sources.PresentationInterfaces;
using Sources.Services;
using Sources.ServicesInterfaces;
using Sources.ServicesInterfaces.UI;
using Sources.Utils.Configs;
using Sources.View.SceneEntity;

namespace Sources.Application.StateMachine.GameStates
{
	public class BuildSandState : IPayloadState<LevelConfig>
	{
		private readonly GameStateMachine _gameStateMachine;
		private readonly LoadingCurtain   _loadingCurtain;
		private readonly ServiceLocator   _serviceLocator;
		private readonly SceneLoader      _sceneLoader;

		private IPlayerFactory _playerFactory;

		private IResourcesProgressPresenter _resourcesProgress;
		private IUpgradeWindowFactory       _upgradeWindowFactory;
		private IProgressLoadDataService    _progressLoadService;
		private ICameraFactory              _cameraFactory;
		private IAssetProvider              _assetProvider;

		private LevelConfig _levelConfig;

		public BuildSandState
		(
			GameStateMachine gameStateMachine,
			SceneLoader      sceneLoader,
			ServiceLocator   serviceLocator,
			LoadingCurtain   loadingCurtain
		)
		{
			_gameStateMachine = gameStateMachine;
			_sceneLoader      = sceneLoader;
			_serviceLocator   = serviceLocator;
			_loadingCurtain   = loadingCurtain;
		}

		public async UniTask Enter(LevelConfig levelConfig)
		{
			_resourcesProgress = _serviceLocator.Get<IResourcesProgressPresenter>();
			_assetProvider     = _serviceLocator.Get<IAssetProvider>();

			_loadingCurtain.Show();
			_levelConfig = levelConfig;

			await _sceneLoader.Load(levelConfig.LevelName);
			Create();

			OnSceneLoaded();
		}

		private void Create()
		{
			IMeshModifiable           meshModifiable = new SandFactory(_assetProvider).Create();
			IMeshDeformationPresenter presenter      = new MeshDeformationPresenter(meshModifiable, _resourcesProgress);

			new MeshPresenter(presenter, _resourcesProgress);
		}

		private void OnSceneLoaded() =>
			_gameStateMachine.Enter<InitializeServicesWithViewState>();

		public void Exit() { }
	}
}