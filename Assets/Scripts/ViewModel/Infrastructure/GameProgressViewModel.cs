using System;
using Model.DI;
using Model.Infrastructure.Data;
using Model.Infrastructure.Services;
using UnityEngine;

namespace ViewModel
{
	public class GameProgressViewModel : IGameProgressViewModel
	{
		public event Action<int> ScoreChanged;
		public event Action<int> MoneyChanged;

		private readonly GameProgressModel _gameProgress;

		public GameProgressViewModel()
		{
			_gameProgress = ServiceLocator.Container.GetSingle<IPersistentProgressService>().GameProgress;
		}

		public bool CheckMaxScore()
		{
			if (_gameProgress.CurrentSandCount >= _gameProgress.MaxFilledScore)
				return false;

			return true;
		}

		public bool AddSand(int newScore)
		{
			int currentScore = _gameProgress.CurrentSandCount;
			currentScore += newScore;

			if (currentScore > _gameProgress.MaxFilledScore)
				return false;

			int score = Mathf.Clamp(newScore, 0, _gameProgress.MaxFilledScore);

			_gameProgress.AddSand(score);

			ScoreChanged?.Invoke(_gameProgress.CurrentSandCount);
			return true;
		}

		public void SellSand()
		{
			if (_gameProgress.CurrentSandCount <= 0)
				return;

			if (_gameProgress.CurrentSandCount <= 0)
				return;

			_gameProgress.AddMoney(1);
			_gameProgress.DecreaseSand(1);
			ScoreChanged?.Invoke(_gameProgress.CurrentSandCount);
			MoneyChanged?.Invoke(_gameProgress.CurrentMoney);
		}
	}
}