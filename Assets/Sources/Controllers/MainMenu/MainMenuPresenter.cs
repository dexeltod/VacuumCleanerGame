using System;
using Sources.Controllers.Common;
using Sources.ControllersInterfaces;
using Sources.DomainInterfaces;
using Sources.InfrastructureInterfaces.Services;
using Sources.InfrastructureInterfaces.States;
using Sources.Presentation;
using Sources.Presentation.UI.MainMenu.LeaderBoard;
using Sources.PresentationInterfaces;
using Sources.ServicesInterfaces;
using Sources.Utils.Configs.Scripts;
using UnityEngine;

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
		private readonly ILeaderBoardView _leaderBoardView;

		private int CurrentNumber => _levelProgress.CurrentLevelNumber;

		public MainMenuPresenter(
			IMainMenuView mainMenu,
			ILevelProgressFacade levelProgress,
			IGameStateChanger stateMachine,
			ILevelConfigGetter levelConfigGetter,
			IProgressSaveLoadDataService progressSaveLoadDataService,
			IAuthorizationPresenter authorizationPresenter,
			ILeaderBoardView leaderBoardView
		)
		{
			_mainMenu = mainMenu ?? throw new ArgumentNullException(nameof(mainMenu));
			_levelProgress = levelProgress ?? throw new ArgumentNullException(nameof(levelProgress));
			_stateMachine = stateMachine ?? throw new ArgumentNullException(nameof(stateMachine));
			_levelConfigGetter = levelConfigGetter ?? throw new ArgumentNullException(nameof(levelConfigGetter));
			_progressSaveLoadDataService = progressSaveLoadDataService ??
				throw new ArgumentNullException(nameof(progressSaveLoadDataService));
			_authorizationPresenter = authorizationPresenter ??
				throw new ArgumentNullException(nameof(authorizationPresenter));
			_leaderBoardView = leaderBoardView ?? throw new ArgumentNullException(nameof(leaderBoardView));
		}

		public override void Enable()
		{
			_mainMenu.PlayButtonPressed += OnPlay;
			_mainMenu.DeleteSavesButtonPressed += OnDeleteSaves;
			_mainMenu.Enable();
		}

		public override void Disable()
		{
			_mainMenu.Disable();

			_mainMenu.PlayButtonPressed -= OnPlay;
			_mainMenu.DeleteSavesButtonPressed -= OnDeleteSaves;
		}

		public void ShowLeaderBoard()
		{
			if (_authorizationPresenter.IsAuthorized == false)
			{
				_authorizationPresenter.Authorize();
				return;
			}

			Debug.Log("ShowLeaderBoard");
			_leaderBoardView.Enable();
		}

		private async void OnDeleteSaves() =>
			await _progressSaveLoadDataService.ClearSaves();

		private void OnPlay() =>
			_stateMachine.Enter<IBuildSceneState, LevelConfig>(_levelConfigGetter.Get(CurrentNumber));
	}
}