using System;
using Joystick_Pack.Scripts.Base;
using Sources.PresentationInterfaces;
using Sources.ServicesInterfaces;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Sources.Services
{
	public class GameplayInterfaceView : MonoBehaviour, IDisposable, IGameplayInterfaceView
	{
		private const float MaxFillAmount = 1f;

		[SerializeField] private TextMeshProUGUI _currentLevel;
		[SerializeField] private TextMeshProUGUI _scoreCash;
		[SerializeField] private TextMeshProUGUI _globalScoreText;
		[SerializeField] private TextMeshProUGUI _maxGlobalScoreText;
		[SerializeField] private TextMeshProUGUI _moneyText;

		[SerializeField] private Button _goToNextLevelButton;

		[SerializeField] private Image    _scoreFillBar;
		[SerializeField] private Joystick _joystick;

		[SerializeField] private Image _globalScoreImage;

		private Canvas _canvas;

		private int _cashScore;
		private int _maxCashScore;
		private int _globalScore;
		private int _maxGlobalScore = 1000; //TODO: Need config for global score

		private bool                          _isInitialized;
		private IResourceProgressEventHandler _resourceProgressEventHandler;

		public TextMeshProUGUI ScoreText => _scoreCash;
		public TextMeshProUGUI MoneyText => _moneyText;

		public Joystick   Joystick    => _joystick;
		public Image      ScoreSlider => _scoreFillBar;
		public Canvas     Canvas      => _canvas;
		public GameObject GameObject  { get; private set; }

		public event Action GoToTextLevelButtonClicked;
		public event Action ButtonDestroying;
		public event Action Destroying;

		~GameplayInterfaceView() =>
			Unsubscribe();

		public void Construct
		(
			int                           startCashScore,
			int                           maxScore,
			int                           moneyCount,
			IResourceProgressEventHandler resourcesProgressPresenter,
			bool isActiveOnStart
		)
		{
			if (_isInitialized == true)
				return;

			_resourceProgressEventHandler = resourcesProgressPresenter;

			_goToNextLevelButton.gameObject.SetActive(false);

			_canvas ??= GetComponent<Canvas>();

			_moneyText.SetText($"{moneyCount}");

			Subscribe();

			_maxCashScore = maxScore;
			SetCashScoreText(startCashScore);
			enabled = true;

			OnSetMaxGlobalScore(_maxGlobalScore);

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
			_globalScore = newScore;

			_globalScoreImage.fillAmount = CalculateValue
			(
				MaxFillAmount,
				_globalScore,
				_maxGlobalScore
			);

			_globalScoreText.SetText($"{_globalScore}");
		}

		private void OnSetMaxCashScore(int newScore)
		{
			_maxCashScore = newScore;
			_maxGlobalScoreText.SetText($"{_maxCashScore}");
		}

		private void SetActiveGoToNextLevelButton(bool isActive) =>
			_goToNextLevelButton.gameObject.SetActive(isActive);

		private void OnSetMaxGlobalScore(int newMaxScore)
		{
			_maxGlobalScore = newMaxScore;
			_maxGlobalScoreText.SetText($"{_maxGlobalScore}");
		}

		private void OnSetSoftCurrency(int newMoney) =>
			_moneyText.SetText(newMoney.ToString());

		public void SetCurrentLevel(int newLevel) =>
			_currentLevel.SetText($"{newLevel}");

		private void SetCashScoreValue(int newScore)
		{
			_cashScore = newScore;
			float value = CalculateValue
			(
				MaxFillAmount,
				_cashScore,
				_maxCashScore
			);
			_scoreFillBar.fillAmount = value;
		}

		private float CalculateValue(float maxValue, int newScore, int currentMaxScore) =>
			maxValue / currentMaxScore * newScore;

		private void SetCashScoreText(int newScore) =>
			_scoreCash.SetText($"{newScore}/{_maxCashScore}");

		private void OnGoToNextLevelButtonClicked()
		{
			Debug.Assert
			(
				GoToTextLevelButtonClicked != null,
				nameof(GoToTextLevelButtonClicked) + " != null"
			);
			GoToTextLevelButtonClicked.Invoke();
		}

		private void Subscribe()
		{
			_goToNextLevelButton.onClick.AddListener(OnGoToNextLevelButtonClicked);

			_resourceProgressEventHandler.CashScoreChanged       += OnSetCashScore;
			_resourceProgressEventHandler.GlobalScoreChanged     += OnSetGlobalScore;
			_resourceProgressEventHandler.MaxCashScoreChanged    += OnSetMaxCashScore;
			_resourceProgressEventHandler.MaxGlobalScoreChanged  += OnSetMaxGlobalScore;
			_resourceProgressEventHandler.SoftCurrencyChanged    += OnSetSoftCurrency;
			_resourceProgressEventHandler.HalfGlobalScoreReached += SetActiveGoToNextLevelButton;
		}

		private void Unsubscribe()
		{
			_goToNextLevelButton.onClick.RemoveListener(OnGoToNextLevelButtonClicked);

			_resourceProgressEventHandler.CashScoreChanged       -= OnSetCashScore;
			_resourceProgressEventHandler.GlobalScoreChanged     -= OnSetGlobalScore;
			_resourceProgressEventHandler.MaxCashScoreChanged    -= OnSetMaxCashScore;
			_resourceProgressEventHandler.MaxGlobalScoreChanged  -= OnSetMaxGlobalScore;
			_resourceProgressEventHandler.SoftCurrencyChanged    -= OnSetSoftCurrency;
			_resourceProgressEventHandler.HalfGlobalScoreReached -= SetActiveGoToNextLevelButton;
		}
	}
}