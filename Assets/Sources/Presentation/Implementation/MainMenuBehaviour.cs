using System;
using Sources.ApplicationServicesInterfaces;
using Sources.ApplicationServicesInterfaces.StateMachineInterfaces;
using Sources.DomainInterfaces;
using Sources.Presentation.UI;
using Sources.PresentationInterfaces;
using Sources.ServicesInterfaces;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using VContainer;

namespace Sources.Presentation.Implementation
{
	public class MainMenuBehaviour : MonoBehaviour, IMainMenuView, IDisposable
	{
		[SerializeField] private GameObject _settingsMenu;
		[SerializeField] private GameObject _mainMenu;

		[SerializeField] private Button _playButton;
		[SerializeField] private Button _deleteSavesButton;
		[SerializeField] private Button _addScoreButton;

		[FormerlySerializedAs("_translatorBehaviour")] [SerializeField]
		private TmpPhrases _translator;

		private ILevelConfigGetter _levelConfigGetter;
		private IGameStateMachine _gameStateMachine;
		private ILeaderBoardService _leaderBoardService;
		private IProgressLoadDataService _progressLoadDataService;
		private ILevelProgressFacade _levelProgressFacade;

		public TmpPhrases Translator => _translator;
		public event Action PlayButtonPressed;
		public event Action DeleteSavesButtonPressed;

		[Inject]
		private void Construct(
			ILevelProgressFacade levelProgressFacade,
			IProgressLoadDataService progressLoadDataService,
			ILeaderBoardService leaderBoardService,
			IGameStateMachine gameStateMachine,
			ILevelConfigGetter levelConfigGetter
		)
		{
			_levelConfigGetter = levelConfigGetter ?? throw new ArgumentNullException(nameof(levelConfigGetter));
			_gameStateMachine = gameStateMachine ?? throw new ArgumentNullException(nameof(gameStateMachine));
			_leaderBoardService = leaderBoardService ?? throw new ArgumentNullException(nameof(leaderBoardService));
			_progressLoadDataService = progressLoadDataService ??
				throw new ArgumentNullException(nameof(progressLoadDataService));
			_levelProgressFacade = levelProgressFacade ?? throw new ArgumentNullException(nameof(levelProgressFacade));
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

		public void Dispose()
		{
			_playButton.onClick.RemoveListener(OnPlay);
			_addScoreButton.onClick.RemoveListener(OnAddLeader);
			_deleteSavesButton.onClick.RemoveListener(OnDeleteSaves);
		}

		private void OnPlay() =>
			PlayButtonPressed.Invoke();

		private async void OnAddLeader() =>
			await _leaderBoardService.AddScore(200);

		private void OnDeleteSaves() =>
			DeleteSavesButtonPressed.Invoke();

		private void OnExit() =>
			UnityEngine.Application.Quit();
	}
}