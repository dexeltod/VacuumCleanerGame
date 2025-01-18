#if YANDEX_GAMES && !UNITY_EDITOR
using Agava.YandexGames;
#endif

using System;
using Sources.BusinessLogic;
using Sources.BusinessLogic.Interfaces;
using Sources.BusinessLogic.Repository;
using Sources.BusinessLogic.Services;
using Sources.BusinessLogic.ServicesInterfaces;
using Sources.BusinessLogic.ServicesInterfaces.Advertisement;
using Sources.Controllers;
using Sources.ControllersInterfaces;
using Sources.DomainInterfaces;
using Sources.DomainInterfaces.Entities;
using Sources.Infrastructure.Services.Decorators;
using Sources.InfrastructureInterfaces.Factories.Presentations;
using Sources.Presentation;
using Sources.Presentation.UI;
using Sources.Utils;
using Sources.Utils.Enums;
using VContainer;

namespace Sources.Infrastructure.Factories.Presentation.UI
{
	public class GameplayInterfacePresenterFactory : PresenterFactory<IGameplayInterfacePresenter>,
		IGameplayInterfacePresenterFactory
	{
		private const float Time = 10f;

		private readonly IAssetFactory _assetFactory;
		private readonly GameplayInterfaceViewFactory _gameplayInterfaceViewFactory;
		private readonly IResourcesProgressPresenter _resourceProgressProgressPresenter;
		private readonly IPersistentProgressService _persistentProgressService;
		private readonly TranslatorService _translatorService;
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
			TranslatorService translatorService,
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
			_persistentProgressService.GlobalProgress.ResourceModelReadOnly;

		private int CashScore =>
			_persistentProgressService.GlobalProgress.ResourceModelReadOnly.CurrentCashScore;

		public override IGameplayInterfacePresenter Create()
		{
			GameplayInterfaceView gameplayInterfaceView = Load();

			var stat = _playerModelRepository.Get(ProgressType.Speed) as IStat;
			var maxCashScore = _playerModelRepository.Get(ProgressType.MaxCashScore);

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

			gameMenuView.Construct(_gameMenuPresenter);

			new GameplayInterfaceViewFactory(
				_gameplayInterfacePresenter,
				_gameMenuPresenter,
				_gameStateChanger,
				_translatorService,
				ResourceModelReadOnly,
				gameplayInterfaceView,
				_playerModelRepository
			).Create();

			return presenter;
		}

		private GameplayInterfaceView Load() =>
			_assetFactory.Instantiate(UIResourcesUI).GetComponent<GameplayInterfaceView>();
	}
}