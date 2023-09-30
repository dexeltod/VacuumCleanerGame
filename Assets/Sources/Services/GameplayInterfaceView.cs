using System;
using Joystick_Pack.Scripts.Base;
using Sources.ServicesInterfaces;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Sources.Services
{
	public class GameplayInterfaceView : MonoBehaviour, IDisposable
	{
		private const float MaxFillAmount = 1f;

		[SerializeField] private TextMeshProUGUI _scoreCash;
		[SerializeField] private TextMeshProUGUI _globalScoreText;
		[SerializeField] private TextMeshProUGUI _maxGlobalScoreText;
		[SerializeField] private TextMeshProUGUI _moneyText;

		[SerializeField] private Slider          _scoreSlider;
		[SerializeField] private Joystick        _joystick;
		[SerializeField] private TextMeshProUGUI _playerName;

		[FormerlySerializedAs("_scoresSprite")] [SerializeField]
		private Image _globalScoreImage;

		private IResourcesProgressPresenter _resourcesProgress;

		private Canvas _canvas;
		private int    _maxCashScore;
		private int    _globalScore;
		private bool   _isInitialized;

		public TextMeshProUGUI ScoreCash  => _scoreCash;
		public TextMeshProUGUI MoneyText  => _moneyText;
		public TextMeshProUGUI PlayerName => _playerName;

		public Joystick Joystick    => _joystick;
		public Slider   ScoreSlider => _scoreSlider;
		public Canvas   Canvas      => _canvas;

		~GameplayInterfaceView()
		{
			ReleaseUnmanagedResources();
		}

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
			if (_isInitialized)
				return;

			_canvas            ??= GetComponent<Canvas>();
			_resourcesProgress =   resourcesProgress;

			_moneyText.SetText
			(
				_resourcesProgress.SoftCurrency.Count.ToString()
			);

			_resourcesProgress.ScoreChanged       += OnScoreChanged;
			_resourcesProgress.GlobalScoreChanged += OnGlobalChanged;
			_resourcesProgress.MoneyChanged       += OnMoneyChanged;

			_maxCashScore = maxScore;
			OnScoreChanged(0);
			enabled = true;

			_isInitialized = true;
		}

		public void SetMaxCashScore(int newMaxScore) =>
			_maxCashScore = newMaxScore;

		private void OnGlobalChanged(int score)
		{
			_globalScore = score;
			float value = MaxFillAmount / _maxCashScore * score;
			_globalScoreImage.fillAmount = value;

			_globalScoreText.SetText($"{_globalScore}");
		}

		private void OnMoneyChanged(int newMoney) =>
			_moneyText.SetText(newMoney.ToString());

		private void OnScoreChanged(int newScore)
		{
			float value = _scoreSlider.maxValue / _maxCashScore * newScore;

			_scoreSlider.value = value;

			_scoreCash.SetText($"{newScore}/{_maxCashScore}");
		}

		private void ReleaseUnmanagedResources()
		{
			_resourcesProgress.ScoreChanged       -= OnScoreChanged;
			_resourcesProgress.GlobalScoreChanged -= OnGlobalChanged;
			_resourcesProgress.MoneyChanged       -= OnMoneyChanged;
		}
	}
}