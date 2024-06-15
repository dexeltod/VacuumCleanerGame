using System;
using Sources.ApplicationServicesInterfaces;
using Sources.Controllers.Common;
using Sources.ControllersInterfaces;
using Sources.Domain.Settings;
using Sources.DomainInterfaces;
using Sources.InfrastructureInterfaces;
using Sources.InfrastructureInterfaces.Configs;
using Sources.InfrastructureInterfaces.Factory;
using Sources.InfrastructureInterfaces.Services;
using Sources.InfrastructureInterfaces.States;
using Sources.PresentationInterfaces;
using Sources.ServicesInterfaces;
using UnityEngine;
using UnityEngine.Audio;

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
		private readonly ISettingsView _settingsView;
		private readonly AudioMixer _mixer;
		private readonly ILeaderBoardPlayersFactory _leaderBoardPlayersFactory;
		private readonly SoundSettings _soundSettings;
		private readonly ILeaderFactory _leaderFactory;
		private readonly SettingsPresenter _settingsPresenter;

		private bool _leaderboardInitialized;

		private int CurrentNumber => _levelProgress.CurrentLevel;

		public MainMenuPresenter(
			IMainMenuView mainMenu,
			ILevelProgressFacade levelProgress,
			IGameStateChanger stateMachine,
			ILevelConfigGetter levelConfigGetter,
			IProgressSaveLoadDataService progressSaveLoadDataService,
			IAuthorizationPresenter authorizationPresenter,
			ILeaderBoardView leaderBoardView,
			ILeaderBoardService leaderBoardService,
			ISettingsView settingsView,
			AudioMixer mixer,
			ILeaderBoardPlayersFactory leaderBoardPlayersFactory,
			SoundSettings soundSettings,
			ILeaderFactory leaderFactory
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
			_settingsView = settingsView ?? throw new ArgumentNullException(nameof(settingsView));
			_mixer = mixer ?? throw new ArgumentNullException(nameof(mixer));
			_leaderBoardPlayersFactory = leaderBoardPlayersFactory ??
				throw new ArgumentNullException(nameof(leaderBoardPlayersFactory));
			_soundSettings = soundSettings ?? throw new ArgumentNullException(nameof(soundSettings));
			_leaderFactory = leaderFactory ?? throw new ArgumentNullException(nameof(leaderFactory));

			_settingsPresenter = new SettingsPresenter(mixer, soundSettings);
		}

		public override void Enable()
		{
			Debug.Log("Enable MainMenuPresenter");
			_mainMenuView.PlayButton.onClick.AddListener(OnPlay);
			_mainMenuView.DeleteSavesButton.onClick.AddListener(OnDeleteSaves);
			_mainMenuView.LeaderboardButton.onClick.AddListener(OnShowLeaderBoard);
			_mainMenuView.AddScoreButton.onClick.AddListener(OnAddLeader);
			_mainMenuView.SettingsButton.onClick.AddListener(OnSettings);
			_mainMenuView.AddButton.onClick.AddListener(OnAddPanel);

			_mainMenuView.Enable();
			_settingsView.MasterVolumeSlider.onValueChanged.AddListener(OnSoundChanged);

			_settingsView.MasterVolumeSlider.value = _soundSettings.MasterVolume;
		}

		private void OnSoundChanged(float masterVolume) =>
			_settingsPresenter.SetSoundVolume(masterVolume);

		public override void Disable()
		{
			Debug.Log("Disable MainMenuPresenter");
			_settingsView.MasterVolumeSlider.onValueChanged.RemoveListener(OnSoundChanged);

			_mainMenuView.PlayButton.onClick.RemoveListener(OnPlay);
			_mainMenuView.DeleteSavesButton.onClick.RemoveListener(OnDeleteSaves);
			_mainMenuView.LeaderboardButton.onClick.RemoveListener(OnShowLeaderBoard);
			_mainMenuView.AddScoreButton.onClick.RemoveListener(OnAddLeader);
			_mainMenuView.SettingsButton.onClick.RemoveListener(OnSettings);
			_mainMenuView.AddButton.onClick.RemoveListener(OnAddPanel);
			_mainMenuView.Disable();
		}

		private void OnAddPanel() =>
			_leaderFactory.Create();

		private void OnSettings() =>
			_settingsView.Enable();

		private void OnShowLeaderBoard()
		{
			Debug.Log("OnShowLeaderBoard");

			if (_authorizationPresenter.IsAuthorized == false)
			{
				_authorizationPresenter.Authorize();
				return;
			}

			if (_authorizationPresenter.IsAuthorized == false)
				throw new Exception("You are not authorized");

			if (_leaderboardInitialized == false)
			{
				_leaderBoardPlayersFactory.Create();
				_leaderboardInitialized = true;
			}

			_leaderBoardView.Enable();
		}

		private async void OnDeleteSaves() =>
			await _progressSaveLoadDataService.ClearSaves();

		private void OnPlay() =>
			_stateMachine.Enter<IBuildSceneState, ILevelConfig>(_levelConfigGetter.GetOrDefault(CurrentNumber));

		private async void OnAddLeader() =>
			await _leaderBoardService.AddScore(200);
	}
}