using System;
using Sources.BusinessLogic;
using Sources.BusinessLogic.Interfaces;
using Sources.BusinessLogic.Interfaces.Configs;
using Sources.BusinessLogic.Interfaces.Factory;
using Sources.BusinessLogic.Interfaces.Factory.StateMachine;
using Sources.BusinessLogic.Scene;
using Sources.BusinessLogic.ServicesInterfaces;
using Sources.BusinessLogic.StateMachine.GameStates;
using Sources.Controllers.Factories;
using Sources.DomainInterfaces;
using Sources.DomainInterfaces.DomainServicesInterfaces;
using Sources.Infrastructure.Factories;
using Sources.Infrastructure.Factories.Domain;
using Sources.Infrastructure.Factories.Presentation;
using Sources.Infrastructure.Factories.Presentation.LeaderBoard;
using Sources.Infrastructure.Factories.Presentation.Scene;
using Sources.Infrastructure.Factories.Presentation.UI;
using Sources.Infrastructure.Factories.Presenters;
using Sources.Infrastructure.Factories.Progress;
using Sources.Infrastructure.Leaderboard;
using Sources.Infrastructure.Services;
using Sources.Infrastructure.Services.DomainServices;
using Sources.InfrastructureInterfaces.Configs.Scripts;
using UnityEngine;
using VContainer;
using VContainer.Unity;
#if YANDEX_CODE
#endif

namespace Sources.Boot.Scripts
{
	public class ServiceRegister
	{
		private readonly IContainerBuilder _builder;
		private readonly IObjectResolver _resolver;
		private IAssetFactory _assetFactory;

		public ServiceRegister(IContainerBuilder builder)
		{
			_builder = builder ?? throw new ArgumentNullException(nameof(builder));
		}

		public void Register()
		{
			_builder.RegisterEntryPoint<GameBuilder>();

			#region BaseServices

			_builder.Register<IInjectableAssetFactory, InjectableAssetFactory>(Lifetime.Singleton).As<IInjectableAssetFactory>()
				.AsSelf();

			_builder.Register(
				_ =>
				{
					_assetFactory = new AssetFactory();
					return _assetFactory;
				},
				Lifetime.Singleton
			).As<IAssetFactory>().AsSelf();

			_builder.Register<ISceneLoader, SceneLoader>(Lifetime.Singleton);
			_builder.Register<IResourcesPrefabs, ResourcesPrefabs>(Lifetime.Singleton);
			_builder.Register<ILocalizationService, LocalizationService>(Lifetime.Singleton);
			_builder.Register<TranslatorService>(Lifetime.Singleton);
			_builder.Register<InitialProgressFactory>(Lifetime.Singleton).AsImplementedInterfaces();

			_builder.Register<ProgressionConfig>(Lifetime.Singleton);
			_builder.Register<ProgressServiceRegister>(Lifetime.Singleton);

			_builder.Register<ClearProgressFactory>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();

			RegisterLoadingCurtain();

			#endregion

			new ProviderRegister(_builder).Register();

			_builder.Register<GameStateChangerFactory>(Lifetime.Scoped).AsImplementedInterfaces();
			_builder.Register<GameStatesRepositoryFactory>(Lifetime.Scoped);

			#region Factories

			_builder.Register<ResourcePathConfigServiceFactory>(Lifetime.Scoped);

			_builder.Register<ShopViewFactory>(Lifetime.Scoped);
			_builder.Register<MainMenuFactory>(Lifetime.Scoped).AsSelf().AsImplementedInterfaces();
			_builder.Register<LeaderBoardPlayersFactory>(Lifetime.Scoped).AsSelf().AsImplementedInterfaces();

			_builder.Register<SaveLoaderFactory>(
				Lifetime.Scoped
			).AsImplementedInterfaces().AsSelf();

			_builder.Register<InitialProgressFactory>(Lifetime.Scoped).AsImplementedInterfaces().AsSelf();
			_builder.Register<CoroutineRunnerFactory>(Lifetime.Scoped);
			_builder.Register<LoadingCurtainFactory>(Lifetime.Scoped);
			_builder.Register<GameplayInterfacePresenterFactory>(Lifetime.Scoped);
			_builder.Register<ProgressFactory>(Lifetime.Scoped).AsImplementedInterfaces().AsSelf();
			_builder.Register<ResourcePathConfigServiceFactory>(Lifetime.Singleton);

			_builder.Register<IPlayerFactory, PlayerFactory>(Lifetime.Singleton);

			_builder.Register<ResourcesProgressPresenterFactory>(Lifetime.Singleton);

			#endregion

			#region States

			_builder.Register<MenuState>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
			_builder.Register<BuildSceneState>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
			_builder.Register<GameLoopState>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();

			#endregion

			#region InitializeServicesAndProgress

			InitializeLeaderBoardService(_builder);

			_builder.Register<ILevelChangerService, LevelChangerService>(Lifetime.Singleton);

			_builder.Register<IProgressSaveLoadDataService, ProgressSaveLoadDataService>(Lifetime.Singleton);

			RegisterCloudSavers();

			_builder.Register<ISaveLoader>(container => new SaveLoaderFactory().Create(), Lifetime.Singleton)
				.AsImplementedInterfaces().AsSelf();

			CreateResourceService();
			CreateSceneLoadServices();

			#endregion

			#region InitializeProgressServices

			_builder.Register<ILevelConfigGetter, LevelConfigGetter>(Lifetime.Singleton);

			_builder.Register<ILevelProgressFacade, LevelProgressFacade>(Lifetime.Singleton);

			#endregion

			_builder.RegisterEntryPointExceptionHandler(
				exception => Debug.LogError(exception.Message)
			);
		}

		private void RegisterCloudSavers()
		{
		}

		private void RegisterLoadingCurtain() =>
			_builder.Register(
				container => container.Resolve<LoadingCurtainFactory>().Create(),
				Lifetime.Singleton
			).AsImplementedInterfaces().AsSelf();

		private void CreateSceneLoadServices()
		{
			ServicesLoadInvokerInformer servicesLoadInvokerInformer = new ServicesLoadInvokerInformer();

			_builder.RegisterInstance<ISceneLoadInformer>(
				servicesLoadInvokerInformer
			);
			_builder.RegisterInstance<ISceneLoadInvoker>(
				servicesLoadInvokerInformer
			);
		}

		private void CreateResourceService()
		{
			ResourceServiceFactory resourceServiceFactory = new ResourceServiceFactory();

			_builder.RegisterInstance<IResourcesRepository>
			(
				new ResourcesRepository(
					resourceServiceFactory.CreateIntCurrencies()
				)
			);
		}

		private void InitializeLeaderBoardService(IContainerBuilder builder)
		{
			LeaderBoardRepository leaderBoardRepositoryService = new LeaderBoardRepository(
				GetLeaderboard()
			);

			builder.RegisterInstance<ILeaderBoardService>(
				leaderBoardRepositoryService
			);
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