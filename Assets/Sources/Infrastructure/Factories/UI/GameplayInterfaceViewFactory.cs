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
		private readonly IGameplayInterfacePresenterProvider _gameplayInterfacePresenterProvider;
		private readonly IGameMenuPresenterProvider _gameMenuPresenterProvider;
		private readonly IGameStateChangerProvider _stateChangerProvider;
		private readonly ITranslatorService _translatorService;
		private readonly IResourcesModel _resourcesModel;
		private readonly GameplayInterfaceView _gameplayInterfaceView;

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
			_gameplayInterfaceView = gameplayInterfaceView ? gameplayInterfaceView : throw new ArgumentNullException(nameof(gameplayInterfaceView));
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
				= _resourcesModel.GlobalScore.Count > _resourcesModel.GlobalScore.Count / 2;

			_gameplayInterfaceView.Construct(
				_gameplayInterfacePresenterProvider.Implementation,
				_resourcesModel.CurrentCashScore,
				_resourcesModel.CurrentGlobalScore,
				_resourcesModel.MaxCashScore,
				_resourcesModel.MaxGlobalScore,
				_resourcesModel.SoftCurrency.Count,
				isHalfScoreReached
			);

			_gameplayInterfaceView.Phrases.Phrases
				= _translatorService.Localize(_gameplayInterfaceView.Phrases.Phrases);
		}
	}
}