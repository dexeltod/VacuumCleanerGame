using System;
using Model.Data.Player;
using Model.DI;
using UnityEngine;

namespace ViewModel.Infrastructure.Services.DataViewModel
{
	public class ResourcesProgressViewModel : IResourcesProgressViewModel
	{
		private readonly IPersistentProgressService _resourcesData;

		public int Money => Data.Money;

		public event Action<int> ScoreChanged;
		public event Action<int> MoneyChanged;

		private ResourcesData Data => _resourcesData.GameProgress.ResourcesData;

		public ResourcesProgressViewModel()
		{
			_resourcesData = ServiceLocator.Container.GetSingle<IPersistentProgressService>();
		}

		public bool CheckMaxScore()
		{
			if (Data.CurrentSandCount >= Data.MaxFilledScore)
				return false;

			return true;
		}

		public bool AddSand(int newScore)
		{
			int currentScore = Data.CurrentSandCount;
			currentScore += newScore;

			if (currentScore > Data.MaxFilledScore)
				return false;

			int score = Mathf.Clamp(newScore, 0, Data.MaxFilledScore);

			Data.AddSand(score);

			ScoreChanged?.Invoke(Data.CurrentSandCount);
			return true;
		}

		public void SellSand()
		{
			if (Data.CurrentSandCount <= 0)
				return;

			if (Data.CurrentSandCount <= 0)
				return;

			Data.AddMoney(1);
			Data.DecreaseSand(1);
			ScoreChanged?.Invoke(Data.CurrentSandCount);
			MoneyChanged?.Invoke(Data.Money);
		}

		public void AddMoney(int count)
		{
			Data.AddMoney(count);
			MoneyChanged?.Invoke(Data.Money);
		}

		public void DecreaseMoney(int count)
		{
			Data.DecreaseMoney(count);
			MoneyChanged?.Invoke(Data.Money);
		}
	}
}