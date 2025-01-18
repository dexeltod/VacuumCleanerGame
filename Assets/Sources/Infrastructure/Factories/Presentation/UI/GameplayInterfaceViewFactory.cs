using System;
using Sources.BusinessLogic;
using Sources.BusinessLogic.Repository;
using Sources.BusinessLogic.Services;
using Sources.Controllers;
using Sources.ControllersInterfaces;
using Sources.DomainInterfaces;
using Sources.Presentation.UI;
using Sources.PresentationInterfaces;
using Sources.Utils.Enums;

namespace Sources.Presentation.Factories.UI
{
	public class GameplayInterfaceViewFactory
	{
		private const bool IsGlobalScoreViewed = false;

		private readonly IGameMenuPresenter _gameMenuPresenterProvider;
		private readonly IGameplayInterfacePresenter _gameplayInterfacePresenter;
		private readonly GameplayInterfaceView _gameplayInterfaceView;
		private readonly IResourceModelReadOnly _resourceModelReadOnly;
		private readonly IPlayerModelRepository _playerModelRepository;
		private readonly IGameStateChanger _stateChangerProvider;
		private readonly TranslatorService _translatorService;

		public GameplayInterfaceViewFactory(
			IGameplayInterfacePresenter gameplayInterfacePresenterProvider,
			IGameMenuPresenter gameMenuPresenterProvider,
			IGameStateChanger stateChangerProvider,
			TranslatorService translatorService,
			IResourceModelReadOnly resourceModelReadOnly,
			GameplayInterfaceView gameplayInterfaceView,
			IPlayerModelRepository playerModelRepository
		)
		{
			_gameplayInterfacePresenter = gameplayInterfacePresenterProvider ??
			                              throw new ArgumentNullException(nameof(gameplayInterfacePresenterProvider));

			_gameMenuPresenterProvider =
				gameMenuPresenterProvider ?? throw new ArgumentNullException(nameof(gameMenuPresenterProvider));
			_stateChangerProvider = stateChangerProvider ?? throw new ArgumentNullException(nameof(stateChangerProvider));
			_translatorService = translatorService ?? throw new ArgumentNullException(nameof(translatorService));
			_resourceModelReadOnly = resourceModelReadOnly ?? throw new ArgumentNullException(nameof(resourceModelReadOnly));
			_playerModelRepository = playerModelRepository ?? throw new ArgumentNullException(nameof(playerModelRepository));
			_gameplayInterfaceView = gameplayInterfaceView
				? gameplayInterfaceView
				: throw new ArgumentNullException(nameof(gameplayInterfaceView));
		}

		public void Create()
		{
			if (_resourceModelReadOnly == null) throw new ArgumentNullException(nameof(_resourceModelReadOnly));

			var a = new GameMenuPresenter(
				_gameplayInterfaceView.GetComponent<IGameMenuView>(),
				_stateChangerProvider
			);

			bool isHalfScoreReached
				= _resourceModelReadOnly.CurrentTotalResources > _resourceModelReadOnly.CurrentTotalResources / 2;

			_gameplayInterfaceView.Construct(
				_gameplayInterfacePresenter,
				_resourceModelReadOnly.CurrentCashScore,
				_resourceModelReadOnly.CurrentTotalResources,
				(int)_playerModelRepository.Get(ProgressType.MaxCashScore).Value,
				_resourceModelReadOnly.MaxTotalResourceCount,
				_resourceModelReadOnly.SoftCurrency.Value,
				isHalfScoreReached,
				IsGlobalScoreViewed
			);

			_gameplayInterfaceView.PhrasesList.Phrases =
				_translatorService.GetLocalize(_gameplayInterfaceView.PhrasesList.Phrases);
		}
	}
}