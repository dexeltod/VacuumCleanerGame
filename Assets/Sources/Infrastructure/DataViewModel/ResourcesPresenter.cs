using System;
using Sources.DIService;
using Sources.Domain.Progress.Player;
using Sources.DomainInterfaces;
using Sources.DomainInterfaces.DomainServicesInterfaces;
using Sources.ServicesInterfaces;
using UnityEngine;

namespace Sources.Infrastructure.DataViewModel
{
	public class ResourcesPresenter : IResourcesProgressPresenter
	{
		private const int Count = 1;
		private readonly IPersistentProgressService _resourcesData;

		public IResource<int> SoftCurrency => Data.SoftCurrency;

		public event Action<int> ScoreChanged;
		public event Action<int> MoneyChanged;

		private IResourcesData Data => _resourcesData.GameProgress.ResourcesData;

		public ResourcesPresenter()
		{
			_resourcesData = GameServices.Container.Get<IPersistentProgressService>();
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

			Data.AddMoney(Count);
			Data.DecreaseSand(Count);
			ScoreChanged?.Invoke(Data.CurrentSandCount);
			MoneyChanged?.Invoke(Data.SoftCurrency.Count);
		}

		public void AddMoney(int count)
		{
			Data.AddMoney(count);
			MoneyChanged?.Invoke(Data.SoftCurrency.Count);
		}

		public void DecreaseMoney(int count)
		{
			Data.DecreaseMoney(count);
			MoneyChanged?.Invoke(Data.SoftCurrency.Count);
		}
	}
}