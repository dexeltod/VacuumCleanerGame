using System;
using Sources.ApplicationServicesInterfaces;
using Sources.ApplicationServicesInterfaces.StateMachineInterfaces;

using Sources.DomainInterfaces;
using Sources.InfrastructureInterfaces;
using Sources.InfrastructureInterfaces.Scene;
using Sources.Presentation.UI;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace Sources.Application
{
	public class MainMenuBehaviour : MonoBehaviour, IMainMenuView, IDisposable
	{
		[SerializeField] private GameObject _settingsMenu;
		[SerializeField] private GameObject _mainMenu;

		[SerializeField] private Button _playButton;
		[SerializeField] private Button _deleteSavesButton;
		[SerializeField] private Button _addScoreButton;

		private ILevelConfigGetter _levelConfigGetter;
		private IGameStateMachine _gameStateMachine;
		private ILeaderBoardService _leaderBoardService;
		private IProgressLoadDataService _progressLoadDataService;
		private ILevelProgressFacade _levelProgressFacade;

		public event Action PlayButtonPressed;

		[Inject]
		private void Construct(
			ILevelProgressFacade levelProgressFacade,
			IProgressLoadDataService progressLoadDataService,
			ILeaderBoardService leaderBoardService,
			IGameStateMachine gameStateMachine,
			ILevelConfigGetter levelConfigGetter
		)
		{
			_levelConfigGetter = levelConfigGetter;
			_gameStateMachine = gameStateMachine;
			_leaderBoardService = leaderBoardService;
			_progressLoadDataService = progressLoadDataService;
			_levelProgressFacade = levelProgressFacade;
		}

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

		public void Dispose() =>
			_playButton.onClick.RemoveListener(OnPlay);

		private void OnPlay() =>
			PlayButtonPressed.Invoke();

		private async void OnAddLeader() =>
			await _leaderBoardService.AddScore(200);

		private async void OnDeleteSaves() =>
			await _progressLoadDataService.ClearSaves();

		private void OnExit() =>
			UnityEngine.Application.Quit();
	}
}