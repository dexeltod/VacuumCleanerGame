using System;
using Sources.Controllers.Common;
using Sources.ControllersInterfaces;
using Sources.DomainInterfaces;
using Sources.InfrastructureInterfaces.Services;
using Sources.InfrastructureInterfaces.States;
using Sources.Presentation;
using Sources.PresentationInterfaces;
using Sources.ServicesInterfaces;
using Sources.Utils.Configs.Scripts;

namespace Sources.Controllers.MainMenu
{
	public class MainMenuPresenter : Presenter, IMainMenuPresenter
	{
		private readonly IMainMenuView _mainMenu;
		private readonly ILevelProgressFacade _levelProgress;
		private readonly IGameStateChanger _stateMachine;
		private readonly ILevelConfigGetter _levelConfigGetter;
		private readonly IProgressSaveLoadDataService _progressSaveLoadDataService;
		private readonly IAuthorizationPresenter _authorizationPresenter;

		private int CurrentNumber => _levelProgress.CurrentLevelNumber;

		public MainMenuPresenter(
			IMainMenuView mainMenu,
			ILevelProgressFacade levelProgress,
			IGameStateChanger stateMachine,
			ILevelConfigGetter levelConfigGetter,
			IProgressSaveLoadDataService progressSaveLoadDataService,
			IAuthorizationPresenter authorizationPresenter
		)
		{
			_mainMenu = mainMenu ?? throw new ArgumentNullException(nameof(mainMenu));
			_levelProgress = levelProgress ?? throw new ArgumentNullException(nameof(levelProgress));
			_stateMachine = stateMachine ?? throw new ArgumentNullException(nameof(stateMachine));
			_levelConfigGetter = levelConfigGetter ?? throw new ArgumentNullException(nameof(levelConfigGetter));
			_progressSaveLoadDataService = progressSaveLoadDataService ??
				throw new ArgumentNullException(nameof(progressSaveLoadDataService));
			_authorizationPresenter = authorizationPresenter ?? throw new ArgumentNullException(nameof(authorizationPresenter));
		}

		public override void Enable()
		{
			_mainMenu.PlayButtonPressed += OnPlay;
			_mainMenu.DeleteSavesButtonPressed += OnDeleteSaves;
		}

		public override void Disable()
		{
			_mainMenu.PlayButtonPressed -= OnPlay;
			_mainMenu.DeleteSavesButtonPressed -= OnDeleteSaves;
		}

		public void Authorize()
		{
			_authorizationPresenter.Authorize();
		}
		
		private async void OnDeleteSaves() =>
			await _progressSaveLoadDataService.ClearSaves();

		private void OnPlay() =>
			_stateMachine.Enter<IBuildSceneState, LevelConfig>(_levelConfigGetter.Get(CurrentNumber));
	}
}