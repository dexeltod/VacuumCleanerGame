using System;
using Sources.Controllers;
using Sources.ControllersInterfaces;
using Sources.DomainInterfaces;
using Sources.InfrastructureInterfaces.Providers;
using Sources.InfrastructureInterfaces.Repository;
using Sources.Presentation.UI;
using Sources.PresentationInterfaces;
using Sources.Services.Localization;
using Sources.Utils;
using Sources.Utils.Enums;

namespace Sources.Infrastructure.Factories.UI
{
	public class GameplayInterfaceViewFactory
	{
		private const bool IsGlobalScoreViewed = false;

		private readonly IGameMenuPresenterProvider _gameMenuPresenterProvider;
		private readonly IGameplayInterfacePresenterProvider _gameplayInterfacePresenterProvider;
		private readonly GameplayInterfaceView _gameplayInterfaceView;
		private readonly IResourceModelReadOnly _resourceModelReadOnly;
		private readonly IPlayerModelRepository _playerModelRepository;
		private readonly IGameStateChangerProvider _stateChangerProvider;
		private readonly ITranslatorService _translatorService;

		public GameplayInterfaceViewFactory(
			IGameplayInterfacePresenterProvider gameplayInterfacePresenterProvider,
			IGameMenuPresenterProvider gameMenuPresenterProvider,
			IGameStateChangerProvider stateChangerProvider,
			ITranslatorService translatorService,
			IResourceModelReadOnly resourceModelReadOnly,
			GameplayInterfaceView gameplayInterfaceView,
			IPlayerModelRepository playerModelRepository
		)
		{
			_gameplayInterfacePresenterProvider = gameplayInterfacePresenterProvider ??
				throw new ArgumentNullException(nameof(gameplayInterfacePresenterProvider));
			_gameMenuPresenterProvider = gameMenuPresenterProvider ??
				throw new ArgumentNullException(nameof(gameMenuPresenterProvider));
			_stateChangerProvider
				= stateChangerProvider ?? throw new ArgumentNullException(nameof(stateChangerProvider));
			_translatorService = translatorService ?? throw new ArgumentNullException(nameof(translatorService));
			_resourceModelReadOnly
				= resourceModelReadOnly ?? throw new ArgumentNullException(nameof(resourceModelReadOnly));
			_playerModelRepository
				= playerModelRepository ?? throw new ArgumentNullException(nameof(playerModelRepository));
			_gameplayInterfaceView = gameplayInterfaceView
				? gameplayInterfaceView
				: throw new ArgumentNullException(nameof(gameplayInterfaceView));
		}

		public void Create()
		{
			if (_resourceModelReadOnly == null) throw new ArgumentNullException(nameof(_resourceModelReadOnly));

			_gameMenuPresenterProvider.Register<IGameMenuPresenter>(
				new GameMenuPresenter(
					_gameplayInterfaceView.GetComponent<IGameMenuView>(),
					_stateChangerProvider
						.Self
				)
			);

			bool isHalfScoreReached
				= _resourceModelReadOnly.CurrentTotalResources > _resourceModelReadOnly.CurrentTotalResources / 2;

			_gameplayInterfaceView.Construct(
				_gameplayInterfacePresenterProvider.Self,
				_resourceModelReadOnly.CurrentCashScore,
				_resourceModelReadOnly.CurrentTotalResources,
				(int)_playerModelRepository.Get(ProgressType.MaxCashScore).Value,
				_resourceModelReadOnly.MaxTotalResourceCount,
				_resourceModelReadOnly.SoftCurrency.Value,
				isHalfScoreReached,
				IsGlobalScoreViewed
			);

			_gameplayInterfaceView.Phrases.Phrases
				= _translatorService.GetLocalize(_gameplayInterfaceView.Phrases.Phrases);
		}
	}
}