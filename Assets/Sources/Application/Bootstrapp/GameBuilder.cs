using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Sources.Controllers;
using Sources.DomainInterfaces.DomainServicesInterfaces;
using Sources.Infrastructure.Factories;
using Sources.Infrastructure.Factories.Domain;
using Sources.Infrastructure.Providers;
using Sources.Infrastructure.Services;
using Sources.InfrastructureInterfaces.Factory;
using Sources.InfrastructureInterfaces.Providers;
using Sources.InfrastructureInterfaces.Services;
using Sources.InfrastructureInterfaces.States;
using Sources.Services;
using Sources.ServicesInterfaces;
using Sources.ServicesInterfaces.Advertisement;
using Sources.Utils;
using UnityEngine.Audio;
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
		private readonly GameFocusHandlerProvider _gameFocusHandlerProvider;
		private readonly IAssetFactory _assetFactory;

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
			IAdvertisement advertisement,
			GameFocusHandlerProvider gameFocusHandlerProvider,
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
			_saveLoaderProvider = saveLoaderProvider ?? throw new ArgumentNullException(nameof(saveLoaderProvider));
			_saveLoaderFactory = saveLoaderFactory ?? throw new ArgumentNullException(nameof(saveLoaderFactory));
			_advertisementHandlerProvider = advertisementHandlerProvider ??
				throw new ArgumentNullException(nameof(advertisementHandlerProvider));
			_advertisement = advertisement ?? throw new ArgumentNullException(nameof(advertisement));
			_gameFocusHandlerProvider = gameFocusHandlerProvider ??
				throw new ArgumentNullException(nameof(gameFocusHandlerProvider));
			_assetFactory = assetFactory ?? throw new ArgumentNullException(nameof(assetFactory));
		}

		private IGameStateChanger GameStateChanger => _gameStateChangerProvider.Self;

		public async UniTask StartAsync(CancellationToken cancellation)
		{
			await Initialize();

			DOTween.Init();

			GameStateChanger.Enter<IMenuState>();
		}

		private async UniTask Initialize()
		{
			ApplicationQuitHandler applicationQuitHandler
				= _assetFactory.InstantiateAndGetComponent<ApplicationQuitHandler>(
					ResourcesAssetPath.GameObjects
						.ApplicationQuitHandler
				);

			_gameFocusHandlerProvider.Register(
				new GameFocusHandler(
					_assetFactory.LoadFromResources<AudioMixer>(ResourcesAssetPath.GameObjects.AudioMixer),
					applicationQuitHandler
				)
			);

			_saveLoaderProvider.Register<ISaveLoader>(_saveLoaderFactory.GetSaveLoader());

			await _saveLoader.Initialize();
			await _progressFactory.Create();

			_advertisementHandlerProvider.Register(new AdvertisementPresenter(_advertisement));

			_pathNameConfigProvider.Register(_resourcePathConfigServiceFactory.Create());

			_gameStateChangerProvider.Register(_gameStateChangerFactory.Create());
		}
	}
}