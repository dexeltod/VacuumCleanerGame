#if YANDEX_GAMES && !UNITY_EDITOR
using Agava.YandexGames;
#endif

using System;
using Sources.BuisenessLogic.Interfaces;
using Sources.BuisenessLogic.Repository;
using Sources.BuisenessLogic.Services;
using Sources.BuisenessLogic.ServicesInterfaces;
using Sources.BuisenessLogic.ServicesInterfaces.Advertisement;
using Sources.Controllers;
using Sources.ControllersInterfaces;
using Sources.DomainInterfaces;
using Sources.DomainInterfaces.Entities;
using Sources.Infrastructure.Repository;
using Sources.Infrastructure.Services.Decorators;
using Sources.Presentation.UI;
using Sources.Utils;
using Sources.Utils.Enums;
using VContainer;

namespace Sources.Presentation.Factories.UI
{
	public class GameplayInterfacePresenterFactory : PresenterFactory<GameplayInterfacePresenter>
	{
		private const float Time = 10f;

		private readonly IAssetFactory _assetFactory;
		private readonly GameplayInterfaceViewFactory _gameplayInterfaceViewFactory;
		private readonly IResourcesProgressPresenter _resourceProgressProgressPresenter;
		private readonly IPersistentProgressService _persistentProgressService;
		private readonly ITranslatorService _translatorService;
		private readonly ILevelChangerService _levelChangerService;
		private readonly IGameplayInterfacePresenter _gameplayInterfacePresenter;
		private readonly IGameMenuPresenter _gameMenuPresenter;
		private readonly IGameStateChanger _gameStateChanger;
		private readonly ICoroutineRunner _coroutineRunner;
		private readonly IAdvertisement _advertisement;
		private readonly IPlayerModelRepository _playerModelRepository;

		[Inject]
		public GameplayInterfacePresenterFactory(
			IAssetFactory assetFactory,
			IPersistentProgressService persistentProgressService,
			ITranslatorService translatorService,
			ILevelChangerService levelChangerService,
			IGameplayInterfacePresenter gameplayInterfacePresenter,
			IGameMenuPresenter gameMenuPresenter,
			IGameStateChanger gameStateChanger,
			ICoroutineRunner coroutineRunner,
			IAdvertisement advertisement,
			IPlayerModelRepository playerModelRepository
		)

		{
			_assetFactory
				= assetFactory ?? throw new ArgumentNullException(nameof(assetFactory));
			_persistentProgressService = persistentProgressService ??
			                             throw new ArgumentNullException(nameof(persistentProgressService));
			_translatorService = translatorService ?? throw new ArgumentNullException(nameof(translatorService));
			_levelChangerService = levelChangerService ?? throw new ArgumentNullException(nameof(levelChangerService));
			_gameplayInterfacePresenter = gameplayInterfacePresenter ??
			                              throw new ArgumentNullException(nameof(gameplayInterfacePresenter));

			_gameMenuPresenter = gameMenuPresenter ??
			                     throw new ArgumentNullException(nameof(gameMenuPresenter));

			_gameStateChanger = gameStateChanger ?? throw new ArgumentNullException(nameof(gameStateChanger));
			_coroutineRunner = coroutineRunner ??
			                   throw new ArgumentNullException(nameof(coroutineRunner));
			_advertisement = advertisement ?? throw new ArgumentNullException(nameof(advertisement));
			_playerModelRepository = playerModelRepository ??
			                         throw new ArgumentNullException(nameof(playerModelRepository));
		}

		private string UIResourcesUI => ResourcesAssetPath.Scene.UIResources.UI;

		private IGameStateChanger GameStateChanger => _gameStateChanger;

		private IResourceModelReadOnly ResourceModelReadOnly =>
			_persistentProgressService.GlobalProgress
				.ResourceModelReadOnly;

		private int CashScore =>
			_persistentProgressService.GlobalProgress.ResourceModelReadOnly
				.CurrentCashScore;

		public override GameplayInterfacePresenter Create()
		{
			GameplayInterfaceView gameplayInterfaceView = Load();

			var stat = PlayerModelRepository.Get(ProgressType.Speed) as IStat;
			var maxCashScore = PlayerModelRepository.Get(ProgressType.MaxCashScore);

			var speedDecorator = new SpeedDecorator(
				_coroutineRunner,
				_advertisement,
				Time,
				stat
			);

			GameplayInterfacePresenter presenter = new GameplayInterfacePresenter(
				_levelChangerService,
				gameplayInterfaceView,
				speedDecorator,
				_coroutineRunner,
				Time,
				CashScore,
				maxCashScore
			);

			GameMenuView gameMenuView = gameplayInterfaceView.GetComponent<GameMenuView>();

			_gameMenuPresenter.Register(new GameMenuPresenter(gameMenuView, GameStateChanger));
			gameMenuView.Construct(_gameMenuPresenter);

			_gameplayInterfacePresenter.Register<IGameplayInterfacePresenter>(presenter);

			new GameplayInterfaceViewFactory(
				_gameplayInterfacePresenter,
				_gameMenuPresenter,
				_gameStateChanger,
				_translatorService,
				ResourceModelReadOnly,
				gameplayInterfaceView,
				PlayerModelRepository
			).Create();

			return presenter;
		}

		private GameplayInterfaceView Load() =>
			_assetFactory.Instantiate(UIResourcesUI).GetComponent<GameplayInterfaceView>();
	}
}