using Sources.ApplicationServicesInterfaces.StateMachineInterfaces;
using Sources.Controllers;
using Sources.DomainInterfaces;
using Sources.Infrastructure.Factories.Common.Decorators;
using Sources.PresentationInterfaces;
using Sources.ServicesInterfaces;
using VContainer;

namespace Sources.Infrastructure.Factories.Presenters
{
	public class MainMenuPresenterFactory : PresenterFactory<MainMenuPresenter>
	{
		private readonly IMainMenuView _mainMenu;
		private readonly ILevelProgressFacade _levelProgress;
		private readonly IGameStateMachine _stateMachine;
		private readonly ILevelConfigGetter _levelConfigGetter;
		private readonly IProgressLoadDataService _progressLoadDataService;

		[Inject]
		public MainMenuPresenterFactory(
			IMainMenuView mainMenu,
			ILevelProgressFacade levelProgress,
			IGameStateMachine stateMachine,
			ILevelConfigGetter levelConfigGetter,
			IProgressLoadDataService progressLoadDataService
		)
		{
			_mainMenu = mainMenu;
			_levelProgress = levelProgress;
			_stateMachine = stateMachine;
			_levelConfigGetter = levelConfigGetter;
			_progressLoadDataService = progressLoadDataService;
		}

		public override MainMenuPresenter Create() =>
			new(
				_mainMenu,
				_levelProgress,
				_stateMachine,
				_levelConfigGetter,
				_progressLoadDataService
			);
	}
}