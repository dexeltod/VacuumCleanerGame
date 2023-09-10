using Joystick_Pack.Scripts.Base;
using Sources.ServicesInterfaces;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Sources.Services
{
	public class GameplayInterfaceView : MonoBehaviour
	{
		[SerializeField] private Slider _scoreSlider;
		[SerializeField] private TextMeshProUGUI _scoreText;
		[SerializeField] private TextMeshProUGUI _moneyText;
		[SerializeField] private Joystick _joystick;
		[SerializeField] private TextMeshProUGUI _playerName;

		private IResourcesProgressPresenter _resourcesProgress;
		private Canvas _canvas;
		private int _maxScore;
		private bool _isInitialized;

		public Slider ScoreSlider => _scoreSlider;
		public TextMeshProUGUI ScoreText => _scoreText;
		public TextMeshProUGUI MoneyText => _moneyText;
		public Joystick Joystick => _joystick;
		public Canvas Canvas => _canvas;
		public TextMeshProUGUI PlayerName => _playerName;

		~GameplayInterfaceView()
		{
			_resourcesProgress.ScoreChanged -= OnScoreChanged;
			_resourcesProgress.MoneyChanged -= OnMoneyChanged;
		}

		public void Construct(IResourcesProgressPresenter resourcesProgress, int maxScore)
		{
			if (_isInitialized)
				return;

			_canvas ??= GetComponent<Canvas>();
			_resourcesProgress = resourcesProgress;
			
			_moneyText.SetText(_resourcesProgress.SoftCurrency.Count.ToString());
			
			_resourcesProgress.ScoreChanged += OnScoreChanged;
			_resourcesProgress.MoneyChanged += OnMoneyChanged;
			_maxScore = maxScore;
			OnScoreChanged(0);
			enabled = true;
			
			_isInitialized = true;
		}

		public void SetMaxScore(int newMaxScore) => _maxScore = newMaxScore;

		private void OnMoneyChanged(int newMoney)
		{
			_moneyText.SetText(newMoney.ToString());
		}

		private void OnScoreChanged(int newScore)
		{
			float value = (_scoreSlider.maxValue / _maxScore) * newScore;
			_scoreSlider.value = value;
			_scoreText.SetText($"{newScore}/{_maxScore}");
		}
	}
}