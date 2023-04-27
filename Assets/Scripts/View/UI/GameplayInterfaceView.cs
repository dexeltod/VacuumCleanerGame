using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using ViewModel;

namespace DefaultNamespace.Presenter
{
	public class GameplayInterfaceView : MonoBehaviour
	{
		[SerializeField] private Slider _scoreSlider;
		[SerializeField] private TextMeshProUGUI _scoreText;
		[SerializeField] private TextMeshProUGUI _moneyText;
		[SerializeField] private Joystick _joystick;

		private IGameProgressViewModel _gameProgress;
		private int _maxScore;

		public Slider ScoreSlider => _scoreSlider;
		public TextMeshProUGUI ScoreText => _scoreText;
		public TextMeshProUGUI MoneyText => _moneyText;
		public Joystick Joystick => _joystick;

		~GameplayInterfaceView()
		{
			_gameProgress.ScoreChanged -= OnScoreChanged;
			_gameProgress.MoneyChanged -= OnMoneyChanged;
		}

		public void Construct(IGameProgressViewModel gameProgress, int maxScore)
		{
			_gameProgress = gameProgress;
			_gameProgress.ScoreChanged += OnScoreChanged;
			_gameProgress.MoneyChanged += OnMoneyChanged;
			_maxScore = maxScore;
			OnScoreChanged(0);
			enabled = true;
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