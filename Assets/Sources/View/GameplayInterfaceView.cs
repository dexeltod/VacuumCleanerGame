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
	public class GameplayInterfaceView : MonoBehaviour, IDisposable, IGameplayInterfaceView, IGoToTextLevelButton
	{
		private const float MaxFillAmount = 1f;

		[SerializeField] private TextMeshProUGUI _scoreCash;
		[SerializeField] private TextMeshProUGUI _globalScoreText;
		[SerializeField] private TextMeshProUGUI _maxGlobalScoreText;
		[SerializeField] private TextMeshProUGUI _moneyText;

		[SerializeField] private Button _goToNextLevelButton;

		[SerializeField] private Slider   _scoreSlider;
		[SerializeField] private Joystick _joystick;

		[FormerlySerializedAs("_scoresSprite")] [SerializeField]
		private Image _globalScoreImage;

		private IResourcesProgressPresenter _resourcesProgress;

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
			ReleaseUnmanagedResources();

		public void Dispose()
		{
			ReleaseUnmanagedResources();
			GC.SuppressFinalize(this);
		}

		public void Construct
		(
			IResourcesProgressPresenter resourcesProgress,
			int                         maxScore
		)
		{
			if (_isInitialized == true)
				return;

			_goToNextLevelButton.gameObject.SetActive(false);
			const int StartScore = 0;

			_goToNextLevelButton.onClick.AddListener(OnGoToNextLevelButtonClicked);

			_canvas            ??= GetComponent<Canvas>();
			_resourcesProgress =   resourcesProgress;

			_resourcesProgress.HalfGlobalScoreReached += OnHalfScoreReached;

			_moneyText.SetText
			(
				_resourcesProgress
					.SoftCurrency
					.Count
					.ToString()
			);

			_resourcesProgress.ScoreChanged       += OnScoreChanged;
			_resourcesProgress.GlobalScoreChanged += OnGlobalChanged;
			_resourcesProgress.MoneyChanged       += OnMoneyChanged;

			_maxCashScore = maxScore;
			SetScoreText(StartScore);
			enabled = true;

			_isInitialized = true;
		}

		private void ReleaseUnmanagedResources()
		{
			_resourcesProgress.ScoreChanged       -= OnScoreChanged;
			_resourcesProgress.GlobalScoreChanged -= OnGlobalChanged;
			_resourcesProgress.MoneyChanged       -= OnMoneyChanged;

			_goToNextLevelButton.onClick.RemoveListener(OnGoToNextLevelButtonClicked);
		}

		private void OnHalfScoreReached() =>
			_goToNextLevelButton.gameObject.SetActive(true);

		public void SetMaxCashScore(int newMaxScore) =>
			_maxCashScore = newMaxScore;

		private void OnGlobalChanged()
		{
			_globalScore = _resourcesProgress.GlobalScore;

			float value = MaxFillAmount / _maxCashScore * _globalScore;
			_globalScoreImage.fillAmount = value;

			_globalScoreText.SetText($"{_globalScore}");
		}

		private void OnMoneyChanged(int newMoney) =>
			_moneyText.SetText(newMoney.ToString());

		private void OnScoreChanged(int newScore)
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