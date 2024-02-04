using System;
using Sources.ApplicationServicesInterfaces.StateMachineInterfaces;
using Sources.Controllers.Common;
using Sources.DomainInterfaces;
using Sources.PresentationInterfaces;
using Sources.ServicesInterfaces;
using Sources.Utils.Configs.Scripts;

namespace Sources.Controllers
{
	public class MainMenuPresenter : Presenter
	{
		private readonly IMainMenuView _mainMenu;
		private readonly ILevelProgressFacade _levelProgress;
		private readonly IGameStateMachine _stateMachine;
		private readonly ILevelConfigGetter _levelConfigGetter;
		private readonly IProgressLoadDataService _progressLoadDataService;

		private int CurrentNumber => _levelProgress.CurrentLevelNumber;

		public MainMenuPresenter(
			IMainMenuView mainMenu,
			ILevelProgressFacade levelProgress,
			IGameStateMachine stateMachine,
			ILevelConfigGetter levelConfigGetter,
			IProgressLoadDataService progressLoadDataService
		)
		{
			_mainMenu = mainMenu ?? throw new ArgumentNullException(nameof(mainMenu));
			_levelProgress = levelProgress ?? throw new ArgumentNullException(nameof(levelProgress));
			_stateMachine = stateMachine ?? throw new ArgumentNullException(nameof(stateMachine));
			_levelConfigGetter = levelConfigGetter ?? throw new ArgumentNullException(nameof(levelConfigGetter));
			_progressLoadDataService = progressLoadDataService ??
				throw new ArgumentNullException(nameof(progressLoadDataService));
		}

		public override void Enable()
		{
			_mainMenu.PlayButtonPressed += OnPlay;
			_mainMenu.DeleteSavesButtonPressed += OnDeleteSaves;
		}

		public override void Disable() =>
			_mainMenu.PlayButtonPressed -= OnPlay;

		private async void OnDeleteSaves() =>
			await _progressLoadDataService.ClearSaves();

		private void OnPlay() =>
			_stateMachine.Enter<BuildSandState, LevelConfig>(_levelConfigGetter.Get(CurrentNumber));
	}
}