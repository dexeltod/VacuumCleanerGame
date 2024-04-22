#if YANDEX_GAMES && !UNITY_EDITOR
using Agava.YandexGames;
#endif

using System;
using Sources.Controllers;
using Sources.ControllersInterfaces;
using Sources.Domain.Temp;
using Sources.DomainInterfaces;
using Sources.Infrastructure.Common.Factory.Decorators;
using Sources.Infrastructure.Configs.Scripts;
using Sources.Infrastructure.Services.Decorators;
using Sources.InfrastructureInterfaces;
using Sources.InfrastructureInterfaces.Providers;
using Sources.InfrastructureInterfaces.Repository;
using Sources.InfrastructureInterfaces.Services;
using Sources.Presentation;
using Sources.Presentation.UI;
using Sources.Services.Localization;
using Sources.ServicesInterfaces;
using Sources.ServicesInterfaces.Advertisement;
using Sources.Utils;
using VContainer;

namespace Sources.Infrastructure.Factories.UI
{
	public class GameplayInterfacePresenterFactory : PresenterFactory<GameplayInterfacePresenter>
	{
		private const float Time = 10f;

		private readonly string _uiResourcesUI = ResourcesAssetPath.Scene.UIResources.UI;
		private readonly IAssetFactory _assetFactory;
		private readonly GameplayInterfaceViewFactory _gameplayInterfaceViewFactory;
		private readonly IResourcesProgressPresenterProvider _resourceProgressProgressPresenterProvider;
		private readonly IPersistentProgressServiceProvider _persistentProgressServiceProvider;
		private readonly ITranslatorService _translatorService;
		private readonly ILevelChangerService _levelChangerService;
		private readonly IGameplayInterfacePresenterProvider _gameplayInterfacePresenterProvider;
		private readonly IGameMenuPresenterProvider _gameMenuPresenterProvider;
		private readonly IGameStateChangerProvider _gameStateChangerProvider;
		private readonly ICoroutineRunnerProvider _coroutineRunnerProvider;
		private readonly IAdvertisement _advertisement;
		private readonly IModifiableStatsRepositoryProvider _modifiableStatsRepositoryProvider;

		[Inject]
		public GameplayInterfacePresenterFactory(
			IAssetFactory assetFactory,
			IPersistentProgressServiceProvider persistentProgressService,
			ITranslatorService translatorService,
			ILevelChangerService levelChangerService,
			IGameplayInterfacePresenterProvider gameplayInterfacePresenterProvider,
			IGameMenuPresenterProvider gameMenuPresenterProvider,
			IGameStateChangerProvider gameStateChanger,
			ICoroutineRunnerProvider coroutineRunnerProvider,
			IAdvertisement advertisement,
			ILevelProgressFacade levelProgressFacade,
			IProgressService progressService,
			IModifiableStatsRepositoryProvider modifiableStatsRepositoryProvider
		)

		{
			_assetFactory = assetFactory ?? throw new ArgumentNullException(nameof(assetFactory));
			_persistentProgressServiceProvider = persistentProgressService ??
				throw new ArgumentNullException(nameof(persistentProgressService));
			_translatorService = translatorService ?? throw new ArgumentNullException(nameof(translatorService));
			_levelChangerService = levelChangerService ?? throw new ArgumentNullException(nameof(levelChangerService));
			_gameplayInterfacePresenterProvider = gameplayInterfacePresenterProvider ??
				throw new ArgumentNullException(nameof(gameplayInterfacePresenterProvider));

			_gameMenuPresenterProvider = gameMenuPresenterProvider ??
				throw new ArgumentNullException(nameof(gameMenuPresenterProvider));

			_gameStateChangerProvider = gameStateChanger ?? throw new ArgumentNullException(nameof(gameStateChanger));
			_coroutineRunnerProvider = coroutineRunnerProvider ??
				throw new ArgumentNullException(nameof(coroutineRunnerProvider));
			_advertisement = advertisement ?? throw new ArgumentNullException(nameof(advertisement));
			_modifiableStatsRepositoryProvider = modifiableStatsRepositoryProvider ??
				throw new ArgumentNullException(nameof(modifiableStatsRepositoryProvider));
		}

		private IGameStateChanger GameStateChanger => _gameStateChangerProvider.Implementation;

		private IResourceModelReadOnly ResourceModelReadOnly =>
			_persistentProgressServiceProvider.Implementation.GlobalProgress
				.ResourceModelReadOnly;

		private int CashScore =>
			_persistentProgressServiceProvider.Implementation.GlobalProgress.ResourceModelReadOnly
				.CurrentCashScore;

		private IModifiableStatsRepository ModifiableStatsRepository =>
			_modifiableStatsRepositoryProvider.Implementation;

		private int MaxCashScore =>
			_persistentProgressServiceProvider.Implementation.GlobalProgress.ResourceModelReadOnly
				.MaxCashScore;

		public override GameplayInterfacePresenter Create()
		{
			GameplayInterfaceView gameplayInterfaceView = Load();

			var speedDecorator = new SpeedDecorator(
				_coroutineRunnerProvider,
				_advertisement,
				Time,
				ModifiableStatsRepository.Get((int)ProgressType.Speed)
			);

			GameplayInterfacePresenter presenter = new GameplayInterfacePresenter(
				_levelChangerService,
				gameplayInterfaceView,
				speedDecorator,
				_coroutineRunnerProvider,
				Time,
				CashScore,
				MaxCashScore
			);

			GameMenuView gameMenuView = gameplayInterfaceView.GetComponent<GameMenuView>();

			_gameMenuPresenterProvider.Register(new GameMenuPresenter(gameMenuView, GameStateChanger));
			gameMenuView.Construct(_gameMenuPresenterProvider.Implementation);

			_gameplayInterfacePresenterProvider.Register<IGameplayInterfacePresenter>(presenter);

			new GameplayInterfaceViewFactory(
				_gameplayInterfacePresenterProvider,
				_gameMenuPresenterProvider,
				_gameStateChangerProvider,
				_translatorService,
				ResourceModelReadOnly,
				gameplayInterfaceView
			).Create();

			return presenter;
		}

		private GameplayInterfaceView Load() =>
			_assetFactory.Instantiate(_uiResourcesUI).GetComponent<GameplayInterfaceView>();
	}
}