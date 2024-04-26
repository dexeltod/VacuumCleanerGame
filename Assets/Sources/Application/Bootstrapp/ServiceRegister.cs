using System;
using System.Collections.Generic;
using Sources.Application.Services.Leaderboard;
using Sources.Application.UnityApplicationServices;
using Sources.ApplicationServicesInterfaces;
using Sources.ControllersInterfaces;
using Sources.DomainInterfaces;
using Sources.DomainInterfaces.DomainServicesInterfaces;
using Sources.Infrastructure;
using Sources.Infrastructure.Configs.Scripts;
using Sources.Infrastructure.Factories;
using Sources.Infrastructure.Factories.Domain;
using Sources.Infrastructure.Factories.Player;
using Sources.Infrastructure.Factories.Presenters;
using Sources.Infrastructure.Factories.Scene;
using Sources.Infrastructure.Factories.StateMachine;
using Sources.Infrastructure.Factories.UI;
using Sources.Infrastructure.Providers;
using Sources.Infrastructure.Repositories;
using Sources.Infrastructure.Repository;
using Sources.Infrastructure.Services;
using Sources.Infrastructure.StateMachine.GameStates;
using Sources.InfrastructureInterfaces;
using Sources.InfrastructureInterfaces.Common.Factories;
using Sources.InfrastructureInterfaces.Factory;
using Sources.InfrastructureInterfaces.Providers;
using Sources.InfrastructureInterfaces.Scene;
using Sources.Presentation.SceneEntity;
using Sources.PresentationInterfaces;
using Sources.Services;
using Sources.Services.DomainServices;
using Sources.Services.Localization;
using Sources.Services.Providers;
using Sources.ServicesInterfaces;
using Sources.ServicesInterfaces.Advertisement;
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
			_builder.Register<ILocalizationService, LocalizationService>(Lifetime.Singleton);
			_builder.Register<ITranslatorService, PhraseTranslatorService>(Lifetime.Singleton);
			_builder.Register<ProgressService>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();

			_builder.Register<ProgressionConfig>(Lifetime.Singleton);

			_builder.Register<ProgressCleaner>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();

			RegisterLoadingCurtain();

#endregion

#region ConstantNames

			_builder.Register<ProgressConstantNames>(Lifetime.Singleton);

#endregion

#region Providers

#region Repositories

			_builder.Register<ModifiableStatsRepositoryProvider>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
			_builder.Register<UpgradeProgressRepositoryProvider>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();

#endregion

			_builder.Register<SandCarContainerViewProvider>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
			_builder.Register<GameStateChangerProvider>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
			_builder.Register<FillMeshShaderControllerProvider>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
			_builder.Register<PlayerModelRepositoryProvider>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();

			_builder.Register<SandParticleSystemProvider>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
			_builder.Register<CloudServiceSdkFacadeProvider>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
			_builder.Register<AdvertisementHandlerProvider>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();

			_builder.Register<GameMenuPresenterProvider>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
			_builder.Register<SaveLoaderProvider>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
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
			_builder.Register<ResourcePathNameConfigProvider>(Lifetime.Singleton);

#endregion

#region Factories

			_builder.Register<ResourcePathConfigServiceFactory>(Lifetime.Scoped);
			_builder
				.Register<IPresentableFactory<IUpgradeWindowPresentation, IUpgradeWindowPresenter>,
					UpgradeWindowViewFactory>(
					Lifetime.Scoped
				).AsImplementedInterfaces();
			_builder.Register<ShopViewFactory>(Lifetime.Scoped);

			_builder.Register<SaveLoaderFactory>(Lifetime.Scoped).AsImplementedInterfaces().AsSelf();
			_builder.Register<InitialProgressFactory>(Lifetime.Scoped).AsImplementedInterfaces().AsSelf();
			_builder.Register<CoroutineRunnerFactory>(Lifetime.Scoped);
			_builder.Register<LoadingCurtainFactory>(Lifetime.Scoped);

			_builder.Register<GameplayInterfacePresenterFactory>(Lifetime.Scoped);
			_builder.Register<GameStatesRepositoryFactory>(Lifetime.Scoped);

			_builder.Register<GameStateChangerFactory>(Lifetime.Scoped).AsImplementedInterfaces();
			_builder.Register<ProgressFactory>(Lifetime.Scoped).AsImplementedInterfaces().AsSelf();

			_builder.Register<ResourcePathConfigServiceFactory>(Lifetime.Singleton);

			_builder.Register<IPlayerFactory, PlayerFactory>(Lifetime.Singleton);
			_builder.Register<IAssetFactory, AssetFactory>(Lifetime.Singleton);

			_builder.Register<ResourcesProgressPresenterFactory>(Lifetime.Singleton);

#endregion

#region States

			_builder.Register<MenuState>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
			_builder.Register<BuildSceneState>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
			_builder.Register<GameLoopState>(Lifetime.Singleton);

#endregion

#region InitializeServicesAndProgress

			InitializeLeaderBoardService(_builder);

			_builder.Register<ILevelChangerService, LevelChangerService>(Lifetime.Singleton);

			_builder.Register<IProgressSaveLoadDataService, ProgressSaveLoadDataService>(Lifetime.Singleton);

			RegisterCloudSavers();

			_builder.Register(
				container =>
				{
					SaveLoaderFactory saveLoaderFactory = container.Resolve<SaveLoaderFactory>();
					return saveLoaderFactory.GetSaveLoader();
				},
				Lifetime.Singleton
			).AsImplementedInterfaces().AsSelf();

			RegisterSaveLoader();
			CreateResourceService();
			CreateSceneLoadServices();

#endregion

#region InitializeProgressServices

			_builder.Register<ILevelConfigGetter, LevelConfigGetter>(Lifetime.Singleton);

			_builder.Register<ILevelProgressFacade, LevelProgressFacade>(Lifetime.Singleton);
			RegisterAdvertisement();

#endregion

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

		private void RegisterSaveLoader() { }

		private void RegisterCloudSavers()
		{
#if YANDEX_CODE
			_builder.Register<ICloudSave, YandexCloudSaveLoader>(Lifetime.Singleton);
#endif
#if !YANDEX_CODE
			_builder.Register<ICloudSave, UnityCloudSaveLoader>(Lifetime.Singleton);
#endif
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

			Dictionary<CurrencyResourceType, IResource<int>> intResources = resourceServiceFactory.GetIntResources();
			Dictionary<CurrencyResourceType, IResource<float>> floatResources
				= resourceServiceFactory.GetFloatResources();

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