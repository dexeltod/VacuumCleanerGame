using System;
using System.Collections.Generic;
using Sources.Application.Leaderboard;
using Sources.Application.StateMachine;
using Sources.Application.StateMachine.GameStates;
using Sources.Application.UI;
using Sources.Application.UnityApplicationServices;
using Sources.Application.YandexSDK;
using Sources.ApplicationServicesInterfaces;
using Sources.ApplicationServicesInterfaces.StateMachineInterfaces;
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
using Sources.Presentation.SceneEntity;
using Sources.Services;
using Sources.Services.DomainServices;
using Sources.Services.Interfaces;
using Sources.Services.Localization;
using Sources.ServicesInterfaces;
using Sources.ServicesInterfaces.Authorization;
using Sources.ServicesInterfaces.UI;
using Sources.Utils;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using UpgradeWindowFactory = Sources.Application.UI.UpgradeWindowFactory;
#if YANDEX_CODE
using Sources.Services.DomainServices.YandexLeaderboard;
#endif

namespace Sources.Application
{
	public class ServiceRegister
	{
		private readonly IContainerBuilder _builder;

		public ServiceRegister(IContainerBuilder builder) =>
			_builder = builder ?? throw new ArgumentNullException(nameof(builder));

		public void Register()
		{
			_builder.RegisterEntryPoint<Game>();

#region ConstantNames

			_builder.Register<ProgressConstantNames>(Lifetime.Singleton);

#endregion

#region BaseServices

			_builder.Register<ILocalizationService, LocalizationService>(Lifetime.Singleton);
			_builder.Register<ITranslatorService, PhraseTranslatorService>(Lifetime.Singleton);

			_builder.Register<IAssetProvider, AssetProvider>(Lifetime.Singleton);

			RegisterLoadingCurtain();
			RegisterCoroutineRunner();

			_builder.Register<IGameStateMachine, GameStateMachine>(Lifetime.Singleton);
			_builder.Register<ISceneLoader, SceneLoader>(Lifetime.Singleton);
			_builder.Register<GameStateMachineFactory>(Lifetime.Singleton);

#endregion

#region InitializeServicesAndProgress

			RegisterAuthorization();

			InitializeLeaderBoardService(_builder);

			_builder.Register<LevelChangerPresenter>(Lifetime.Singleton);
			_builder.Register<IProgressUpgradeFactory, ProgressUpgradeFactory>(Lifetime.Scoped);
			_builder.Register<IProgressLoadDataService, ProgressLoadDataService>(Lifetime.Singleton);

			RegisterCloudSavers();

			RegisterSaveLoader();

			CreateResourceService();

			_builder.Register<InitialProgressFactory>(Lifetime.Scoped);
			_builder.Register<ProgressFactory>(Lifetime.Scoped);

			_builder.Register<PersistentProgressService>(
				container =>
				{
					PersistentProgressService persistentProgressService = new PersistentProgressService();
					InitialProgressFactory initialProgressFactory = container.Resolve<InitialProgressFactory>();
					persistentProgressService.Set(initialProgressFactory.Create());

					return persistentProgressService;
				},
				Lifetime.Singleton
			).AsImplementedInterfaces().AsSelf();
			RegisterResourceProgressPresenter();
			CreateSceneLoadServices();

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
			RegisterAdvertisement();

#endregion

			_builder.Register<IUIFactory, UIFactory>(Lifetime.Scoped).AsImplementedInterfaces();
			_builder.Register<IUpgradeWindowFactory, UpgradeWindowFactory>(Lifetime.Scoped).AsImplementedInterfaces();

			_builder.Register<ICameraFactory, CameraFactory>(Lifetime.Scoped);
			_builder.Register<IRegisterWindowLoader, RegisterWindowLoader>(Lifetime.Singleton);
			_builder.Register<ShopElementFactory>(Lifetime.Scoped);

			_builder.RegisterEntryPointExceptionHandler(
				exception => Debug.LogError(exception.Message)
			);
		}

		private void RegisterAdvertisement()
		{
#if YANDEX_CODE
			_builder.Register<IAdvertisement, YandexAdvertisement>(Lifetime.Singleton);
			return;
#endif
			_builder.Register<IAdvertisement, EditorAdvertisement>(Lifetime.Singleton);
		}

		private void RegisterResourceProgressPresenter() =>
			_builder.Register<ResourcesProgressPresenter>(
				container =>
				{
					IResourcesModel resourcesModel
						= container.Resolve<IPersistentProgressService>().GameProgress.ResourcesModel;
					return new ResourcesProgressPresenter(resourcesModel);
				},
				Lifetime.Singleton
			).AsImplementedInterfaces();

		private void RegisterSaveLoader()
		{
			_builder.Register<SaveLoaderFactory>(Lifetime.Scoped).AsImplementedInterfaces().AsSelf();
			_builder.Register(
				container =>
				{
					SaveLoaderFactory saveLoaderFactory = container.Resolve<SaveLoaderFactory>();
					return saveLoaderFactory.GetSaveLoader();
				},
				Lifetime.Singleton
			).AsImplementedInterfaces().AsSelf();
		}

		private void RegisterCloudSavers()
		{
#if YANDEX_CODE
			_builder.Register<ICloudSave, YandexCloudSaveLoader>(Lifetime.Singleton);
#endif
#if !YANDEX_CODE
			_builder.Register<ICloudSave, UnityCloudSaveLoader>(Lifetime.Singleton);
#endif
		}

		private void RegisterAuthorization()
		{
			_builder.Register<AuthorizationFactory>(Lifetime.Scoped);
			_builder.Register<IAuthorization>(
				container => container.Resolve<AuthorizationFactory>().Create(),
				Lifetime.Singleton
			);
		}

		private void RegisterCoroutineRunner() =>
			_builder.Register<CoroutineRunnerFactory>(Lifetime.Scoped);

		private void RegisterLoadingCurtain()
		{
			_builder.Register<LoadingCurtainFactory>(Lifetime.Scoped);
			_builder.Register(
				container =>
				{
					LoadingCurtain loadingCurtain = container.Resolve<LoadingCurtainFactory>().Create();
					loadingCurtain.gameObject.SetActive(true);
					return loadingCurtain;
				},
				Lifetime.Singleton
			).AsImplementedInterfaces().AsSelf();
		}

		private void CreateSceneLoadServices()
		{
			ServicesLoadInvokerInformer servicesLoadInvokerInformer = new ServicesLoadInvokerInformer();

			_builder.RegisterInstance<ISceneLoadInformer>(servicesLoadInvokerInformer);
			_builder.RegisterInstance<ISceneLoadInvoker>(servicesLoadInvokerInformer);
		}

		private void CreateResourceService()
		{
			ResourceServiceFactory resourceServiceFactory = new ResourceServiceFactory();

			Dictionary<ResourceType, IResource<int>> intResources = resourceServiceFactory.GetIntResources();
			Dictionary<ResourceType, IResource<float>> floatResources = resourceServiceFactory.GetFloatResources();

			_builder.RegisterInstance<IResourceService>
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
		}

		private IAbstractLeaderBoard GetLeaderboard()
		{
#if YANDEX_CODE
			return new YandexLeaderboard();
#endif
			return new TestLeaderBoardService();
		}
	}
}