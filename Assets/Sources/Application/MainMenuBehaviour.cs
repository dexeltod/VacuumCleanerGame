using System;
using Sources.Application.StateMachine.GameStates;
using Sources.Application.StateMachineInterfaces;
using Sources.ApplicationServicesInterfaces;
using Sources.DIService;
using Sources.Domain.Progress;
using Sources.DomainInterfaces;
using Sources.InfrastructureInterfaces;
using Sources.InfrastructureInterfaces.Scene;
using Sources.Utils.Configs;
using UnityEngine;
using UnityEngine.UI;

namespace Sources.Application
{
	public class MainMenuBehaviour : MonoBehaviour, IDisposable
	{
		[SerializeField] private GameObject _settingsMenu;
		[SerializeField] private GameObject _mainMenu;

		[SerializeField] private Button _playButton;
		[SerializeField] private Button _deleteSavesButton;
		[SerializeField] private Button _addScoreButton;

		private IGameStateMachine        _gameStateMachine;
		private ILevelConfigGetter       _levelConfigGetter;
		private IProgressLoadDataService _progressLoadDataService;
		private ILeaderBoardService      _leaderBoardService;

		private ILevelProgressPresenter _levelProgress;

		private void OnEnable()
		{
			_addScoreButton.onClick.AddListener(OnAddLeader);
			_playButton.onClick.AddListener(OnPlay);
			_deleteSavesButton.onClick.AddListener(OnDeleteSaves);
		}

		private void OnDisable()
		{
			_playButton.onClick.RemoveListener(OnPlay);
			_addScoreButton.onClick.RemoveListener(OnAddLeader);
			_deleteSavesButton.onClick.RemoveListener(OnDeleteSaves);
		}

		private void Start()
		{
			_levelProgress = GameServices.Container.Get<ILevelProgressPresenter>();

			_progressLoadDataService = GameServices.Container.Get<IProgressLoadDataService>();
			_leaderBoardService      = GameServices.Container.Get<ILeaderBoardService>();
			_gameStateMachine        = GameServices.Container.Get<IGameStateMachine>();
			_levelConfigGetter       = GameServices.Container.Get<ILevelConfigGetter>();
		}

		public void Dispose()
		{
			_playButton.onClick.RemoveListener(OnPlay);
		}

		private void OnPlay()
		{
			_levelConfigGetter.Get(_levelProgress.CurrentLevelNumber);

			LevelConfig levelConfig = _levelConfigGetter.GetCurrentLevel();
			_gameStateMachine.Enter<BuildSceneState, LevelConfig>(levelConfig);
		}

		private async void OnAddLeader() =>
			await _leaderBoardService.AddScore(200);

		private async void OnDeleteSaves() =>
			await _progressLoadDataService.ClearSaves();

		private void OnSettings()
		{
			_mainMenu.gameObject.SetActive(false);
			_settingsMenu.gameObject.SetActive(true);
		}

		private void OnExit() =>
			UnityEngine.Application.Quit();
	}
}