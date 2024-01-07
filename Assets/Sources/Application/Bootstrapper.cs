using System;
using Sources.Application.StateMachine;
using Sources.Application.StateMachine.GameStates;
using Sources.DIService;
using Sources.Infrastructure;
using Sources.Infrastructure.Presenters;
using Sources.InfrastructureInterfaces.Scene;
using Sources.Presentation.SceneEntity;
using Sources.Services;
using Sources.Services.DomainServices;
using Sources.ServicesInterfaces;
using Sources.UseCases.Scene;
using Sources.Utils.Configs.Scripts;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Sources.Application
{
	public class Bootstrapper : LifetimeScope, ICoroutineRunner, IStartable
	{
		private Game _game;

		public void Start()
		{
			DontDestroyOnLoad(this);

			IAssetProvider assetProvider = new AssetProvider();

			LoadingCurtain loadingCurtain = LoadLoadingCurtain(assetProvider);
			loadingCurtain.gameObject.SetActive(true);

			SceneLoader sceneLoader = new SceneLoader();

			ServiceLocator.Initialize();

			GameStateMachine gameStateMachine =
				new GameStateMachineFactory(
					sceneLoader,
					assetProvider,
					loadingCurtain,
					this
				).Create();

			_game = new Game(gameStateMachine);

			_game.Start();
		}

		public void StopCoroutineRunning(Coroutine coroutine)
		{
			if (coroutine == null)
				throw new ArgumentNullException(nameof(coroutine));

			StopCoroutine(coroutine);
		}

		protected override void Configure(IContainerBuilder builder)
		{
			base.Configure(builder);
			builder.RegisterEntryPoint<BuildSceneState>();
			builder.Register<GameStateMachine>(Lifetime.Scoped).AsImplementedInterfaces().AsSelf();
			builder.Register<LevelProgressFacade>(Lifetime.Scoped).AsImplementedInterfaces().AsSelf();
			builder.Register<LevelConfigGetter>(Lifetime.Scoped).AsImplementedInterfaces().AsSelf();
			builder.Register<ResourcesProgressPresenter>(Lifetime.Scoped).AsImplementedInterfaces().AsSelf();
			builder.Register<ProgressLoadDataService>(Lifetime.Scoped).AsImplementedInterfaces().AsSelf();
			builder.Register<YandexRewardsService>(Lifetime.Scoped).AsImplementedInterfaces().AsSelf();
			builder.Register<LevelChangerPresenter>(Lifetime.Scoped);
		}

		private LoadingCurtain LoadLoadingCurtain(IAssetProvider provider) =>
			provider.InstantiateAndGetComponent<LoadingCurtain>(ResourcesAssetPath.GameObjects.LoadinCrutain);
	}
}