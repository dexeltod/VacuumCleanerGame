using System;
using DG.Tweening;
using Sources.ApplicationServicesInterfaces;
using Sources.ControllersInterfaces;
using Sources.DomainInterfaces;
using Sources.Presentation.Common;
using Sources.PresentationInterfaces;
using Sources.ServicesInterfaces;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Sources.Presentation.UI
{
	public class MainMenuBehaviour : PresentableView<IMainMenuPresenter>, IMainMenuView
	{
		[SerializeField] private Button _playButton;
		[SerializeField] private Button _deleteSavesButton;
		[SerializeField] private Button _addScoreButton;
		[SerializeField] private Button _leaderboardButton;
		[SerializeField] private GameObject _test;

		[FormerlySerializedAs("_translatorBehaviour")] [SerializeField]
		private TmpPhrases _translator;

		private ILevelConfigGetter _levelConfigGetter;
		private ILeaderBoardService _leaderBoardService;
		private IProgressSaveLoadDataService _progressSaveLoadDataService;
		private ILevelProgressFacade _levelProgressFacade;

		public TmpPhrases Translator => _translator;
		public event Action PlayButtonPressed;
		public event Action DeleteSavesButtonPressed;

		private void Construct(IMainMenuPresenter mainMenuPresenter) =>
			base.Construct(mainMenuPresenter);

		private void OnEnable()
		{
			_leaderboardButton.onClick.AddListener(OnShowLeaderboard);
			_addScoreButton.onClick.AddListener(OnAddLeader);
			_playButton.onClick.AddListener(OnPlay);
			_deleteSavesButton.onClick.AddListener(OnDeleteSaves);

			_test.transform.DOScale(_test.transform.localScale * 1.3f, 1f).SetLoops(-1, LoopType.Yoyo);
		}

		private void OnShowLeaderboard() { }

		private void OnDisable()
		{
			_playButton.onClick.RemoveListener(OnPlay);
			_addScoreButton.onClick.RemoveListener(OnAddLeader);
			_deleteSavesButton.onClick.RemoveListener(OnDeleteSaves);
		}
	

		private void OnPlay() =>
			PlayButtonPressed!.Invoke();

		private async void OnAddLeader() =>
			await _leaderBoardService.AddScore(200);

		private void OnDeleteSaves() =>
			DeleteSavesButtonPressed!.Invoke();

		private void OnExit() =>
			UnityEngine.Application.Quit();
	}
}