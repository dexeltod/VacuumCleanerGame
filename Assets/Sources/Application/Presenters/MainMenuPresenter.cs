using System;
using Sources.Application.StateMachine.GameStates;
using Sources.ApplicationServicesInterfaces.StateMachineInterfaces;
using Sources.InfrastructureInterfaces;
using Sources.InfrastructureInterfaces.Scene;
using Sources.Presentation.UI;
using Sources.Utils.Configs.Scripts;
using VContainer;

namespace Sources.Infrastructure.Presenters
{
	public class MainMenuPresenter
	{
		private readonly IMainMenuView _mainMenu;
		private readonly ILevelProgressFacade _levelProgress;
		private readonly IGameStateMachine _stateMachine;
		private readonly ILevelConfigGetter _levelConfigGetter;

		[Inject]
		public MainMenuPresenter(
			IMainMenuView mainMenu,
			ILevelProgressFacade levelProgress,
			IGameStateMachine stateMachine,
			ILevelConfigGetter levelConfigGetter
		)
		{
			_mainMenu = mainMenu ?? throw new ArgumentNullException(nameof(mainMenu));
			_levelProgress = levelProgress ?? throw new ArgumentNullException(nameof(levelProgress));
			_stateMachine = stateMachine ?? throw new ArgumentNullException(nameof(stateMachine));
			_levelConfigGetter = levelConfigGetter ?? throw new ArgumentNullException(nameof(levelConfigGetter));
		}

		public void Enable() =>
			_mainMenu.PlayButtonPressed += OnPlay;

		public void Disable() =>
			_mainMenu.PlayButtonPressed -= OnPlay;

		private void OnPlay() =>
			_stateMachine.Enter<BuildSandState, LevelConfig>(
				_levelConfigGetter.Get(_levelProgress.CurrentLevelNumber)
			);
	}
}