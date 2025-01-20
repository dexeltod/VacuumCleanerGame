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
using Sources.Presentation;
using Sources.Presentation.UI;
using Sources.Utils;
using Sources.Utils.Enums;

namespace Sources.Boot.Scripts.Factories.Presentation.UI
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
		private readonly IGameStateChanger _gameStateChanger;
		private readonly ICoroutineRunner _coroutineRunner;
		private readonly IAdvertisement _advertisement;
		private readonly IPlayerModelRepository _playerModelRepository;

		public GameplayInterfacePresenterFactory(TranslatorService translatorService,
			IAssetFactory assetFactory,
			IPersistentProgressService persistentProgressService,
			ILevelChangerService levelChangerService,
			IGameStateChanger gameStateChanger,
			ICoroutineRunner coroutineRunner,
			IAdvertisement advertisement,
			IPlayerModelRepository playerModelRepository)

		{
			_assetFactory = assetFactory ?? throw new ArgumentNullException(nameof(assetFactory));
			_persistentProgressService =
				persistentProgressService ?? throw new ArgumentNullException(nameof(persistentProgressService));

			_translatorService = translatorService ?? throw new ArgumentNullException(nameof(translatorService));
			_levelChangerService = levelChangerService ?? throw new ArgumentNullException(nameof(levelChangerService));

			_gameStateChanger = gameStateChanger ?? throw new ArgumentNullException(nameof(gameStateChanger));
			_coroutineRunner = coroutineRunner ?? throw new ArgumentNullException(nameof(coroutineRunner));
			_advertisement = advertisement ?? throw new ArgumentNullException(nameof(advertisement));
			_playerModelRepository = playerModelRepository ?? throw new ArgumentNullException(nameof(playerModelRepository));
		}

		private string UIResourcesUI => ResourcesAssetPath.Scene.UIResources.UI;

		private IResourceModelReadOnly ResourceModelReadOnly =>
			_persistentProgressService.GlobalProgress.ResourceModelReadOnly;

		private int CashScore =>
			_persistentProgressService.GlobalProgress.ResourceModelReadOnly.CurrentCashScore;

		public override IGameplayInterfacePresenter Create()
		{
			GameplayInterfaceView gameplayInterfaceView =
				_assetFactory.Instantiate(UIResourcesUI).GetComponent<GameplayInterfaceView>();

			GameplayInterfacePresenter presenter = CreateGameplayInterfacePresenter(gameplayInterfaceView, CreateSpeedDecorator());

			GameMenuView gameMenuView = gameplayInterfaceView.GetComponent<GameMenuView>();

			IGameMenuPresenter gameMenuPresenter = CreateGameMenuPresenter(gameplayInterfaceView);
			gameMenuView.Construct(gameMenuPresenter);

			return presenter;
		}

		private IGameMenuPresenter CreateGameMenuPresenter(GameplayInterfaceView gameplayInterfaceView) =>
			new GameplayInterfaceViewFactory(
				_gameplayInterfacePresenter,
				_gameStateChanger,
				_translatorService,
				ResourceModelReadOnly,
				gameplayInterfaceView,
				_playerModelRepository
			).Create();

		private GameplayInterfacePresenter CreateGameplayInterfacePresenter(GameplayInterfaceView gameplayInterfaceView,
			SpeedDecorator speedDecorator) =>
			new(
				_levelChangerService,
				gameplayInterfaceView,
				speedDecorator,
				_coroutineRunner,
				Time,
				CashScore,
				_playerModelRepository.Get(ProgressType.MaxCashScore)
			);

		private SpeedDecorator CreateSpeedDecorator() =>
			new(
				_coroutineRunner,
				_advertisement,
				Time,
				_playerModelRepository.Get(ProgressType.Speed) as IStat
			);
	}
}
