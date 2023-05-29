using System;
using Model.DI;
using Model.Infrastructure.Data;
using UnityEngine;
using ViewModel.Infrastructure.Services;

namespace ViewModel.Infrastructure
{
	public class PlayerProgressViewModel : IPlayerProgressViewModel
	{
		private readonly PlayerProgress _playerProgress;

		public int Money => _playerProgress.Money;

		public event Action<int> ScoreChanged;
		public event Action<int> MoneyChanged;

		public PlayerProgressViewModel()
		{
			_playerProgress = ServiceLocator.Container.GetSingle<IPersistentProgressService>().GameProgress
				.PlayerProgress;
		}

		public bool CheckMaxScore()
		{
			if (_playerProgress.CurrentSandCount >= _playerProgress.MaxFilledScore)
				return false;

			return true;
		}

		public bool AddSand(int newScore)
		{
			int currentScore = _playerProgress.CurrentSandCount;
			currentScore += newScore;

			if (currentScore > _playerProgress.MaxFilledScore)
				return false;

			int score = Mathf.Clamp(newScore, 0, _playerProgress.MaxFilledScore);

			_playerProgress.AddSand(score);

			ScoreChanged?.Invoke(_playerProgress.CurrentSandCount);
			return true;
		}

		public void SellSand()
		{
			if (_playerProgress.CurrentSandCount <= 0)
				return;

			if (_playerProgress.CurrentSandCount <= 0)
				return;

			_playerProgress.AddMoney(1);
			_playerProgress.DecreaseSand(1);
			ScoreChanged?.Invoke(_playerProgress.CurrentSandCount);
			MoneyChanged?.Invoke(_playerProgress.Money);
		}

		public void AddMoney(int count)
		{
			_playerProgress.AddMoney(count);
			MoneyChanged?.Invoke(_playerProgress.Money);
		}

		public void DecreaseMoney(int count)
		{
			_playerProgress.DecreaseMoney(count);
			MoneyChanged?.Invoke(_playerProgress.Money);
		}
	}
}