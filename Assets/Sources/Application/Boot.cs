using System;
using Sources.Application.StateMachine;
using Sources.Presentation.SceneEntity;
using Sources.Services;
using Sources.UseCases.Scene;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Sources.Application
{
	public class Boot : LifetimeScope, ICoroutineRunner
	{
		private Game _game;

		public void StopCoroutineRunning(Coroutine coroutine)
		{
			if (coroutine == null)
				throw new ArgumentNullException(nameof(coroutine));

			StopCoroutine(coroutine);
		}

		protected override void Configure(IContainerBuilder builder)
		{
			base.Configure(builder);
			builder.RegisterEntryPoint<Game>();
			builder.Register<AssetProvider>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();

			builder.RegisterFactory<LoadingCurtainFactory, LoadingCurtain>(
				container =>
				{
					var loadingCurtain = container.Load();
					loadingCurtain.gameObject.SetActive(true);
					return loadingCurtain;
				}
			);

			builder.Register<SceneLoader>(Lifetime.Singleton);
			как ты сука меня заебала шлюха блядь, пошла ты на хуй
			builder.Register<GameStateMachineFactory>(Lifetime.Singleton);

			// builder.Register<LevelProgressFacade>(Lifetime.Scoped).AsImplementedInterfaces().AsSelf();
			// builder.Register<LevelConfigGetter>(Lifetime.Scoped).AsImplementedInterfaces().AsSelf();
			// builder.Register<ResourcesProgressPresenter>(Lifetime.Scoped).AsImplementedInterfaces().AsSelf();
			// builder.Register<ProgressLoadDataService>(Lifetime.Scoped).AsImplementedInterfaces().AsSelf();
			// builder.Register<YandexRewardsService>(Lifetime.Scoped).AsImplementedInterfaces().AsSelf();
			// builder.Register<LevelChangerPresenter>(Lifetime.Scoped);
			выблядь ебучая
		}
	}
}