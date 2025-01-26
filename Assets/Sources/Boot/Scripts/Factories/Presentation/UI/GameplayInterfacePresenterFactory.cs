#if YANDEX_GAMES && !UNITY_EDITOR
using Agava.YandexGames;
#endif

using System;
using Sources.BusinessLogic;
using Sources.BusinessLogic.Interfaces;
using Sources.BusinessLogic.Repository;
using Sources.BusinessLogic.Services;
using Sources.BusinessLogic.ServicesInterfaces.Advertisement;
using Sources.Controllers;
using Sources.ControllersInterfaces;
using Sources.DomainInterfaces;
using Sources.DomainInterfaces.DomainServicesInterfaces;
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
		private const float SpeedCooldown = 10f;
		private readonly IAdvertisement _advertisement;
		private readonly ICoroutineRunner _coroutineRunner;

		private readonly GameplayInterfaceView _gameplayInterfaceView;
		private readonly GameplayInterfaceViewFactory _gameplayInterfaceViewFactory;
		private readonly ILevelChangerService _levelChangerService;
		private readonly IPersistentProgressService _persistentProgressService;
		private readonly IPlayerModelRepository _playerModelRepository;

		private readonly IResourcesProgressPresenter _resourceProgressProgressPresenter;
		private readonly IStateMachine _stateMachine;
		private readonly TranslatorService _translatorService;

		public GameplayInterfacePresenterFactory(
			GameplayInterfaceView gameplayInterfaceView,
			TranslatorService translatorService,
			IPersistentProgressService persistentProgressService,
			ILevelChangerService levelChangerService,
			IStateMachine stateMachine,
			ICoroutineRunner coroutineRunner,
			IAdvertisement advertisement,
			IPlayerModelRepository playerModelRepository)

		{
			_gameplayInterfaceView = gameplayInterfaceView ?? throw new ArgumentNullException(nameof(gameplayInterfaceView));
			_persistentProgressService =
				persistentProgressService ?? throw new ArgumentNullException(nameof(persistentProgressService));

			_translatorService = translatorService ?? throw new ArgumentNullException(nameof(translatorService));
			_levelChangerService = levelChangerService ?? throw new ArgumentNullException(nameof(levelChangerService));

			_stateMachine = stateMachine ?? throw new ArgumentNullException(nameof(stateMachine));
			_coroutineRunner = coroutineRunner ?? throw new ArgumentNullException(nameof(coroutineRunner));
			_advertisement = advertisement ?? throw new ArgumentNullException(nameof(advertisement));
			_playerModelRepository = playerModelRepository ?? throw new ArgumentNullException(nameof(playerModelRepository));
		}

		private string UIResourcesUI => ResourcesAssetPath.Scene.UIResources.UI;

		private IResourceModelReadOnly ResourceModelReadOnly => _persistentProgressService.GlobalProgress.ResourceModel;

		private IReadOnlyProgress<int> CashScore => _persistentProgressService.GlobalProgress.ResourceModel.CashScore;

		public override IGameplayInterfacePresenter Create()
		{
			IGameplayInterfacePresenter presenter = CreateGameplayInterfacePresenter(
				_gameplayInterfaceView,
				CreateSpeedDecorator()
			);

			_gameplayInterfaceView
				.GetComponent<GameMenuView>()
				.Construct(CreateGameMenuPresenter(_gameplayInterfaceView, presenter));

			return presenter;
		}

		private IGameMenuPresenter CreateGameMenuPresenter(
			GameplayInterfaceView gameplayInterfaceView,
			IGameplayInterfacePresenter presenter) =>
			new GameplayInterfaceViewFactory(
				presenter,
				_stateMachine,
				_translatorService,
				ResourceModelReadOnly,
				gameplayInterfaceView,
				_playerModelRepository
			).Create();

		private GameplayInterfacePresenter CreateGameplayInterfacePresenter(
			GameplayInterfaceView gameplayInterfaceView,
			SpeedDecorator speedDecorator) =>
			new(
				gameplayInterfaceView,
				speedDecorator,
				_levelChangerService,
				SpeedCooldown,
				CashScore,
				_coroutineRunner,
				_playerModelRepository.Get(ProgressType.MaxCashScore),
				ResourceModelReadOnly.TotalAmount,
				ResourceModelReadOnly.SoftCurrency
			);

		private SpeedDecorator CreateSpeedDecorator() =>
			new(
				_coroutineRunner,
				_advertisement,
				SpeedCooldown,
				_playerModelRepository.Get(ProgressType.Speed) as IStat
			);
	}
}