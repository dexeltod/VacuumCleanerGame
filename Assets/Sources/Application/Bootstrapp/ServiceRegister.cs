using System;
using System.Collections.Generic;
using Sources.Application.Services.Leaderboard;
using Sources.Application.UnityApplicationServices;
using Sources.ApplicationServicesInterfaces;
using Sources.ApplicationServicesInterfaces.Authorization;
using Sources.ControllersInterfaces;
using Sources.DomainInterfaces;
using Sources.DomainInterfaces.DomainServicesInterfaces;
using Sources.Infrastructure;
using Sources.Infrastructure.DataViewModel;
using Sources.Infrastructure.Factories;
using Sources.Infrastructure.Factories.Domain;
using Sources.Infrastructure.Factories.LeaderBoard;
using Sources.Infrastructure.Factories.Player;
using Sources.Infrastructure.Factories.Presenters;
using Sources.Infrastructure.Factories.Scene;
using Sources.Infrastructure.Factories.UI;
using Sources.Infrastructure.Factories.UpgradeShop;
using Sources.Infrastructure.Providers;
using Sources.Infrastructure.Shop;
using Sources.Infrastructure.StateMachine.GameStates;
using Sources.InfrastructureInterfaces.Common.Factories;
using Sources.InfrastructureInterfaces.Factory;
using Sources.InfrastructureInterfaces.Providers;
using Sources.InfrastructureInterfaces.Scene;
using Sources.Presentation.SceneEntity;
using Sources.PresentationInterfaces;
using Sources.Services;
using Sources.Services.DomainServices;
using Sources.Services.Localization;
using Sources.Services.PlayerServices;
using Sources.Services.Providers;
using Sources.ServicesInterfaces;
using Sources.ServicesInterfaces.Advertisement;
using Sources.ServicesInterfaces.DTO;
using Sources.Utils;
using Sources.Utils.ConstantNames;
using UnityEngine;
using VContainer;
using VContainer.Unity;
#if YANDEX_CODE
using Sources.Services.DomainServices.YandexLeaderboard;
#endif

namespace Sources.Application.Bootstrapp
{
	public class ServiceRegister
	{
		private readonly IContainerBuilder _builder;

		public ServiceRegister(IContainerBuilder builder) =>
			_builder = builder ?? throw new ArgumentNullException(nameof(builder));

		public void Register()
		{
			_builder.RegisterEntryPoint<GameBuilder>();

#region BaseServices

			_builder.Register<ISceneLoader, SceneLoader>(Lifetime.Singleton);
			_builder.Register<IShopProgressFacade, ShopProgressFacade>(Lifetime.Singleton);
			_builder.Register<ILocalizationService, LocalizationService>(Lifetime.Singleton);
			_builder.Register<ITranslatorService, PhraseTranslatorService>(Lifetime.Singleton);

			RegisterLoadingCurtain();

#endregion

#region States

#region ConstantNames

			_builder.Register<ProgressConstantNames>(Lifetime.Singleton);

#endregion

#region Providers

			_builder.Register<SandCarContainerViewProvider>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
			_builder.Register<GameStateChangerProvider>(Lifetime.Singleton).AsImplementedInterfaces();
			_builder.Register<FillMeshShaderControllerProvider>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
			_builder.Register<PlayerStatsServiceProvider>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
			_builder.Register<SandParticleSystemProvider>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
			_builder.Register<GameplayInterfaceProvider>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
			_builder.Register<PlayerProgressSetterFacadeProvider>(Lifetime.Singleton).AsImplementedInterfaces()
				.AsSelf();
			_builder.Register<GameplayInterfacePresenterProvider>(Lifetime.Singleton).AsImplementedInterfaces()
				.AsSelf();
			_builder.Register<PersistentProgressServiceProvider>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
			_builder.Register<UpgradeWindowPresenterProvider>(Lifetime.Singleton);
			_builder.Register<ICoroutineRunnerProvider, CoroutineRunnerProvider>(Lifetime.Singleton).AsSelf();
			_builder.Register<ResourcesProgressPresenterProvider>(Lifetime.Singleton).AsImplementedInterfaces()
				.AsSelf();

			_builder.Register<DissolveShaderViewControllerProvider>(Lifetime.Singleton);
			_builder.Register<ResourcePathConfigProvider>(Lifetime.Singleton);

#endregion

#region Factories

			_builder.Register<ResourcePathConfigServiceFactory>(Lifetime.Scoped);
			_builder.Register<IPresentableFactory<IUpgradeWindow, IUpgradeWindowPresenter>, UpgradeWindowViewFactory>(
				Lifetime.Scoped
			).AsImplementedInterfaces();
			_builder.Register<ICameraFactory, CameraFactory>(Lifetime.Scoped);
			_builder.Register<ShopElementFactory>(Lifetime.Scoped);

			_builder.Register<SaveLoaderFactory>(Lifetime.Scoped).AsImplementedInterfaces().AsSelf();
			_builder.Register<CoroutineRunnerFactory>(Lifetime.Scoped);
			_builder.Register<Coroutine>(Lifetime.Scoped);
			_builder.Register<LoadingCurtainFactory>(Lifetime.Scoped);
			_builder.Register<InitialProgressFactory>(Lifetime.Scoped);
			_builder.Register<GameplayInterfacePresenterFactory>(Lifetime.Scoped);
			_builder.Register<GameStatesRepositoryFactory>(Lifetime.Scoped);
			_builder.Register<GameStateContainerFactory>(Lifetime.Scoped);
			_builder.Register<IProgressUpgradeFactory, ProgressUpgradeFactory>(Lifetime.Scoped);
			_builder.Register<GameStateChangerFactory>(Lifetime.Scoped).AsImplementedInterfaces();
			_builder.Register<ProgressFactory>(Lifetime.Scoped);
			_builder.Register<PlayerStatsFactory>(Lifetime.Scoped);
			_builder.Register<ResourcePathConfigServiceFactory>(Lifetime.Singleton);

			_builder.Register<IPlayerFactory, PlayerFactory>(Lifetime.Singleton);
			_builder.Register<IAssetFactory, AssetFactory>(Lifetime.Singleton);

			_builder.Register<ResourcesProgressPresenterFactory>(Lifetime.Singleton);

#endregion

			_builder.Register<MenuState>(Lifetime.Singleton);
			_builder.Register<BuildSceneState>(Lifetime.Singleton).AsImplementedInterfaces();
			_builder.Register<GameLoopState>(Lifetime.Singleton);

#endregion

#region InitializeServicesAndProgress

			RegisterAuthorization();

			InitializeLeaderBoardService(_builder);

			_builder.Register<ILevelChangerService, LevelChangerService>(Lifetime.Singleton);

			_builder.Register<IProgressSaveLoadDataService, ProgressSaveLoadDataService>(Lifetime.Singleton);

			RegisterCloudSavers();

			RegisterSaveLoader();

			CreateResourceService();

			CreateSceneLoadServices();

#endregion

#region InitializeProgressServices

			_builder.Register<ILevelConfigGetter, LevelConfigGetter>(Lifetime.Singleton);

			_builder.Register<ILevelProgressFacade, LevelProgressFacade>(Lifetime.Singleton);
			RegisterAdvertisement();

#endregion

			_builder.Register<IRegisterWindowLoader, RegisterWindowLoader>(Lifetime.Singleton);

			_builder.RegisterEntryPointExceptionHandler(exception => Debug.LogError(exception.Message));
		}

		private void RegisterAdvertisement()
		{
#if YANDEX_CODE
			_builder.Register<IAdvertisement, YandexAdvertisement>(Lifetime.Singleton);
			return;
#endif
			_builder.Register<IAdvertisement, EditorAdvertisement>(Lifetime.Singleton);
		}

		private void RegisterSaveLoader()
		{
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

		private void RegisterLoadingCurtain() =>
			_builder.Register(
				container =>
				{
					LoadingCurtain loadingCurtain = container.Resolve<LoadingCurtainFactory>().Create();
					loadingCurtain.gameObject.SetActive(true);
					return loadingCurtain;
				},
				Lifetime.Singleton
			).AsImplementedInterfaces().AsSelf();

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