#if YANDEX_GAMES && !UNITY_EDITOR
using Agava.YandexGames;
#endif

using System;
using Sources.Controllers;
using Sources.ControllersInterfaces;
using Sources.DomainInterfaces;
using Sources.Infrastructure.Common.Factory.Decorators;
using Sources.Infrastructure.Providers;
using Sources.InfrastructureInterfaces.Factory;
using Sources.InfrastructureInterfaces.Providers;
using Sources.Presentation.UI;
using Sources.PresentationInterfaces;
using Sources.Services.Localization;
using Sources.ServicesInterfaces;
using Sources.Utils.Configs.Scripts;
using VContainer;

namespace Sources.Infrastructure.Factories.UI
{
	public class GameplayInterfacePresenterFactory : PresenterFactory<GameplayInterfacePresenter>
	{
		private const bool IsActiveOnStart = true;

		private readonly IAssetFactory _assetFactory;
		private readonly IResourcesProgressPresenterProvider _resourceProgressPresenterProvider;
		private readonly IPersistentProgressService _gameProgress;
		private readonly ITranslatorService _translatorService;
		private readonly ILevelChangerService _levelChangerService;
		private readonly GameplayInterfaceProvider _gameplayInterfaceProvider;
		private readonly IGameplayInterfacePresenterProvider _gameplayInterfacePresenterProvider;

		private readonly string _uiResourcesUI = ResourcesAssetPath.Scene.UIResources.UI;


		[Inject]
		public GameplayInterfacePresenterFactory(
			IAssetFactory assetFactory,
			IResourcesProgressPresenterProvider resourceProgressProgressPresenterProvider,
			IPersistentProgressService persistentProgressService,
			ITranslatorService translatorService,
			ILevelChangerService levelChangerService,
			GameplayInterfaceProvider gameplayInterfaceProvider,
			IGameplayInterfacePresenterProvider gameplayInterfacePresenterProvider
		)
		{
			_assetFactory = assetFactory ?? throw new ArgumentNullException(nameof(assetFactory));

			_resourceProgressPresenterProvider = resourceProgressProgressPresenterProvider ??
				throw new ArgumentNullException(nameof(resourceProgressProgressPresenterProvider));

			_gameProgress = persistentProgressService ??
				throw new ArgumentNullException(nameof(persistentProgressService));
			_translatorService = translatorService ?? throw new ArgumentNullException(nameof(translatorService));
			_levelChangerService = levelChangerService ?? throw new ArgumentNullException(nameof(levelChangerService));
			_gameplayInterfaceProvider = gameplayInterfaceProvider ??
				throw new ArgumentNullException(nameof(gameplayInterfaceProvider));
			_gameplayInterfacePresenterProvider = gameplayInterfacePresenterProvider ??
				throw new ArgumentNullException(nameof(gameplayInterfacePresenterProvider));
		}

		public override GameplayInterfacePresenter Create()
		{
			GameplayInterfaceView gameplayInterfaceView = Load();
			_gameplayInterfaceProvider.Register<IGameplayInterfaceView>(gameplayInterfaceView);

			_gameplayInterfacePresenterProvider.Register<IGameplayInterfacePresenter>(
				new GameplayInterfacePresenter(_levelChangerService, gameplayInterfaceView)
			);

			IResourcesModel model = GetModel();

			ConstructView(
				model,
				new GameplayInterfacePresenter(_levelChangerService, gameplayInterfaceView),
				gameplayInterfaceView
			);

			return new GameplayInterfacePresenter(_levelChangerService, gameplayInterfaceView);
		}

		private void ConstructView(
			IResourcesModel model,
			GameplayInterfacePresenter gameplayInterfacePresenter,
			GameplayInterfaceView gameplayInterfaceView
		)
		{
			if (model == null) throw new ArgumentNullException(nameof(model));

			gameplayInterfaceView.Construct(
				gameplayInterfacePresenter,
				model.CurrentCashScore,
				model.MaxCashScore,
				model.MaxGlobalScore,
				model.SoftCurrency.Count,
				IsActiveOnStart
			);

			gameplayInterfaceView.Phrases.Phrases = _translatorService.Localize(gameplayInterfaceView.Phrases.Phrases);
		}

		private IResourcesModel GetModel() =>
			_gameProgress
				.GameProgress
				.ResourcesModel;

		private GameplayInterfaceView Load() =>
			_assetFactory
				.Instantiate(_uiResourcesUI)
				.GetComponent<GameplayInterfaceView>();
	}
}