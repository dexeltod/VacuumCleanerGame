using System;
using Joystick_Pack.Scripts.Base;
using Sources.PresentationInterfaces;
using Sources.ServicesInterfaces;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
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

		[SerializeField] private Slider   _scoreSlider;
		[SerializeField] private Joystick _joystick;

		[FormerlySerializedAs("_scoresSprite")] [SerializeField]
		private Image _globalScoreImage;

		private Canvas _canvas;
		private int    _maxCashScore;
		private int    _globalScore;
		private bool   _isInitialized;

		public TextMeshProUGUI ScoreText => _scoreCash;
		public TextMeshProUGUI MoneyText => _moneyText;

		public Joystick   Joystick    => _joystick;
		public Slider     ScoreSlider => _scoreSlider;
		public GameObject GameObject  => gameObject;
		public Canvas     Canvas      => _canvas;

		public event Action GoToTextLevelButtonClicked;

		~GameplayInterfaceView() =>
			Unsubscribe();

		public void Dispose()
		{
			Unsubscribe();
			GC.SuppressFinalize(this);
		}

		public void SetGlobalScore(int newScore)
		{
			_globalScore = newScore;

			float value = MaxFillAmount / _maxCashScore * _globalScore;
			_globalScoreImage.fillAmount = value;

			_globalScoreText.SetText($"{_globalScore}");
		}

		public void Construct
		(
			int maxScore,
			int moneyCount
		)
		{
			if (_isInitialized == true)
				return;

			_goToNextLevelButton.gameObject.SetActive(false);
			const int StartScore = 0;

			_canvas ??= GetComponent<Canvas>();

			_moneyText.SetText($"{moneyCount}");

			Subscribe();

			_maxCashScore = maxScore;
			SetScoreText(StartScore);
			enabled = true;

			_isInitialized = true;
		}

		private void Subscribe() =>
			_goToNextLevelButton.onClick.AddListener(OnGoToNextLevelButtonClicked);

		private void Unsubscribe() =>
			_goToNextLevelButton.onClick.RemoveListener(OnGoToNextLevelButtonClicked);

		public void SetActiveGoToNextLevelButton(bool isActive) =>
			_goToNextLevelButton.gameObject.SetActive(isActive);

		public void SetMaxCashScore(int newMaxScore)
		{
			_maxCashScore = newMaxScore;
			_maxGlobalScoreText.SetText($"{newMaxScore}");
		}

		public void SetMoney(int newMoney) =>
			_moneyText.SetText(newMoney.ToString());

		public void SetCurrentLevel(int newLevel) =>
			_currentLevel.SetText($"{newLevel}");

		public void SetScore(int newScore)
		{
			SetSliderValue(newScore);
			SetScoreText(newScore);
		}

		private void SetSliderValue(int newScore)
		{
			float value = CalculateValue(newScore);
			_scoreSlider.value = value;
		}

		private float CalculateValue(int newScore) =>
			_scoreSlider.maxValue / _maxCashScore * newScore;

		private void SetScoreText(int newScore) =>
			_scoreCash.SetText($"{newScore}/{_maxCashScore}");

		private void OnGoToNextLevelButtonClicked() =>
			GoToTextLevelButtonClicked.Invoke();
	}
}