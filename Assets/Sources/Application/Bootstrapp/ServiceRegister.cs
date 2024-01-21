using System.Collections.Generic;
using Sources.Application.Leaderboard;
using Sources.Application.StateMachine.GameStates;
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
using Sources.ServicesInterfaces;
using Sources.ServicesInterfaces.Authorization;
using Sources.Utils;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Sources.Application
{
	public class ServiceRegister
	{
		private readonly IContainerBuilder _builder;

		public ServiceRegister(IContainerBuilder builder)
		{
			_builder = builder;
		}

		public void Register()
		{
			#region BaseServices

			_builder.RegisterEntryPoint<Game>();

			_builder.Register<IAssetProvider, AssetProvider>(Lifetime.Singleton);
			_builder.Register<LoadingCurtainFactory>(Lifetime.Scoped);

			_builder.Register(
				container =>
				{
					var loadingCurtain = container.Resolve<LoadingCurtainFactory>().Create();
					loadingCurtain.gameObject.SetActive(true);
					return loadingCurtain;
				},
				Lifetime.Singleton
			).AsImplementedInterfaces().AsSelf();

			_builder.Register<CoroutineRunnerFactory>(Lifetime.Scoped);
			_builder.Register(
				container => container.Resolve<CoroutineRunnerFactory>().Create(),
				Lifetime.Singleton
			).AsImplementedInterfaces().AsSelf();

			_builder.Register<ISceneLoader, SceneLoader>(Lifetime.Singleton);
			_builder.Register<GameStateMachineFactory>(Lifetime.Singleton);

			#endregion

			#region InitializeServicesAndProgress

			_builder.Register<ILocalizationService, LocalizationService>(Lifetime.Singleton);

			_builder.Register<AuthorizationFactory>(Lifetime.Scoped);
			_builder.Register<IAuthorization>(
				container => container.Resolve<AuthorizationFactory>().Create(),
				Lifetime.Scoped
			);

			_builder.Register<IRewardService, YandexRewardsService>(Lifetime.Singleton);
			InitializeLeaderBoardService(_builder);

			_builder.Register<IUpgradeDataFactory, UpgradeDataFactory>(Lifetime.Scoped);

			_builder.Register<IPersistentProgressService, PersistentProgressService>(Lifetime.Singleton);
			_builder.Register<IProgressLoadDataService, ProgressLoadDataService>(Lifetime.Singleton);
			_builder.Register<SaveLoaderFactory>(Lifetime.Scoped);

			_builder.Register(
				container =>
				{
					var saveLoaderFactory = container.Resolve<SaveLoaderFactory>();
					return saveLoaderFactory.GetSaveLoader();
				},
				Lifetime.Singleton
			);

			CreateResourceService(_builder);

			_builder.Register<ProgressFactory>(Lifetime.Singleton);

			_builder.Register(
				container =>
				{
					var resourcesModel = container.Resolve<IPersistentProgressService>().GameProgress.ResourcesModel;
					return new ResourcesProgressPresenter(resourcesModel);
				},
				Lifetime.Singleton
			).AsImplementedInterfaces();

			CreateSceneLoadServices(_builder);

			#endregion

			#region InitializeProgressServices

			_builder.Register<PlayerStatsFactory>(Lifetime.Scoped);
			_builder.Register(
				container => container.Resolve<PlayerStatsFactory>()
					.CreatePlayerStats(container.Resolve<IPersistentProgressService>()),
				Lifetime.Singleton
			).AsImplementedInterfaces().AsSelf();

			_builder.Register<IPlayerProgressProvider, PlayerProgressProvider>(Lifetime.Singleton);
			_builder.Register<ILevelConfigGetter, LevelConfigGetter>(Lifetime.Singleton);
			_builder.Register<IShopProgressProvider, ShopProgressProvider>(Lifetime.Singleton);
			_builder.Register<IPlayerFactory, PlayerFactory>(Lifetime.Singleton);
			_builder.Register<ILevelProgressFacade, LevelProgressFacade>(Lifetime.Singleton);
			_builder.Register<ICameraFactory, CameraFactory>(Lifetime.Scoped);

			_builder.Register<ShopElementFactory>(Lifetime.Scoped);

			#endregion

			_builder.RegisterEntryPointExceptionHandler(
				exception =>
				{
					Debug.LogError(exception.Message);
					Debug.LogErrorFormat(exception.Message);
				}
			);
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
			builder.Register<IYandexSDKController, YandexGamesSdkFacade>(Lifetime.Singleton);
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