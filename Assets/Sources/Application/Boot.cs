using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Sources.Application.Leaderboard;
using Sources.Application.StateMachine.GameStates;
using Sources.Application.UnityApplicationServices;
using Sources.Application.YandexSDK;
using Sources.ApplicationServicesInterfaces;
using Sources.DomainInterfaces;
using Sources.DomainInterfaces.DomainServicesInterfaces;
using Sources.Infrastructure;
using Sources.Infrastructure.DataViewModel;
using Sources.Infrastructure.Factories;
using Sources.Infrastructure.Factories.Player;
using Sources.Infrastructure.Factories.UpgradeShop;
using Sources.Infrastructure.Presenters;
using Sources.Infrastructure.Shop;
using Sources.InfrastructureInterfaces;
using Sources.InfrastructureInterfaces.DTO;
using Sources.InfrastructureInterfaces.Factory;
using Sources.InfrastructureInterfaces.Scene;
using Sources.Services;
using Sources.Services.DomainServices;
using Sources.Services.Localization;
using Sources.Services.PlayerServices;
using Sources.ServicesInterfaces;
using Sources.ServicesInterfaces.Authorization;
using Sources.Utils;
using Unity.Services.Core;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Sources.Application
{
	public class Boot : LifetimeScope
	{
		private Game _game;

		public void StopCoroutineRunning(Coroutine coroutine)
		{
			if (coroutine == null)
				throw new ArgumentNullException(nameof(coroutine));

			StopCoroutine(coroutine);
		}

		protected override async void Configure(IContainerBuilder builder)
		{
			base.Configure(builder);

			#region BaseServices

			builder.RegisterEntryPoint<Game>();

			builder.Register<IAssetProvider, AssetProvider>(Lifetime.Singleton);
			builder.Register<LoadingCurtainFactory>(Lifetime.Scoped);

			builder.Register(
				container =>
				{
					var loadingCurtain = container.Resolve<LoadingCurtainFactory>().Create();
					loadingCurtain.gameObject.SetActive(true);
					return loadingCurtain;
				},
				Lifetime.Singleton
			).AsImplementedInterfaces().AsSelf();

			builder.Register<CoroutineRunnerFactory>(Lifetime.Scoped);
			builder.Register(
				container => container.Resolve<CoroutineRunnerFactory>().Create(),
				Lifetime.Singleton
			).AsImplementedInterfaces().AsSelf();

			builder.Register<ISceneLoader, SceneLoader>(Lifetime.Singleton);
			builder.Register<GameStateMachineFactory>(Lifetime.Singleton);

			#endregion

			#region InitializeServicesAndProgress

			builder.Register<ILocalizationService, LocalizationService>(Lifetime.Singleton);

			builder.Register<AuthorizationFactory>(Lifetime.Scoped);
			builder.Register<IAuthorization>(
				container => container.Resolve<AuthorizationFactory>().Create(),
				Lifetime.Scoped
			);

			builder.Register<IRewardService, YandexRewardsService>(Lifetime.Singleton);
			InitializeLeaderBoardService(builder);

			builder.Register<IUpgradeDataFactory, UpgradeDataFactory>(Lifetime.Scoped);

			builder.Register<IPersistentProgressService, PersistentProgressService>(Lifetime.Singleton);
			builder.Register<IProgressLoadDataService, ProgressLoadDataService>(Lifetime.Singleton);
			builder.Register<SaveLoaderFactory>(Lifetime.Scoped);

			builder.Register(
				container =>
				{
					var saveLoaderFactory = container.Resolve<SaveLoaderFactory>();
					return saveLoaderFactory.GetSaveLoader();
				},
				Lifetime.Singleton
			);

			CreateResourceService(builder);

			builder.Register<ProgressFactory>(Lifetime.Singleton);

			builder.Register(
				container =>
				{
					var a = container.Resolve<IPersistentProgressService>().GameProgress.ResourcesModel;
					return new ResourcesProgressPresenter(a);
				},
				Lifetime.Singleton
			).AsImplementedInterfaces();

			CreateSceneLoadServices(builder);

			#endregion

			#region InitializeProgressServices

			builder.Register<PlayerStatsFactory>(Lifetime.Scoped);
			builder.Register(
				container => container.Resolve<PlayerStatsFactory>()
					.CreatePlayerStats(container.Resolve<IPersistentProgressService>()),
				Lifetime.Singleton
			).AsImplementedInterfaces().AsSelf();

			builder.Register<IPlayerProgressProvider, PlayerProgressProvider>(Lifetime.Singleton);
			builder.Register<ILevelConfigGetter, LevelConfigGetter>(Lifetime.Singleton);
			builder.Register<IShopProgressProvider, ShopProgressProvider>(Lifetime.Singleton);
			builder.Register<IPlayerFactory, PlayerFactory>(Lifetime.Singleton);
			builder.Register<ILevelProgressFacade, LevelProgressFacade>(Lifetime.Singleton);
			builder.Register<ICameraFactory, CameraFactory>(Lifetime.Scoped);

			builder.Register<ShopElementFactory>(Lifetime.Scoped);

			#endregion

			builder.RegisterEntryPointExceptionHandler(e => Debug.LogError(e.Data));
		}

		private void CreateSceneLoadServices(IContainerBuilder builder)
		{
			ServicesLoadInvokerInformer servicesLoadInvokerInformer = new ServicesLoadInvokerInformer();

			builder.RegisterInstance<ISceneLoadInformer>(servicesLoadInvokerInformer);
			builder.RegisterInstance<ISceneLoadInvoker>(servicesLoadInvokerInformer);
		}

		private void CreateResourceService(IContainerBuilder builder)
		{
			ResourceServiceFactory resourceServiceFactory = new ResourceServiceFactory();

			Dictionary<ResourceType, IResource<int>> intResources = resourceServiceFactory.GetIntResources();
			Dictionary<ResourceType, IResource<float>> floatResources = resourceServiceFactory.GetFloatResources();

			builder.RegisterInstance<IResourceService>
			(
				new ResourcesService(
					intResources,
					floatResources
				)
			);
		}

		private void InitializeLeaderBoardService(IContainerBuilder builder)
		{
			LeaderBoard leaderBoardService = new LeaderBoard(GetLeaderboard());

			builder.RegisterInstance<ILeaderBoardService>(leaderBoardService);

#if YANDEX_GAMES
			builder.Register<IYandexSDKController, YandexGamesSdkFacade>(Lifetime.Singleton);
#endif
		}

		private async UniTask<ISaveLoader> GetSaveLoader(
			IYandexSDKController sdkController,
			IPersistentProgressService progressService
		)
		{
#if YANDEX_GAMES && YANDEX_CODE
			return new YandexSaveLoader(sdkController);
#endif

#if UNITY_EDITOR
			return await GetEditorSaveLoader(progressService);
#endif
		}

		private async UniTask<EditorSaveLoader> GetEditorSaveLoader(IPersistentProgressService progressService)
		{
			IUnityServicesController controller = new UnityServicesController(new InitializationOptions());

			await controller.InitializeUnityServices();

			EditorSaveLoader saveLoader = new EditorSaveLoader(progressService, controller);
			return saveLoader;
		}

		private IAbstractLeaderBoard GetLeaderboard()
		{
			return new TestLeaderBoardService();

#if !UNITY_EDITOR
			return new YandexLeaderboard();
#endif
		}
	}
}