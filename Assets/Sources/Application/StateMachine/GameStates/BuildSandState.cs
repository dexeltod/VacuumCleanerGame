using System;
using Cysharp.Threading.Tasks;
using Sources.ApplicationServicesInterfaces;
using Sources.ApplicationServicesInterfaces.StateMachineInterfaces;
using Sources.Infrastructure.Factories.Scene;
using Sources.Presentation.SceneEntity;
using Sources.PresentationInterfaces;
using Sources.Services;
using Sources.ServicesInterfaces;
using Sources.Utils.Configs.Scripts;

namespace Sources.Application.StateMachine.GameStates
{
	public sealed class BuildSandState : IPayloadState<LevelConfig>
	{
		private readonly GameStateMachine _gameStateMachine;
		private readonly LoadingCurtain _loadingCurtain;
		private readonly ISceneLoader _sceneLoader;
		private readonly IAssetResolver _assetResolver;
		private readonly IResourcesProgressPresenter _resourcesProgress;

		public BuildSandState(
			GameStateMachine gameStateMachine,
			ISceneLoader sceneLoader,
			LoadingCurtain loadingCurtain,
			IResourcesProgressPresenter resourcesProgress,
			IAssetResolver assetResolver
		)
		{
			_gameStateMachine = gameStateMachine ?? throw new ArgumentNullException(nameof(gameStateMachine));
			_sceneLoader = sceneLoader ?? throw new ArgumentNullException(nameof(sceneLoader));
			_resourcesProgress = resourcesProgress ?? throw new ArgumentNullException(nameof(resourcesProgress));
			_assetResolver = assetResolver ?? throw new ArgumentNullException(nameof(assetResolver));
			_loadingCurtain = loadingCurtain ? loadingCurtain : throw new ArgumentNullException(nameof(loadingCurtain));
		}

		public async UniTask Enter(LevelConfig levelConfig)
		{
			if (levelConfig == null) throw new ArgumentNullException(nameof(levelConfig));

			_loadingCurtain.Show();

			await _sceneLoader.Load(levelConfig.LevelName);
			Create();

			OnSceneLoaded();
		}

		private void Create()
		{
			IMeshModifiable meshModifiable = new SandFactory(_assetResolver).Create();
			IMeshDeformationPresenter presenter = new MeshDeformationPresenter(meshModifiable, _resourcesProgress);

			new MeshPresenter(presenter, _resourcesProgress);
		}

		private void OnSceneLoaded() =>
			_gameStateMachine.Enter<BuildSceneState>();

		public void Exit() { }
	}
}