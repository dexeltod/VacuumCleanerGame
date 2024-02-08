#if YANDEX_GAMES && !UNITY_EDITOR
using Agava.YandexGames;
#endif

using System;
using Sources.Controllers;
using Sources.ControllersInterfaces;
using Sources.DomainInterfaces;
using Sources.Infrastructure.Common.Factory.Decorators;
using Sources.InfrastructureInterfaces.Factory;
using Sources.Services.Localization;
using Sources.ServicesInterfaces;
using Sources.Utils.Configs.Scripts;
using VContainer;

namespace Sources.Infrastructure.Factories.UI
{
	public class UIFactory : PresentableFactory<IGameplayInterfaceView, IGameplayInterfacePresenter>, IUIFactory
	{
		private const bool IsActiveOnStart = true;

		private readonly IAssetFactory _assetFactory;
		private readonly IResourceProgressEventHandler _resourceProgressEventHandler;
		private readonly IPersistentProgressService _gameProgress;
		private readonly ITranslatorService _translatorService;
		private readonly ILevelChangerService _levelChangerService;

		private readonly string _uiResourcesUI = ResourcesAssetPath.Scene.UIResources.UI;

		[Inject]
		public UIFactory(
			IAssetFactory assetFactory,
			IResourceProgressEventHandler resourceProgressEventHandler,
			IPersistentProgressService persistentProgressService,
			ITranslatorService translatorService,
			ILevelChangerService levelChangerService
		)
		{
			_assetFactory = assetFactory ?? throw new ArgumentNullException(nameof(assetFactory));

			_resourceProgressEventHandler = resourceProgressEventHandler ??
				throw new ArgumentNullException(nameof(resourceProgressEventHandler));

			_gameProgress = persistentProgressService ??
				throw new ArgumentNullException(nameof(persistentProgressService));
			_translatorService = translatorService ?? throw new ArgumentNullException(nameof(translatorService));
			_levelChangerService = levelChangerService ?? throw new ArgumentNullException(nameof(levelChangerService));
		}

		public override IGameplayInterfaceView Create()
		{
			IGameplayInterfaceView gameplayInterfaceView = Load();
			IResourcesModel model = GetModel();
			Construct(model, gameplayInterfaceView);

			return gameplayInterfaceView;
		}

		private void Construct(IResourcesModel model, IGameplayInterfaceView gameplayInterfaceView)
		{
			if (model == null) throw new ArgumentNullException(nameof(model));

			IGameplayInterfacePresenter gameplayInterfacePresenter
				= new GameplayInterfacePresenter(_levelChangerService);

			gameplayInterfaceView.Construct(
				gameplayInterfacePresenter,
				model.CurrentCashScore,
				model.MaxCashScore,
				model.MaxGlobalScore,
				model.SoftCurrency.Count,
				_resourceProgressEventHandler,
				IsActiveOnStart
			);

			gameplayInterfaceView.Phrases.Phrases = _translatorService.Localize(gameplayInterfaceView.Phrases.Phrases);
		}

		private IResourcesModel GetModel() =>
			_gameProgress
				.GameProgress
				.ResourcesModel;

		private IGameplayInterfaceView Load() =>
			_assetFactory
				.Instantiate(_uiResourcesUI)
				.GetComponent<IGameplayInterfaceView>();
	}
}