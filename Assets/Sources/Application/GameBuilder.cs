using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Sources.Application.Services;
using Sources.ApplicationServicesInterfaces;
using Sources.DomainInterfaces;
using Sources.DomainInterfaces.DomainServicesInterfaces;
using Sources.Infrastructure.Factories;
using Sources.Infrastructure.Factories.Player;
using Sources.Infrastructure.Providers;
using Sources.Infrastructure.StateMachine.GameStates;
using Sources.InfrastructureInterfaces.Factory;
using Sources.InfrastructureInterfaces.Providers;
using Sources.ServicesInterfaces;
using VContainer;
using VContainer.Unity;

namespace Sources.Application
{
	public class GameBuilder : IAsyncStartable
	{
		private readonly IGameStateChangerProvider _gameStateChanger;
		private readonly ProgressFactory _progressFactory;
		private readonly ISaveLoader _saveLoader;
		private readonly IGameStateChangerProvider _gameStateChangerProvider;
		private readonly IGameStateChangerFactory _gameStateChangerFactory;
		private readonly ResourcePathConfigServiceFactory _resourcePathConfigServiceFactory;
		private readonly ResourcePathConfigProvider _pathConfigProvider;
		private readonly PlayerStatsFactory _playerStatsFactory;
		private readonly IPersistentProgressServiceProvider _persistentProgressServiceProvider;
		private readonly IYandexSDKController _yandexSDKController;

		[Inject]
		public GameBuilder(
			IGameStateChangerProvider gameStateChanger,
			ProgressFactory progressFactory,
			ISaveLoader saveLoader,
			IGameStateChangerProvider gameStateChangerProvider,
			IGameStateChangerFactory gameStateChangerFactory,
			ResourcePathConfigServiceFactory resourcePathConfigServiceFactory,
			ResourcePathConfigProvider pathConfigProvider,
			PlayerStatsFactory playerStatsFactory,
			IPlayerStatsServiceProvider playerStatsServiceProvider,
			IPersistentProgressServiceProvider persistentProgressService

#if YANDEX_CODE
			, IYandexSDKController yandexSDKController
#endif
		)
		{
			_gameStateChanger = gameStateChanger ?? throw new ArgumentNullException(nameof(gameStateChanger));
			_progressFactory = progressFactory ?? throw new ArgumentNullException(nameof(progressFactory));
			_saveLoader = saveLoader ?? throw new ArgumentNullException(nameof(saveLoader));
			_gameStateChangerProvider = gameStateChangerProvider ??
				throw new ArgumentNullException(nameof(gameStateChangerProvider));
			_gameStateChangerFactory = gameStateChangerFactory ??
				throw new ArgumentNullException(nameof(gameStateChangerFactory));
			_resourcePathConfigServiceFactory = resourcePathConfigServiceFactory ??
				throw new ArgumentNullException(nameof(resourcePathConfigServiceFactory));
			_pathConfigProvider = pathConfigProvider ?? throw new ArgumentNullException(nameof(pathConfigProvider));
			_playerStatsFactory = playerStatsFactory ?? throw new ArgumentNullException(nameof(playerStatsFactory));
			_persistentProgressServiceProvider = persistentProgressService ??
				throw new ArgumentNullException(nameof(persistentProgressService));

#if YANDEX_CODE
			_yandexSDKController = yandexSDKController ?? throw new ArgumentNullException(nameof(yandexSDKController));
#endif
		}

		private async UniTask Initialize()
		{
			await _saveLoader.Initialize();
			await _progressFactory.Initialize();
		}

		public async UniTask StartAsync(CancellationToken cancellation)
		{
			await Initialize();
#if YANDEX_CODE
			_yandexSDKController.SetStatusInitialized();
#endif

			_playerStatsFactory.CreatePlayerStats(_persistentProgressServiceProvider);

			_pathConfigProvider.Register(_resourcePathConfigServiceFactory.Create());
			_gameStateChangerProvider.Register(_gameStateChangerFactory.Create());

			_gameStateChanger.Implementation.Enter<MenuState>();
		}
	}
}