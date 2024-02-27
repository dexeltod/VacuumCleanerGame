using System;
using DG.Tweening;
using Joystick_Pack.Scripts.Base;
using Sources.Controllers;
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

		private int _cashScore;
		private int _maxCashScore;
		private int _globalScore;
		private int _maxGlobalScore;

		private bool _isInitialized;

		public ITmpPhrases Phrases => _phrases;

		public Joystick Joystick => _joystick;
		public GameObject InterfaceGameObject { get; private set; }

		public void Construct(
			IGameplayInterfacePresenter gameplayInterfacePresenter,
			int cashScore,
			int globalScore,
			int maxCashScore,
			int maxGlobalScore,
			int moneyCount,
			bool isHalfScoreReached
		)
		{
			if (gameplayInterfacePresenter == null) throw new ArgumentNullException(nameof(gameplayInterfacePresenter));

			if (cashScore < 0) throw new ArgumentOutOfRangeException(nameof(cashScore));
			if (maxCashScore < 0) throw new ArgumentOutOfRangeException(nameof(maxCashScore));
			if (maxGlobalScore < 0) throw new ArgumentOutOfRangeException(nameof(maxGlobalScore));

			base.Construct(gameplayInterfacePresenter);

			_goToNextLevelButton.gameObject.SetActive(isHalfScoreReached);

			_moneyText.SetText($"{moneyCount}");

			_globalScore = globalScore;
			_cashScore = cashScore;
			_maxCashScore = maxCashScore;
			_maxGlobalScore = maxGlobalScore;

			SetGlobalScore(globalScore);
			SetCashScore(cashScore);

			SetMaxGlobalScore(maxGlobalScore);

			InterfaceGameObject = gameObject;
		}

		public override void Enable()
		{
			base.Enable();
			
			Subscribe();
		}

		public override void Disable()
		{
			base.Disable();
			Unsubscribe();
		}

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

			_globalScoreImage.fillAmount = NormalizeValue(
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

		public void SetSoftCurrencyText(int newMoney)
		{
			if (newMoney < 0) throw new ArgumentOutOfRangeException(nameof(newMoney));
			_moneyText.SetText(newMoney.ToString());
		}

		private void SetScoreBarValue(int newScore)
		{
			_cashScore = newScore;

			float value = NormalizeValue(
				MaxNormalizeThreshold,
				_cashScore,
				_maxCashScore
			);

			_scoreFillBar.fillAmount = value;
		}

		private float NormalizeValue(
			float topValue,
			float newScore,
			float currentMaxScore
		) =>
			topValue / currentMaxScore * newScore;

		private void SetCashScoreText(int newScore) =>
			_scoreCash.SetText($"{newScore}/{_maxCashScore}");

		private void OnGoToNextLevelButtonClicked() =>
			Presenter.GoToNextLevel();

		private void OnIncreaseSpeed() =>
			Presenter.IncreaseSpeed();

		private void Subscribe()
		{
			_goToNextLevelButton.onClick.AddListener(OnGoToNextLevelButtonClicked);
			_increaseSpeedButton.onClick.AddListener(OnIncreaseSpeed);
		}

		private void Unsubscribe()
		{
			_goToNextLevelButton.onClick.RemoveListener(OnGoToNextLevelButtonClicked);
			_increaseSpeedButton.onClick.RemoveListener(OnIncreaseSpeed);
		}
	}
}