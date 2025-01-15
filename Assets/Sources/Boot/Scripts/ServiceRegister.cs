using System;
using Sources.BuisenessLogic;
using Sources.BuisenessLogic.Interfaces;
using Sources.BuisenessLogic.Interfaces.Factory;
using Sources.BuisenessLogic.Interfaces.Factory.StateMachine;
using Sources.BuisenessLogic.Scene;
using Sources.BuisenessLogic.ServicesInterfaces;
using Sources.BuisenessLogic.StateMachine.GameStates;
using Sources.ControllersInterfaces;
using Sources.Domain.Progress;
using Sources.DomainInterfaces;
using Sources.DomainInterfaces.DomainServicesInterfaces;
using Sources.Infrastructure.Factories;
using Sources.Infrastructure.Factories.Domain;
using Sources.Infrastructure.Factories.Player;
using Sources.Infrastructure.Factories.Presenters;
using Sources.Infrastructure.Factories.Progress;
using Sources.Infrastructure.Leaderboard;
using Sources.Infrastructure.Services;
using Sources.Infrastructure.Services.DomainServices;
using Sources.InfrastructureInterfaces.Configs.Scripts;
using Sources.Presentation.Factories;
using Sources.Presentation.Factories.Scene;
using Sources.Presentation.Factories.UI;
using Sources.Presentation.SceneEntity;
using Sources.PresentationInterfaces;
using UnityEngine;
using VContainer;
using VContainer.Unity;
#if YANDEX_CODE
using Sources.Infrastructure.Yandex;
using Sources.Services.DomainServices.YandexLeaderboard;
#endif

namespace Sources.Boot.Scripts
{
	public class ServiceRegister
	{
		private readonly IContainerBuilder _builder;
		private IAssetFactory _assetFactory;

		public ServiceRegister(IContainerBuilder builder) =>
			_builder = builder ?? throw new ArgumentNullException(nameof(builder));

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
			_builder.Register<ILocalizationService, LocalizationService>(Lifetime.Singleton);
			_builder.Register<TranslatorService>(Lifetime.Singleton);

			_builder.RegisterFactory<IGlobalProgress, GlobalProgress>(
				resolver => { await resolver.Resolve<ProgressFactory>().Create(); },
				Lifetime.Singleton
			);

			_builder.Register<ProgressionConfig>(Lifetime.Singleton);
			_builder.Register<ProgressServiceRegister>(Lifetime.Singleton);

			_builder.Register<ProgressCleaner>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();

			RegisterLoadingCurtain();

			#endregion

			new ProviderRegister(_builder, _assetFactory).Register();

			_builder.Register<GameStateChangerFactory>(Lifetime.Scoped).AsImplementedInterfaces();
			_builder.Register<GameStatesRepositoryFactory>(Lifetime.Scoped);

			#region Factories

			_builder.Register<ResourcePathConfigServiceFactory>(Lifetime.Scoped);

			_builder.Register<IPresentableFactory<IUpgradeWindowPresentation, IUpgradeWindowPresenter>,
				UpgradeWindowViewFactory>(Lifetime.Scoped).AsImplementedInterfaces();

			_builder.Register<ShopViewFactory>(Lifetime.Scoped);

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
				container =>
				{
					LoadingCurtain loadingCurtain = container.Resolve<LoadingCurtainFactory>().Create();
					loadingCurtain.gameObject.SetActive(
						true
					);
					return loadingCurtain;
				},
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
					resourceServiceFactory.CreateIntCurrencies(),
					resourceServiceFactory.GetFloatResources()
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
