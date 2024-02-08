using System;
using Sources.ApplicationServicesInterfaces;
using Sources.Controllers.MainMenu;
using Sources.ControllersInterfaces;
using Sources.DomainInterfaces;
using Sources.Presentation.Common;
using Sources.ServicesInterfaces;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Sources.Presentation.UI
{
	public class MainMenuBehaviour : PresentableView<MainMenuPresenter>, IMainMenuView, IDisposable
	{
		[SerializeField] private GameObject _settingsMenu;
		[SerializeField] private GameObject _mainMenu;

		[SerializeField] private Button _playButton;
		[SerializeField] private Button _deleteSavesButton;
		[SerializeField] private Button _addScoreButton;

		[FormerlySerializedAs("_translatorBehaviour")] [SerializeField]
		private TmpPhrases _translator;

		private ILevelConfigGetter _levelConfigGetter;
		private ILeaderBoardService _leaderBoardService;
		private IProgressSaveLoadDataService _progressSaveLoadDataService;
		private ILevelProgressFacade _levelProgressFacade;

		public TmpPhrases Translator => _translator;
		public event Action PlayButtonPressed;
		public event Action DeleteSavesButtonPressed;

	
		private void Construct(
			ILevelProgressFacade levelProgressFacade,
			IProgressSaveLoadDataService progressSaveLoadDataService,
			ILeaderBoardService leaderBoardService,
			ILevelConfigGetter levelConfigGetter
		)
		{
			_levelConfigGetter = levelConfigGetter ?? throw new ArgumentNullException(nameof(levelConfigGetter));
			_leaderBoardService = leaderBoardService ?? throw new ArgumentNullException(nameof(leaderBoardService));
			_progressSaveLoadDataService = progressSaveLoadDataService ??
				throw new ArgumentNullException(nameof(progressSaveLoadDataService));
			_levelProgressFacade = levelProgressFacade ?? throw new ArgumentNullException(nameof(levelProgressFacade));
		}

		public void Construct(IMainMenuPresenter presenter) =>
			throw new NotImplementedException();

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