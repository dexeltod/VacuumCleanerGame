using System;
using Cysharp.Threading.Tasks;
using Sources.ApplicationServicesInterfaces.StateMachineInterfaces;
using Sources.DomainInterfaces;
using Sources.Infrastructure.Factories;
using Sources.Infrastructure.Factories.Player;
using Sources.InfrastructureInterfaces.Scene;
using Sources.Presentation.SceneEntity;
using Sources.PresentationInterfaces;
using Sources.Services;
using Sources.ServicesInterfaces;
using Sources.ServicesInterfaces.UI;
using Sources.Utils.Configs.Scripts;
using VContainer;

namespace Sources.Application.StateMachine.GameStates
{
	public sealed class BuildSandState : IPayloadState<LevelConfig>
	{
		private readonly GameStateMachine _gameStateMachine;
		private readonly LoadingCurtain _loadingCurtain;
		private readonly IObjectResolver _container;
		private readonly ISceneLoader _sceneLoader;

		private IPlayerFactory _playerFactory;

		private IResourcesProgressPresenter _resourcesProgress;
		private IUpgradeWindowFactory _upgradeWindowFactory;
		private IProgressLoadDataService _progressLoadService;
		private ICameraFactory _cameraFactory;
		private IAssetProvider _assetProvider;

		private LevelConfig _levelConfig;

		public BuildSandState(
			GameStateMachine gameStateMachine,
			ISceneLoader sceneLoader,
			LoadingCurtain loadingCurtain
		)
		{
			_gameStateMachine = gameStateMachine ?? throw new ArgumentNullException(nameof(gameStateMachine));
			_sceneLoader = sceneLoader ?? throw new ArgumentNullException(nameof(sceneLoader));
			_loadingCurtain = loadingCurtain ? loadingCurtain : throw new ArgumentNullException(nameof(loadingCurtain));
		}

		public async UniTask Enter(LevelConfig levelConfig)
		{
			if (levelConfig == null) throw new ArgumentNullException(nameof(levelConfig));
			_resourcesProgress = _container.Resolve<IResourcesProgressPresenter>();
			_assetProvider = _container.Resolve<IAssetProvider>();

			_loadingCurtain.Show();
			_levelConfig = levelConfig;

			await _sceneLoader.Load(levelConfig.LevelName);
			Create();

			OnSceneLoaded();
		}

		private void Create()
		{
			IMeshModifiable meshModifiable = new SandFactory(_assetProvider).Create();
			IMeshDeformationPresenter presenter = new MeshDeformationPresenter(meshModifiable, _resourcesProgress);

			new MeshPresenter(presenter, _resourcesProgress);
		}

		private void OnSceneLoaded() =>
			_gameStateMachine.Enter<InitializeServicesWithViewState>();

		public void Exit() { }
	}
}