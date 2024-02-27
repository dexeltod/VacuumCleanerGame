using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Sources.ApplicationServicesInterfaces;
using Sources.Controllers;
using Sources.DomainInterfaces.DomainServicesInterfaces;
using Sources.Infrastructure.Factories;
using Sources.Infrastructure.Factories.Player;
using Sources.Infrastructure.Providers;
using Sources.InfrastructureInterfaces.Factory;
using Sources.InfrastructureInterfaces.Providers;
using Sources.InfrastructureInterfaces.Services;
using Sources.InfrastructureInterfaces.States;
using Sources.ServicesInterfaces.Advertisement;
using VContainer;
using VContainer.Unity;

namespace Sources.Application
{
	public class GameBuilder : IAsyncStartable
	{
		private readonly IGameStateChangerProvider _gameStateChangerProvider;
		private readonly ProgressFactory _progressFactory;
		private readonly ISaveLoader _saveLoader;
		private readonly IGameStateChangerFactory _gameStateChangerFactory;
		private readonly ResourcePathConfigServiceFactory _resourcePathConfigServiceFactory;
		private readonly ResourcePathNameConfigProvider _pathNameConfigProvider;
		private readonly PlayerStatsFactory _playerStatsFactory;
		private readonly ISaveLoaderProvider _saveLoaderProvider;
		private readonly SaveLoaderFactory _saveLoaderFactory;
		private readonly AdvertisementHandlerProvider _advertisementHandlerProvider;
		private readonly IAdvertisement _advertisement;

		[Inject]
		public GameBuilder(
			ProgressFactory progressFactory,
			ISaveLoader saveLoader,
			IGameStateChangerProvider gameStateChangerProvider,
			IGameStateChangerFactory gameStateChangerFactory,
			ResourcePathConfigServiceFactory resourcePathConfigServiceFactory,
			ResourcePathNameConfigProvider pathNameConfigProvider,
			PlayerStatsFactory playerStatsFactory,
			IPlayerStatsServiceProvider playerStatsServiceProvider,
			IPersistentProgressServiceProvider persistentProgressService,
			ISaveLoaderProvider saveLoaderProvider,
			SaveLoaderFactory saveLoaderFactory,
			AdvertisementHandlerProvider advertisementHandlerProvider,
			IAdvertisement advertisement
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
			_playerStatsFactory = playerStatsFactory ?? throw new ArgumentNullException(nameof(playerStatsFactory));
			_saveLoaderProvider = saveLoaderProvider ?? throw new ArgumentNullException(nameof(saveLoaderProvider));
			_saveLoaderFactory = saveLoaderFactory ?? throw new ArgumentNullException(nameof(saveLoaderFactory));
			_advertisementHandlerProvider = advertisementHandlerProvider ??
				throw new ArgumentNullException(nameof(advertisementHandlerProvider));
			_advertisement = advertisement ?? throw new ArgumentNullException(nameof(advertisement));
		}

		private IGameStateChanger GameStateChanger => _gameStateChangerProvider.Implementation;

		public async UniTask StartAsync(CancellationToken cancellation)
		{
			await Initialize();
			DOTween.Init();
			GameStateChanger.Enter<IMenuState>();
		}

		private async UniTask Initialize()
		{
			_saveLoaderProvider.Register<ISaveLoader>(_saveLoaderFactory.GetSaveLoader());

			await _saveLoader.Initialize();
			await _progressFactory.Create();

			_advertisementHandlerProvider.Register(new AdvertisementHandler(_advertisement));
			_pathNameConfigProvider.Register(_resourcePathConfigServiceFactory.Create());
			_gameStateChangerProvider.Register(_gameStateChangerFactory.Create());

			_playerStatsFactory.Create();
		}
	}
}