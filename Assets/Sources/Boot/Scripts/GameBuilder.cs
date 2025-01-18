using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Sources.BusinessLogic.Interfaces.Factory;
using Sources.BusinessLogic.Interfaces.Factory.StateMachine;
using Sources.BusinessLogic.Services;
using Sources.BusinessLogic.ServicesInterfaces;
using Sources.BusinessLogic.States;
using Sources.ControllersInterfaces;
using Sources.DomainInterfaces.DomainServicesInterfaces;
using Sources.Infrastructure.Factories;
using Sources.Infrastructure.Factories.Domain;
using Sources.Infrastructure.Factories.Progress;
using Sources.Infrastructure.Services;
using VContainer;
using VContainer.Unity;

namespace Sources.Boot.Scripts
{
	public class GameBuilder : IAsyncStartable
	{
		private readonly IGameStateChanger _gameStateChangerProvider;
		private readonly ProgressFactory _progressFactory;
		private readonly ISaveLoader _saveLoader;
		private readonly IGameStateChangerFactory _gameStateChangerFactory;
		private readonly ResourcePathConfigServiceFactory _resourcePathConfigServiceFactory;
		private readonly ResourcesPrefabs _pathNameConfigProvider;
		private readonly IPersistentProgressServiceUpdatable _persistentProgressService;
		private readonly ISaveLoader _saveLoaderProvider;
		private readonly SaveLoaderFactory _saveLoaderFactory;
		private readonly IAdvertisementPresenter _advertisement;
		private readonly IAssetFactory _assetFactory;

		[Inject]
		public GameBuilder(
			ProgressFactory progressFactory,
			ISaveLoader saveLoader,
			IGameStateChanger gameStateChangerProvider,
			IGameStateChangerFactory gameStateChangerFactory,
			ResourcePathConfigServiceFactory resourcePathConfigServiceFactory,
			ResourcesPrefabs pathNameConfigProvider,
			IPersistentProgressServiceUpdatable persistentProgressService,
			ISaveLoader saveLoaderProvider,
			SaveLoaderFactory saveLoaderFactory,
			IAdvertisementPresenter advertisement,
			IAssetFactory assetFactory
		)
		{
			_progressFactory = progressFactory ?? throw new ArgumentNullException(nameof(progressFactory));
			_saveLoader = saveLoader ?? throw new ArgumentNullException(nameof(saveLoader));
			_gameStateChangerProvider = gameStateChangerProvider ??
			                            throw new ArgumentNullException(nameof(gameStateChangerProvider));
			_gameStateChangerFactory = gameStateChangerFactory ??
			                           throw new ArgumentNullException(nameof(gameStateChangerFactory));
			_resourcePathConfigServiceFactory = resourcePathConfigServiceFactory ??
			                                    throw new ArgumentNullException(nameof(resourcePathConfigServiceFactory));
			_pathNameConfigProvider = pathNameConfigProvider ??
			                          throw new ArgumentNullException(nameof(pathNameConfigProvider));

			_persistentProgressService =
				persistentProgressService ?? throw new ArgumentNullException(nameof(persistentProgressService));

			_saveLoaderProvider = saveLoaderProvider ?? throw new ArgumentNullException(nameof(saveLoaderProvider));
			_saveLoaderFactory = saveLoaderFactory ?? throw new ArgumentNullException(nameof(saveLoaderFactory));
			_advertisement = advertisement ?? throw new ArgumentNullException(nameof(advertisement));
			_assetFactory = assetFactory ?? throw new ArgumentNullException(nameof(assetFactory));
		}

		public async UniTask StartAsync(CancellationToken cancellation)
		{
			await Initialize();

			DOTween.Init();

			var a = new GameStateChangerFactory();

			_gameStateChangerProvider.Enter<IMenuState>();
		}

		private async UniTask Initialize()
		{
			await _saveLoader.Initialize();

			_persistentProgressService.Update(await _progressFactory.Create());

			IGameStateChanger a = _gameStateChangerFactory.Create();
		}
	}
}
