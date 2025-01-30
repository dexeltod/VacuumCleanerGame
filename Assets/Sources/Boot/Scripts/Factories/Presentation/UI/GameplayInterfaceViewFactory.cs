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

namespace Sources.Boot.Scripts.Factories.Presentation.UI
{
	public class GameplayInterfaceViewFactory
	{
		private const bool IsGlobalScoreViewed = true;

		private readonly GameplayInterfaceView _gameplayInterfaceView;
		private readonly IPlayerModelRepository _playerModelRepository;
		private readonly IGameplayInterfacePresenter _presenter;
		private readonly IResourceModelReadOnly _resourceModelReadOnly;
		private readonly IStateMachine _stateMachineProvider;
		private readonly TranslatorService _translatorService;

		public GameplayInterfaceViewFactory(
			IGameplayInterfacePresenter presenter,
			IStateMachine stateMachineProvider,
			TranslatorService translatorService,
			IResourceModelReadOnly resourceModelReadOnly,
			GameplayInterfaceView gameplayInterfaceView,
			IPlayerModelRepository playerModelRepository)
		{
			_presenter = presenter ?? throw new ArgumentNullException(nameof(presenter));
			_stateMachineProvider = stateMachineProvider ?? throw new ArgumentNullException(nameof(stateMachineProvider));
			_translatorService = translatorService ?? throw new ArgumentNullException(nameof(translatorService));
			_resourceModelReadOnly = resourceModelReadOnly ?? throw new ArgumentNullException(nameof(resourceModelReadOnly));
			_playerModelRepository = playerModelRepository ?? throw new ArgumentNullException(nameof(playerModelRepository));
			_gameplayInterfaceView = gameplayInterfaceView
				? gameplayInterfaceView
				: throw new ArgumentNullException(nameof(gameplayInterfaceView));
		}

		public IGameMenuPresenter Create()
		{
			if (_resourceModelReadOnly == null) throw new ArgumentNullException(nameof(_resourceModelReadOnly));

			IGameMenuPresenter presenter = new GameMenuPresenter(
				_gameplayInterfaceView.GetComponent<IGameMenuView>(),
				_stateMachineProvider
			);

			bool isHalfScoreReached
				= _resourceModelReadOnly.CurrentTotalResources > _resourceModelReadOnly.CurrentTotalResources / 2;

			_gameplayInterfaceView.Construct(
				_presenter,
				_resourceModelReadOnly.CashScore.ReadOnlyValue,
				_resourceModelReadOnly.TotalAmount.ReadOnlyValue,
				(int)_playerModelRepository.Get(ProgressType.MaxCashScore).Value,
				_resourceModelReadOnly.MaxTotalResourceCount,
				_resourceModelReadOnly.SoftCurrency.ReadOnlyValue,
				isHalfScoreReached,
				IsGlobalScoreViewed
			);

			_gameplayInterfaceView.PhrasesList.Phrases =
				_translatorService.GetLocalize(_gameplayInterfaceView.PhrasesList.Phrases);

			return presenter;
		}
	}
}
