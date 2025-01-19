using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Sources.ControllersInterfaces;
using Sources.Frameworks.DOTween.Tweeners;
using Sources.Presentation.Common;
using Sources.PresentationInterfaces;
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
		[SerializeField] private Button _settingsButton;

		[FormerlySerializedAs("_addButton")]
		[SerializeField]
		private Button _addLeadersButton;

		[SerializeField] private GameObject _leaderBoardView;

		[FormerlySerializedAs("_translatorBehaviour")]
		[SerializeField]
		private TextPhrasesList _translator;

		private TweenerCore<Vector3, Vector3, VectorOptions> _playButtonTween;

		public TextPhrasesList Translator => _translator;
		public Button PlayButton => _playButton;
		public Button DeleteSavesButton => _deleteSavesButton;
		public Button AddScoreButton => _addScoreButton;
		public Button LeaderboardButton => _leaderboardButton;
		public Button SettingsButton => _settingsButton;
		public GameObject LeaderBoardView => _leaderBoardView;
		public Button AddLeadersButton => _addLeadersButton;

		public ISettingsView GetSettingsView() =>
			GetComponent<ISettingsView>();

		public override void Enable()
		{
			_playButtonTween ??= CustomTweeners.StartPulseLocal(
				_playButton.transform
			);
		}

		public override void Disable() =>
			_playButtonTween.Kill();

		public void OnDestroy() =>
			_playButtonTween.Kill();

		private void OnExit() =>
			Application.Quit();
	}
}
