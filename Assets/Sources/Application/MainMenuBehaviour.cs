using System;
using Sources.Application.StateMachine.GameStates;
using Sources.ApplicationServicesInterfaces;
using Sources.ApplicationServicesInterfaces.StateMachineInterfaces;
using Sources.DIService;
using Sources.DomainInterfaces;
using Sources.InfrastructureInterfaces;
using Sources.InfrastructureInterfaces.Scene;
using Sources.Utils.Configs;
using Sources.Utils.Configs.Scripts;
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

		private IGameStateMachine _gameStateMachine;
		private ILevelConfigGetter _levelConfigGetter;
		private IProgressLoadDataService _progressLoadDataService;
		private ILeaderBoardService _leaderBoardService;

		private ILevelProgressFacade _levelProgress;

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
			_levelProgress = ServiceLocator.Container.Get<ILevelProgressFacade>();
			_progressLoadDataService = ServiceLocator.Container.Get<IProgressLoadDataService>();
			_leaderBoardService = ServiceLocator.Container.Get<ILeaderBoardService>();
			_gameStateMachine = ServiceLocator.Container.Get<IGameStateMachine>();
			_levelConfigGetter = ServiceLocator.Container.Get<ILevelConfigGetter>();
		}

		public void Dispose() =>
			_playButton.onClick.RemoveListener(OnPlay);

		private void OnPlay()
		{
			LevelConfig levelConfig = _levelConfigGetter.Get(_levelProgress.CurrentLevelNumber);
			_gameStateMachine.Enter<BuildSandState, LevelConfig>(levelConfig);
		}

		private async void OnAddLeader() =>
			await _leaderBoardService.AddScore(200);

		private async void OnDeleteSaves() =>
			await _progressLoadDataService.ClearSaves();

		private void OnExit() =>
			UnityEngine.Application.Quit();
	}
}