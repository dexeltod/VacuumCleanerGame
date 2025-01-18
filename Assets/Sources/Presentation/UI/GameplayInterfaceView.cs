using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Plugins.Joystick_Pack.Scripts.Base;
using Sources.ControllersInterfaces;
using Sources.Domain.Interfaces;
using Sources.Frameworks.DOTween.Tweeners;
using Sources.Presentation.Common;
using Sources.PresentationInterfaces;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Sources.Presentation.UI
{
	[RequireComponent(typeof(Canvas))]
	public class GameplayInterfaceView : PresentableView<IGameplayInterfacePresenter>, IGameplayInterfaceView
	{
		private const float MaxNormalizeThreshold = 1f;

		[FormerlySerializedAs("_phrases")]
		[FormerlySerializedAs("_phrasesTranslator")]
		[SerializeField]
		private TextPhrasesList _phrasesList;

//=============================Text=========================================================
		[Header("Text")] [SerializeField] private TextMeshProUGUI _scoreCash;

		[SerializeField] private TextMeshProUGUI _globalScoreText;
		[SerializeField] private TextMeshProUGUI _maxGlobalScoreText;

		[SerializeField] private TextMeshProUGUI _moneyText;

//=============================Buttons===========================================================
		[Space]
		[Header("Buttons")]
		[SerializeField]
		private Button _goToNextLevelButton;

		[SerializeField] private Button _increaseSpeedButton;

//===============================Images============================================================
		[Space]
		[Header("Images")]
		[SerializeField]
		private Image _scoreFillBar;

		[SerializeField] private Image _increaseSpeedButtonImage;

		[SerializeField] private Image _globalScoreImage;

//=================================Other=======================================================
		[Space]
		[Header("Other")]
		[SerializeField]
		private Joystick _joystick;

		private int _cashScore;
		private int _globalScore;
		private TweenerCore<Vector3, Vector3, VectorOptions> _goToNextLevelButtonTween;
		private TweenerCore<Vector3, Vector3, VectorOptions> _increaseSpeedButtonTween;

		private bool _isInitialized;
		private bool _isScoresViewed;
		private int _maxCashScore;
		private int _maxGlobalScore;

		public ITextPhrases PhrasesList => _phrasesList;
		public Button GoToNextLevelButton => _goToNextLevelButton;
		public Button IncreaseSpeedButton => _increaseSpeedButton;
		public GameObject InterfaceGameObject { get; private set; }
		public Image IncreaseSpeedButtonImage => _increaseSpeedButtonImage;
		public Joystick Joystick => _joystick;

		public void Construct(
			IGameplayInterfacePresenter gameplayInterfacePresenter,
			int cashScore,
			int globalScore,
			int maxCashScore,
			int maxGlobalScore,
			int moneyCount,
			bool isHalfScoreReached,
			bool isScoresViewed
		)
		{
			_isScoresViewed = isScoresViewed;
			if (gameplayInterfacePresenter == null)
				throw new ArgumentNullException(
					nameof(gameplayInterfacePresenter)
				);

			if (cashScore < 0)
				throw new ArgumentOutOfRangeException(nameof(cashScore));

			if (maxGlobalScore < 0)
				throw new ArgumentOutOfRangeException(
					nameof(maxGlobalScore)
				);

			base.Construct(gameplayInterfacePresenter);

			_goToNextLevelButton.gameObject.SetActive(
				isHalfScoreReached
			);

			_moneyText.SetText(
				$"{moneyCount}"
			);

			_globalScore = globalScore;
			_cashScore = cashScore;
			_maxCashScore = maxCashScore;
			_maxGlobalScore = maxGlobalScore;

			SetTotalResourceScore(
				globalScore
			);
			SetCashScore(
				cashScore
			);

			SetMaxGlobalScore(
				maxGlobalScore
			);

			InterfaceGameObject = gameObject;
		}

		public override void Enable()
		{
			gameObject.SetActive(
				true
			);
			_goToNextLevelButtonTween ??= CustomTweeners.StartPulseLocal(
				_goToNextLevelButton.transform,
				1.15f
			);
			_increaseSpeedButtonTween ??= CustomTweeners.StartPulseLocal(
				_increaseSpeedButton.transform
			);

			SetActiveGlobalScores();
		}

		protected override void DestroySelf()
		{
			_goToNextLevelButtonTween!.Kill(
				true
			);
			_increaseSpeedButtonTween!.Kill(
				true
			);
			base.DestroySelf();
		}

		public override void Disable()
		{
			_goToNextLevelButtonTween!.Kill(
				true
			);
			_increaseSpeedButtonTween!.Kill(
				true
			);
			gameObject.SetActive(
				false
			);
		}

		public void SetActiveGoToNextLevelButton(bool isActive) =>
			_goToNextLevelButton.gameObject.SetActive(
				isActive
			);

		public void SetCashScore(int newScore)
		{
			SetScoreBarValue(
				newScore
			);
			SetCashScoreText(
				newScore
			);
		}

		public void SetTotalResourceScore(int newScore)
		{
			if (newScore < 0)
				throw new ArgumentOutOfRangeException(
					nameof(newScore)
				);

			_globalScore = newScore;

			_globalScoreImage.fillAmount = NormalizeValue(
				MaxNormalizeThreshold,
				_globalScore,
				_maxGlobalScore
			);

			_globalScoreText.SetText(
				$"{_globalScore}"
			);
		}

		public void SetMaxCashScore(int maxScore)
		{
			if (maxScore < 0)
				throw new ArgumentOutOfRangeException(
					nameof(maxScore)
				);

			Debug.Log(
				"SetMaxCashScore" + maxScore
			);
			_maxCashScore = maxScore;
			_maxGlobalScoreText.SetText(
				$"{_maxCashScore}"
			);
		}

		public void SetSoftCurrencyText(int newMoney)
		{
			if (newMoney < 0)
				throw new ArgumentOutOfRangeException(
					nameof(newMoney)
				);

			_moneyText.SetText(
				newMoney.ToString()
			);
		}

		public void FillSpeedButtonImage(float fillAmount) =>
			_increaseSpeedButtonImage.fillAmount = fillAmount;

		public void SetMaxGlobalScore(int newMaxScore)
		{
			if (newMaxScore < 0)
				throw new ArgumentOutOfRangeException(
					nameof(newMaxScore)
				);

			_maxGlobalScore = newMaxScore;
			_maxGlobalScoreText.SetText(
				$"{_maxGlobalScore}"
			);
		}

		private void SetActiveGlobalScores()
		{
			_globalScoreText.gameObject.SetActive(
				_isScoresViewed
			);
			_maxGlobalScoreText.gameObject.SetActive(
				_isScoresViewed
			);
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
			_scoreCash.SetText(
				$"{newScore}"
			);
	}
}
