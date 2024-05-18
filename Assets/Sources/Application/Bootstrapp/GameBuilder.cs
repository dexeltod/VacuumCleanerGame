using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Sources.Controllers;
using Sources.DomainInterfaces.DomainServicesInterfaces;
using Sources.Infrastructure.Factories;
using Sources.Infrastructure.Factories.Domain;
using Sources.Infrastructure.Factories.Player;
using Sources.Infrastructure.Providers;
using Sources.InfrastructureInterfaces.Factory;
using Sources.InfrastructureInterfaces.Providers;
using Sources.InfrastructureInterfaces.Services;
using Sources.InfrastructureInterfaces.States;
using Sources.ServicesInterfaces.Advertisement;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Sources.Application.Bootstrapp
{
	public class GameBuilder : IAsyncStartable
	{
		private readonly IGameStateChangerProvider _gameStateChangerProvider;
		private readonly ProgressFactory _progressFactory;
		private readonly ISaveLoader _saveLoader;
		private readonly IGameStateChangerFactory _gameStateChangerFactory;
		private readonly ResourcePathConfigServiceFactory _resourcePathConfigServiceFactory;
		private readonly ResourcePathNameConfigProvider _pathNameConfigProvider;
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
			_saveLoaderProvider = saveLoaderProvider ?? throw new ArgumentNullException(nameof(saveLoaderProvider));
			_saveLoaderFactory = saveLoaderFactory ?? throw new ArgumentNullException(nameof(saveLoaderFactory));
			_advertisementHandlerProvider = advertisementHandlerProvider ??
				throw new ArgumentNullException(nameof(advertisementHandlerProvider));
			_advertisement = advertisement ?? throw new ArgumentNullException(nameof(advertisement));
		}

		private IGameStateChanger GameStateChanger => _gameStateChangerProvider.Self;

		public async UniTask StartAsync(CancellationToken cancellation)
		{
			Debug.Log("Initialize");
			await Initialize();
			Debug.Log("Initialized");
			DOTween.Init();
			Debug.Log("GameStateChanger.Enter<IMenuState>();}");
			GameStateChanger.Enter<IMenuState>();
		}

		private async UniTask Initialize()
		{
			Debug.Log("Start Building");
			_saveLoaderProvider.Register<ISaveLoader>(_saveLoaderFactory.GetSaveLoader());

			await _saveLoader.Initialize();
			await _progressFactory.Create();
			Debug.Log("Progress created");

			Debug.Log("register _advertisementHandlerProvider");
			_advertisementHandlerProvider.Register(new AdvertisementHandler(_advertisement));
			Debug.Log("register _pathNameConfigProvider");
			_pathNameConfigProvider.Register(_resourcePathConfigServiceFactory.Create());
			Debug.Log("register _gameStateChangerFactory");
			_gameStateChangerProvider.Register(_gameStateChangerFactory.Create());
		}
	}
}