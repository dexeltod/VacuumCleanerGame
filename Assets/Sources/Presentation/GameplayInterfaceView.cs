using System;
using Joystick_Pack.Scripts.Base;
using Sources.PresentationInterfaces;
using Sources.Services.Localization;
using Sources.ServicesInterfaces;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Sources.Presentation
{
	[RequireComponent(typeof(Canvas))]
	public class GameplayInterfaceView : MonoBehaviour, IDisposable, IGameplayInterfaceView
	{
		private const float MaxFillAmount = 1f;

		[FormerlySerializedAs("_phrasesTranslator")] [SerializeField] private TmpPhrases _phrases;

		[SerializeField] private TextMeshProUGUI _scoreCash;
		[SerializeField] private TextMeshProUGUI _globalScoreText;
		[SerializeField] private TextMeshProUGUI _maxGlobalScoreText;
		[SerializeField] private TextMeshProUGUI _moneyText;

		[SerializeField] private Button _goToNextLevelButton;

		[SerializeField] private Image _scoreFillBar;
		[SerializeField] private Joystick _joystick;

		[SerializeField] private Image _globalScoreImage;

		private IResourceProgressEventHandler _resourceProgressEventHandler;
		private Canvas _canvas;

		private int _cashScore;
		private int _maxCashScore;
		private int _globalScore;
		private int _maxGlobalScore;

		private bool _isInitialized;

		public TmpPhrases Phrases => _phrases;
		public Button GoToNextLevelButton => _goToNextLevelButton;
		public TextMeshProUGUI ScoreText => _scoreCash;
		public TextMeshProUGUI MoneyText => _moneyText;

		public Joystick Joystick => _joystick;
		public Image ScoreSlider => _scoreFillBar;
		public Canvas Canvas => _canvas;
		public GameObject GameObject { get; private set; }

		public event Action GoToTextLevelButtonClicked;
		public event Action ButtonDestroying;
		public event Action Destroying;

		~GameplayInterfaceView() =>
			Unsubscribe();

		public void Construct(
			int startCashScore,
			int maxCashScore,
			int maxGlobalScore,
			int moneyCount,
			IResourceProgressEventHandler resourceProgressEventHandler,
			bool isActiveOnStart
		)
		{
			if (resourceProgressEventHandler == null)
				throw new ArgumentNullException(nameof(resourceProgressEventHandler));
			if (startCashScore < 0) throw new ArgumentOutOfRangeException(nameof(startCashScore));
			if (maxCashScore < 0) throw new ArgumentOutOfRangeException(nameof(maxCashScore));
			if (maxGlobalScore < 0) throw new ArgumentOutOfRangeException(nameof(maxGlobalScore));

			if (_isInitialized == true)
				return;

			_resourceProgressEventHandler = resourceProgressEventHandler;

			_goToNextLevelButton.gameObject.SetActive(false);

			_canvas ??= GetComponent<Canvas>();

			_moneyText.SetText($"{moneyCount}");

			Subscribe();

			_maxCashScore = maxCashScore;
			SetCashScoreText(startCashScore);
			enabled = true;

			OnSetMaxGlobalScore(maxGlobalScore);

			GameObject = gameObject;

			_isInitialized = true;
		}

		public void Dispose()
		{
			Unsubscribe();
			GC.SuppressFinalize(this);
		}

		private void OnDestroy()
		{
			Unsubscribe();
			ButtonDestroying?.Invoke();
			Destroying?.Invoke();
		}

		private void OnSetCashScore(int newScore)
		{
			SetCashScoreValue(newScore);
			SetCashScoreText(newScore);
		}

		private void OnSetGlobalScore(int newScore)
		{
			if (newScore < 0) throw new ArgumentOutOfRangeException(nameof(newScore));
			_globalScore = newScore;

			_globalScoreImage.fillAmount = NormalizeValue(
				MaxFillAmount,
				_globalScore,
				_maxGlobalScore
			);

			_globalScoreText.SetText($"{_globalScore}");
		}

		private void OnSetMaxCashScore(int newScore)
		{
			if (newScore < 0) throw new ArgumentOutOfRangeException(nameof(newScore));
			_maxCashScore = newScore;
			_maxGlobalScoreText.SetText($"{_maxCashScore}");
		}

		private void SetActiveGoToNextLevelButton(bool isActive) =>
			_goToNextLevelButton.gameObject.SetActive(isActive);

		private void OnSetMaxGlobalScore(int newMaxScore)
		{
			if (newMaxScore < 0) throw new ArgumentOutOfRangeException(nameof(newMaxScore));
			_maxGlobalScore = newMaxScore;
			_maxGlobalScoreText.SetText($"{_maxGlobalScore}");
		}

		private void OnSetSoftCurrency(int newMoney)
		{
			if (newMoney < 0) throw new ArgumentOutOfRangeException(nameof(newMoney));
			_moneyText.SetText(newMoney.ToString());
		}

		private void SetCashScoreValue(int newScore)
		{
			_cashScore = newScore;

			float value = NormalizeValue(
				MaxFillAmount,
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

		private void OnGoToNextLevelButtonClicked()
		{
			Debug.Assert(
				GoToTextLevelButtonClicked != null,
				nameof(GoToTextLevelButtonClicked) + " != null"
			);
			GoToTextLevelButtonClicked.Invoke();
		}

		private void Subscribe()
		{
			_goToNextLevelButton.onClick.AddListener(OnGoToNextLevelButtonClicked);

			_resourceProgressEventHandler.CashScoreChanged += OnSetCashScore;
			_resourceProgressEventHandler.GlobalScoreChanged += OnSetGlobalScore;
			_resourceProgressEventHandler.MaxCashScoreChanged += OnSetMaxCashScore;
			_resourceProgressEventHandler.MaxGlobalScoreChanged += OnSetMaxGlobalScore;
			_resourceProgressEventHandler.SoftCurrencyChanged += OnSetSoftCurrency;
			_resourceProgressEventHandler.HalfGlobalScoreReached += SetActiveGoToNextLevelButton;
		}

		private void Unsubscribe()
		{
			_goToNextLevelButton.onClick.RemoveListener(OnGoToNextLevelButtonClicked);

			_resourceProgressEventHandler.CashScoreChanged -= OnSetCashScore;
			_resourceProgressEventHandler.GlobalScoreChanged -= OnSetGlobalScore;
			_resourceProgressEventHandler.MaxCashScoreChanged -= OnSetMaxCashScore;
			_resourceProgressEventHandler.MaxGlobalScoreChanged -= OnSetMaxGlobalScore;
			_resourceProgressEventHandler.SoftCurrencyChanged -= OnSetSoftCurrency;
			_resourceProgressEventHandler.HalfGlobalScoreReached -= SetActiveGoToNextLevelButton;
		}
	}
}