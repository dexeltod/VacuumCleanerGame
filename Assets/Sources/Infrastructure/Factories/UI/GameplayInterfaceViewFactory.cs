using System;
using Sources.Controllers;
using Sources.ControllersInterfaces;
using Sources.DomainInterfaces;
using Sources.InfrastructureInterfaces.Providers;
using Sources.Presentation.UI;
using Sources.PresentationInterfaces;
using Sources.Services.Localization;

namespace Sources.Infrastructure.Factories.UI
{
	public class GameplayInterfaceViewFactory
	{
		private const bool IsGlobalScoreViewed = false;

		private readonly IGameMenuPresenterProvider _gameMenuPresenterProvider;
		private readonly IGameplayInterfacePresenterProvider _gameplayInterfacePresenterProvider;
		private readonly GameplayInterfaceView _gameplayInterfaceView;
		private readonly IResourcesModel _resourcesModel;
		private readonly IGameStateChangerProvider _stateChangerProvider;
		private readonly ITranslatorService _translatorService;

		public GameplayInterfaceViewFactory(
			IGameplayInterfacePresenterProvider gameplayInterfacePresenterProvider,
			IGameMenuPresenterProvider gameMenuPresenterProvider,
			IGameStateChangerProvider stateChangerProvider,
			ITranslatorService translatorService,
			IResourcesModel resourcesModel,
			GameplayInterfaceView gameplayInterfaceView
		)
		{
			_gameplayInterfacePresenterProvider = gameplayInterfacePresenterProvider ??
				throw new ArgumentNullException(nameof(gameplayInterfacePresenterProvider));
			_gameMenuPresenterProvider = gameMenuPresenterProvider ??
				throw new ArgumentNullException(nameof(gameMenuPresenterProvider));
			_stateChangerProvider
				= stateChangerProvider ?? throw new ArgumentNullException(nameof(stateChangerProvider));
			_translatorService = translatorService ?? throw new ArgumentNullException(nameof(translatorService));
			_resourcesModel = resourcesModel ?? throw new ArgumentNullException(nameof(resourcesModel));
			_gameplayInterfaceView = gameplayInterfaceView
				? gameplayInterfaceView
				: throw new ArgumentNullException(nameof(gameplayInterfaceView));
		}

		public void Create()
		{
			if (_resourcesModel == null) throw new ArgumentNullException(nameof(_resourcesModel));

			_gameMenuPresenterProvider.Register<IGameMenuPresenter>(
				new GameMenuPresenter(
					_gameplayInterfaceView.GetComponent<IGameMenuView>(),
					_stateChangerProvider
						.Implementation
				)
			);

			bool isHalfScoreReached
				= _resourcesModel.CurrentTotalResources > _resourcesModel.CurrentTotalResources / 2;

			_gameplayInterfaceView.Construct(
				_gameplayInterfacePresenterProvider.Implementation,
				_resourcesModel.CurrentCashScore,
				_resourcesModel.CurrentTotalResources,
				_resourcesModel.MaxCashScore,
				_resourcesModel.MaxGlobalScore,
				_resourcesModel.SoftCurrency.Count,
				isHalfScoreReached,
				IsGlobalScoreViewed
			);

			_gameplayInterfaceView.Phrases.Phrases
				= _translatorService.GetLocalize(_gameplayInterfaceView.Phrases.Phrases);
		}
	}
}