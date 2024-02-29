using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Sources.ApplicationServicesInterfaces;
using Sources.ControllersInterfaces;
using Sources.DomainInterfaces;
using Sources.Presentation.Common;
using Sources.PresentationInterfaces;
using Sources.ServicesInterfaces;
using Sources.Utils.Tweeners;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Sources.Presentation.UI
{
	public class MainMenuView : PresentableView<IMainMenuPresenter>, IMainMenuView
	{
		[SerializeField] private Button _playButton;
		[SerializeField] private Button _deleteSavesButton;
		[SerializeField] private Button _addScoreButton;
		[SerializeField] private Button _leaderboardButton;

		[FormerlySerializedAs("_translatorBehaviour")] [SerializeField]
		private TextPhrases _translator;

		private ILevelConfigGetter _levelConfigGetter;
		private ILeaderBoardService _leaderBoardService;
		private IProgressSaveLoadDataService _progressSaveLoadDataService;
		private ILevelProgressFacade _levelProgressFacade;

		private TweenerCore<Vector3, Vector3, VectorOptions> _playButtonTween;

		public TextPhrases Translator => _translator;
		public event Action PlayButtonPressed;
		public event Action DeleteSavesButtonPressed;

		public override void Enable()
		{
			_leaderboardButton.onClick.AddListener(OnShowLeaderboard);
			_addScoreButton.onClick.AddListener(OnAddLeader);
			_playButton.onClick.AddListener(OnPlay);
			_deleteSavesButton.onClick.AddListener(OnDeleteSaves);

			_playButtonTween ??= CustomTweeners.StartPulseLocal(_playButton.transform);
		}

		public override void Disable()
		{
			_playButtonTween.Kill();

			_playButton.onClick.RemoveListener(OnPlay);
			_addScoreButton.onClick.RemoveListener(OnAddLeader);
			_deleteSavesButton.onClick.RemoveListener(OnDeleteSaves);
			_leaderboardButton.onClick.RemoveListener(OnShowLeaderboard);
		}

		public void OnDestroy()
		{
			_playButtonTween.Kill();
			_playButton.onClick.RemoveListener(OnPlay);
			_addScoreButton.onClick.RemoveListener(OnAddLeader);
			_deleteSavesButton.onClick.RemoveListener(OnDeleteSaves);
			_leaderboardButton.onClick.RemoveListener(OnShowLeaderboard);
		}

		private void OnPlay() =>
			PlayButtonPressed!.Invoke();

		private async void OnAddLeader() =>
			await _leaderBoardService.AddScore(200);

		private void OnShowLeaderboard() =>
			Presenter.ShowLeaderBoard();

		private void OnDeleteSaves() =>
			DeleteSavesButtonPressed!.Invoke();

		private void OnExit() =>
			UnityEngine.Application.Quit();
	}
}