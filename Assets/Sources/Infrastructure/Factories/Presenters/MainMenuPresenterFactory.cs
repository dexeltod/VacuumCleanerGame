using Sources.Controllers;
using Sources.Controllers.MainMenu;
using Sources.DomainInterfaces;
using Sources.Infrastructure.Common.Factory.Decorators;
using Sources.Infrastructure.Providers;
using Sources.InfrastructureInterfaces.Providers;
using Sources.InfrastructureInterfaces.Services;
using Sources.PresentationInterfaces;
using Sources.ServicesInterfaces;
using VContainer;

namespace Sources.Infrastructure.Factories.Presenters
{
	public class MainMenuPresenterFactory : PresenterFactory<MainMenuPresenter>
	{
		private readonly IMainMenuView _mainMenu;
		private readonly ILevelProgressFacade _levelProgress;
		private readonly IGameStateChangerProvider _stateMachineProvider;
		private readonly ILevelConfigGetter _levelConfigGetter;
		private readonly IProgressSaveLoadDataService _progressSaveLoadDataService;

		private IGameStateChanger StateMachine => _stateMachineProvider.Implementation;

		[Inject]
		public MainMenuPresenterFactory(
			IMainMenuView mainMenu,
			ILevelProgressFacade levelProgress,
			IGameStateChangerProvider stateMachineProvider,
			ILevelConfigGetter levelConfigGetter,
			IProgressSaveLoadDataService progressSaveLoadDataService
		)
		{
			_mainMenu = mainMenu;
			_levelProgress = levelProgress;
			_stateMachineProvider = stateMachineProvider;
			_levelConfigGetter = levelConfigGetter;
			_progressSaveLoadDataService = progressSaveLoadDataService;
		}

		public override MainMenuPresenter Create() =>
			new(
				_mainMenu,
				_levelProgress,
				StateMachine,
				_levelConfigGetter,
				_progressSaveLoadDataService
			);
	}
}