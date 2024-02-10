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
	//potato realisation ╕_╡
	public class GameplayInterfaceView : PresentableView<IGameplayInterfacePresenter>, IDisposable,
		IGameplayInterfaceView

	{
		private const float MaxNormilizeThreshold = 1f;

		[FormerlySerializedAs("_phrasesTranslator")] [SerializeField]
		private TmpPhrases _phrases;

		[SerializeField] private TextMeshProUGUI _scoreCash;
		[SerializeField] private TextMeshProUGUI _globalScoreText;
		[SerializeField] private TextMeshProUGUI _maxGlobalScoreText;
		[SerializeField] private TextMeshProUGUI _moneyText;

		[SerializeField] private Button _goToNextLevelButton;

		[SerializeField] private Image _scoreFillBar;
		[SerializeField] private Joystick _joystick;

		[SerializeField] private Image _globalScoreImage;

		private Canvas _canvas;

		private int _cashScore;
		private int _maxCashScore;
		private int _globalScore;
		private int _maxGlobalScore;

		private bool _isInitialized;

		public ITmpPhrases Phrases => _phrases;

		public Joystick Joystick => _joystick;
		public Canvas Canvas => _canvas;
		public GameObject GameObject { get; private set; }

		public void Construct(
			IGameplayInterfacePresenter gameplayInterfacePresenter,
			int startCashScore,
			int maxCashScore,
			int maxGlobalScore,
			int moneyCount,
			bool isActiveOnStart
		)
		{
			if (gameplayInterfacePresenter == null) throw new ArgumentNullException(nameof(gameplayInterfacePresenter));

			if (startCashScore < 0) throw new ArgumentOutOfRangeException(nameof(startCashScore));
			if (maxCashScore < 0) throw new ArgumentOutOfRangeException(nameof(maxCashScore));
			if (maxGlobalScore < 0) throw new ArgumentOutOfRangeException(nameof(maxGlobalScore));

			if (_isInitialized == true)
				return;

			base.Construct(gameplayInterfacePresenter);

			_goToNextLevelButton.gameObject.SetActive(false);

			_canvas ??= GetComponent<Canvas>();

			_moneyText.SetText($"{moneyCount}");

			Subscribe();

			_maxCashScore = maxCashScore;
			SetCashScoreText(startCashScore);
			enabled = isActiveOnStart;

			SetMaxGlobalScore(maxGlobalScore);

			GameObject = gameObject;

			_isInitialized = true;
		}

		public void Dispose() =>
			Unsubscribe();

		private void OnDestroy() =>
			Unsubscribe();

		public void SetActiveGoToNextLevelButton(bool isActive) =>
			_goToNextLevelButton.gameObject.SetActive(isActive);

		public void SetCashScore(int newScore)
		{
			NormalizeScoreValue(newScore);
			SetCashScoreText(newScore);
		}

		public void SetGlobalScore(int newScore)
		{
			if (newScore < 0) throw new ArgumentOutOfRangeException(nameof(newScore));
			_globalScore = newScore;

			_globalScoreImage.fillAmount = NormalizeValue(
				MaxNormilizeThreshold,
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

		private void NormalizeScoreValue(int newScore)
		{
			_cashScore = newScore;

			float value = NormalizeValue(
				MaxNormilizeThreshold,
				_cashScore,
				_maxCashScore
			);

			_scoreFillBar.fillAmount = value;
		}

		private float NormalizeValue(
			float topValue,
			int newScore,
			int currentMaxScore
		) =>
			topValue / currentMaxScore * newScore;

		private void SetCashScoreText(int newScore) =>
			_scoreCash.SetText($"{newScore}/{_maxCashScore}");

		private void OnGoToNextLevelButtonClicked() =>
			Presenter.GoToNextLevel();

		private void Subscribe() =>
			_goToNextLevelButton.onClick.AddListener(OnGoToNextLevelButtonClicked);

		private void Unsubscribe() =>
			_goToNextLevelButton.onClick.RemoveListener(OnGoToNextLevelButtonClicked);
	}
}