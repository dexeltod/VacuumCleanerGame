#if YANDEX_GAMES && !UNITY_EDITOR
using Agava.YandexGames;
#endif

using System;
using Sources.Controllers;
using Sources.ControllersInterfaces;
using Sources.DomainInterfaces;
using Sources.Infrastructure.Common.Factory.Decorators;
using Sources.Infrastructure.Providers;
using Sources.InfrastructureInterfaces.Providers;
using Sources.Presentation.UI;
using Sources.PresentationInterfaces;
using Sources.Services.Localization;
using Sources.ServicesInterfaces;
using Sources.Utils.Configs.Scripts;
using VContainer;

namespace Sources.Infrastructure.Factories.UI
{
	public class GameplayInterfacePresenterFactory : PresenterFactory<IGameplayInterfacePresenter>
	{
		private const bool IsActiveOnStart = true;

		private readonly IAssetFactory _assetFactory;
		private readonly IPersistentProgressServiceProvider _gameProgress;
		private readonly ITranslatorService _translatorService;
		private readonly ILevelChangerService _levelChangerService;
		private readonly GameplayInterfaceProvider _gameplayInterfaceProvider;
		private readonly IGameplayInterfacePresenterProvider _gameplayInterfacePresenterProvider;
		private readonly IGameMenuPresenterProvider _gameMenuPresenterProvider;
		private readonly IGameStateChangerProvider _gameStateChanger;

		private readonly string _uiResourcesUI = ResourcesAssetPath.Scene.UIResources.UI;

		private IGameplayInterfacePresenter GameplayInterfacePresenter =>
			_gameplayInterfacePresenterProvider.Implementation;

		private IResourcesModel GlobalProgressResourcesModel =>
			_gameProgress.Implementation.GlobalProgress.ResourcesModel;

		[Inject]
		public GameplayInterfacePresenterFactory(
			IAssetFactory assetFactory,
			IResourcesProgressPresenterProvider resourceProgressProgressPresenterProvider,
			IPersistentProgressServiceProvider persistentProgressService,
			ITranslatorService translatorService,
			ILevelChangerService levelChangerService,
			GameplayInterfaceProvider gameplayInterfaceProvider,
			IGameplayInterfacePresenterProvider gameplayInterfacePresenterProvider,
			IGameMenuPresenterProvider gameMenuPresenterProvider,
			IGameStateChangerProvider gameStateChanger
		)
		{
			_assetFactory = assetFactory ?? throw new ArgumentNullException(nameof(assetFactory));

			_gameProgress = persistentProgressService ??
				throw new ArgumentNullException(nameof(persistentProgressService));
			_translatorService = translatorService ?? throw new ArgumentNullException(nameof(translatorService));
			_levelChangerService = levelChangerService ?? throw new ArgumentNullException(nameof(levelChangerService));
			_gameplayInterfaceProvider = gameplayInterfaceProvider ??
				throw new ArgumentNullException(nameof(gameplayInterfaceProvider));
			_gameplayInterfacePresenterProvider = gameplayInterfacePresenterProvider ??
				throw new ArgumentNullException(nameof(gameplayInterfacePresenterProvider));
			_gameMenuPresenterProvider = gameMenuPresenterProvider ??
				throw new ArgumentNullException(nameof(gameMenuPresenterProvider));
			_gameStateChanger = gameStateChanger ?? throw new ArgumentNullException(nameof(gameStateChanger));
		}

		public override IGameplayInterfacePresenter Create()
		{
			GameplayInterfaceView gameplayInterfaceView = Load();
			_gameplayInterfaceProvider.Register<IGameplayInterfaceView>(gameplayInterfaceView);

			GameplayInterfacePresenter presenter = new GameplayInterfacePresenter(
				_levelChangerService,
				_gameplayInterfaceProvider.Implementation
			);

			_gameplayInterfacePresenterProvider.Register<IGameplayInterfacePresenter>(presenter);

			ConstructView(
				GlobalProgressResourcesModel,
				GameplayInterfacePresenter,
				gameplayInterfaceView
			);

			return GameplayInterfacePresenter;
		}

		private void ConstructView(
			IResourcesModel model,
			IGameplayInterfacePresenter gameplayInterfacePresenter,
			GameplayInterfaceView gameplayInterfaceView
		)
		{
			if (model == null) throw new ArgumentNullException(nameof(model));

			var gameMenuView = gameplayInterfaceView.GetComponent<IGameMenuView>();

			_gameMenuPresenterProvider.Register<IGameMenuPresenter>(
				new GameMenuPresenter(gameMenuView, _gameStateChanger.Implementation)
			);

			gameMenuView.Construct(_gameMenuPresenterProvider.Implementation);

			gameplayInterfaceView.Construct(
				gameplayInterfacePresenter,
				model.CurrentCashScore,
				model.MaxCashScore,
				model.MaxGlobalScore,
				model.SoftCurrency.Count,
				IsActiveOnStart,
				_gameMenuPresenterProvider.Implementation
			);

			gameplayInterfaceView.Phrases.Phrases = _translatorService.Localize(gameplayInterfaceView.Phrases.Phrases);
		}

		private GameplayInterfaceView Load() =>
			_assetFactory
				.Instantiate(_uiResourcesUI)
				.GetComponent<GameplayInterfaceView>();
	}
}