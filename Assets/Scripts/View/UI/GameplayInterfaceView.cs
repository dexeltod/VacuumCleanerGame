using TMPro;
using UnityEngine;
using UnityEngine.UI;
using ViewModel.Infrastructure;

namespace View.UI
{
	public class GameplayInterfaceView : MonoBehaviour
	{
		[SerializeField] private Slider _scoreSlider;
		[SerializeField] private TextMeshProUGUI _scoreText;
		[SerializeField] private TextMeshProUGUI _moneyText;
		[SerializeField] private Joystick _joystick;

		private IPlayerProgressViewModel _playerProgress;
		private int _maxScore;
		private Canvas _canvas;
		private bool _isInitialized;

		public Slider ScoreSlider => _scoreSlider;
		public TextMeshProUGUI ScoreText => _scoreText;
		public TextMeshProUGUI MoneyText => _moneyText;
		public Joystick Joystick => _joystick;
		public Canvas Canvas => _canvas;

		~GameplayInterfaceView()
		{
			_playerProgress.ScoreChanged -= OnScoreChanged;
			_playerProgress.MoneyChanged -= OnMoneyChanged;
		}

		public void Construct(IPlayerProgressViewModel playerProgress, int maxScore)
		{
			if (_isInitialized)
				return;

			_canvas ??= GetComponent<Canvas>();
			_playerProgress = playerProgress;
			
			_moneyText.SetText(_playerProgress.Money.ToString());
			
			_playerProgress.ScoreChanged += OnScoreChanged;
			_playerProgress.MoneyChanged += OnMoneyChanged;
			_maxScore = maxScore;
			OnScoreChanged(0);
			enabled = true;
			
			_isInitialized = true;
		}

		public void UpdateMaxScore(int newMaxScore) => _maxScore = newMaxScore;

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