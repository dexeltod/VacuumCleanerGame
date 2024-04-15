using System;
using Sources.ApplicationServicesInterfaces;
using Sources.Controllers.Common;
using Sources.ControllersInterfaces;
using Sources.DomainInterfaces;
using Sources.InfrastructureInterfaces;
using Sources.InfrastructureInterfaces.Configs;
using Sources.InfrastructureInterfaces.Services;
using Sources.InfrastructureInterfaces.States;
using Sources.PresentationInterfaces;
using Sources.ServicesInterfaces;
using UnityEngine;

namespace Sources.Controllers.MainMenu
{
	public class MainMenuPresenter : Presenter, IMainMenuPresenter
	{
		private readonly IMainMenuView _mainMenuView;
		private readonly ILevelProgressFacade _levelProgress;
		private readonly IGameStateChanger _stateMachine;
		private readonly ILevelConfigGetter _levelConfigGetter;
		private readonly IProgressSaveLoadDataService _progressSaveLoadDataService;
		private readonly IAuthorizationPresenter _authorizationPresenter;
		private readonly ILeaderBoardView _leaderBoardView;
		private readonly ILeaderBoardService _leaderBoardService;

		private int CurrentNumber => _levelProgress.CurrentLevel;

		public MainMenuPresenter(
			IMainMenuView mainMenu,
			ILevelProgressFacade levelProgress,
			IGameStateChanger stateMachine,
			ILevelConfigGetter levelConfigGetter,
			IProgressSaveLoadDataService progressSaveLoadDataService,
			IAuthorizationPresenter authorizationPresenter,
			ILeaderBoardView leaderBoardView,
			ILeaderBoardService leaderBoardService
		)
		{
			_mainMenuView = mainMenu ?? throw new ArgumentNullException(nameof(mainMenu));
			_levelProgress = levelProgress ?? throw new ArgumentNullException(nameof(levelProgress));
			_stateMachine = stateMachine ?? throw new ArgumentNullException(nameof(stateMachine));
			_levelConfigGetter = levelConfigGetter ?? throw new ArgumentNullException(nameof(levelConfigGetter));
			_progressSaveLoadDataService = progressSaveLoadDataService ??
				throw new ArgumentNullException(nameof(progressSaveLoadDataService));
			_authorizationPresenter = authorizationPresenter ??
				throw new ArgumentNullException(nameof(authorizationPresenter));
			_leaderBoardView = leaderBoardView ?? throw new ArgumentNullException(nameof(leaderBoardView));
			_leaderBoardService = leaderBoardService ?? throw new ArgumentNullException(nameof(leaderBoardService));
		}

		public override void Enable()
		{
			_mainMenuView.PlayButton.onClick.AddListener(OnPlay);
			_mainMenuView.DeleteSavesButton.onClick.AddListener(OnDeleteSaves);
			_mainMenuView.LeaderboardButton.onClick.AddListener(OnShowLeaderboard);
			_mainMenuView.AddScoreButton.onClick.AddListener(OnAddLeader);
			_mainMenuView.Enable();
		}

		public override void Disable()
		{
			_mainMenuView.PlayButton.onClick.RemoveListener(OnPlay);
			_mainMenuView.DeleteSavesButton.onClick.RemoveListener(OnDeleteSaves);
			_mainMenuView.LeaderboardButton.onClick.RemoveListener(OnShowLeaderboard);
			_mainMenuView.AddScoreButton.onClick.RemoveListener(OnAddLeader);

			_mainMenuView.Disable();
		}

		private void ShowLeaderBoard()
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
			_stateMachine.Enter<IBuildSceneState, ILevelConfig>(_levelConfigGetter.GetOrDefault(CurrentNumber));

		private async void OnAddLeader() =>
			await _leaderBoardService.AddScore(200);

		private void OnShowLeaderboard() =>
			ShowLeaderBoard();
	}
}