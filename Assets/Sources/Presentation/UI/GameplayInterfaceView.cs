using System;
using Joystick_Pack.Scripts.Base;
using Sources.ControllersInterfaces;
using Sources.Presentation.Common;
using Sources.PresentationInterfaces;
using Sources.ServicesInterfaces;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Sources.Presentation.UI
{
	[RequireComponent(typeof(Canvas))]
	public class GameplayInterfaceView : PresentableView<IGameplayInterfacePresenter>,
		IGameplayInterfaceView

	{
		private const float MaxNormalizeThreshold = 1f;

		[FormerlySerializedAs("_phrasesTranslator")] [SerializeField]
		private TmpPhrases _phrases;

		[SerializeField] private TextMeshProUGUI _scoreCash;
		[SerializeField] private TextMeshProUGUI _globalScoreText;
		[SerializeField] private TextMeshProUGUI _maxGlobalScoreText;
		[SerializeField] private TextMeshProUGUI _moneyText;

		[SerializeField] private Button _goToNextLevelButton;
		[SerializeField] private Button _increaseSpeedButton;

		[SerializeField] private Image _scoreFillBar;
		[SerializeField] private Joystick _joystick;

		[SerializeField] private Image _globalScoreImage;

		private Canvas _canvas;

		private int _cashScore;
		private int _maxCashScore;
		private int _globalScore;
		private int _maxGlobalScore;

		private bool _isInitialized;
		private IGameMenuPresenter _gameMenuPresenter;

		public ITmpPhrases Phrases => _phrases;

		public Joystick Joystick => _joystick;

		public Canvas Canvas => _canvas;

		public GameObject InterfaceGameObject { get; private set; }

		public void Construct(
			IGameplayInterfacePresenter gameplayInterfacePresenter,
			int cashScore,
			int globalScore,
			int maxCashScore,
			int maxGlobalScore,
			int moneyCount,
			bool isActiveOnStart,
			IGameMenuPresenter gameMenuPresenter
		)
		{
			if (gameplayInterfacePresenter == null) throw new ArgumentNullException(nameof(gameplayInterfacePresenter));

			if (cashScore < 0) throw new ArgumentOutOfRangeException(nameof(cashScore));
			if (maxCashScore < 0) throw new ArgumentOutOfRangeException(nameof(maxCashScore));
			if (maxGlobalScore < 0) throw new ArgumentOutOfRangeException(nameof(maxGlobalScore));

			if (_isInitialized == true)
				return;

			base.Construct(gameplayInterfacePresenter);

			_goToNextLevelButton.gameObject.SetActive(false);

			_canvas ??= GetComponent<Canvas>();

			_moneyText.SetText($"{moneyCount}");

			_gameMenuPresenter = gameMenuPresenter ?? throw new ArgumentNullException(nameof(gameMenuPresenter));

			_globalScore = globalScore;
			_cashScore = cashScore;
			_maxCashScore = maxCashScore;
			_maxGlobalScore = maxGlobalScore;

			SetGlobalScore(globalScore);
			SetCashScore(cashScore);
			SetMaxGlobalScore(maxGlobalScore);

			InterfaceGameObject = gameObject;

			enabled = isActiveOnStart;

			_isInitialized = true;
		}

		public override void Enable() =>
			Subscribe();

		public override void Disable() =>
			Unsubscribe();

		private void OnDestroy() =>
			Unsubscribe();

		public void SetActiveGoToNextLevelButton(bool isActive) =>
			_goToNextLevelButton.gameObject.SetActive(isActive);

		public void SetCashScore(int newScore)
		{
			SetScoreBarValue(newScore);
			SetCashScoreText(newScore);
		}

		public void SetGlobalScore(int newScore)
		{
			if (newScore < 0) throw new ArgumentOutOfRangeException(nameof(newScore));
			_globalScore = newScore;

			_globalScoreImage.fillAmount = Normalizer.Normalize(
				MaxNormalizeThreshold,
				_globalScore,
				_maxGlobalScore
			);

			_globalScoreText.SetText($"{_globalScore}");
		}

		public void SetMaxCashScore(int newScore)
		{
			if (newScore < 0) throw new ArgumentOutOfRangeException(nameof(newScore));
			_maxCashScore = newScore;
			_maxGlobalScoreText.SetText($"{_maxCashScore}");
		}

		public void SetMaxGlobalScore(int newMaxScore)
		{
			if (newMaxScore < 0) throw new ArgumentOutOfRangeException(nameof(newMaxScore));
			_maxGlobalScore = newMaxScore;
			_maxGlobalScoreText.SetText($"{_maxGlobalScore}");
		}

		public void SetSoftCurrency(int newMoney)
		{
			if (newMoney < 0) throw new ArgumentOutOfRangeException(nameof(newMoney));
			_moneyText.SetText(newMoney.ToString());
		}

		private void SetScoreBarValue(int newScore)
		{
			_cashScore = newScore;

			float value = Normalizer.Normalize(MaxNormalizeThreshold, _cashScore, _maxCashScore);

			_scoreFillBar.fillAmount = value;
		}

		private void SetCashScoreText(int newScore) =>
			_scoreCash.SetText($"{newScore}/{_maxCashScore}");

		private void OnGoToNextLevelButtonClicked() =>
			Presenter.GoToNextLevel();

		private void OnIncreaseSpeedButtonClicked()
		{
			Presenter.IncreaseSpeed();
		}

		private void Subscribe()
		{
			_goToNextLevelButton.onClick.AddListener(OnGoToNextLevelButtonClicked);
			_increaseSpeedButton.onClick.AddListener(OnIncreaseSpeedButtonClicked);
		}

		private void Unsubscribe()
		{
			_goToNextLevelButton.onClick.RemoveListener(OnGoToNextLevelButtonClicked);
			_increaseSpeedButton.onClick.RemoveListener(OnIncreaseSpeedButtonClicked);
		}
	}
}