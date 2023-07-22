using System;
using Sources.Core.DI;
using Sources.Core.Domain.DomainInterfaces.DomainServicesInterfaces;
using Sources.Core.Domain.Progress.Player;
using Sources.DomainServices.Interfaces;
using Sources.Infrastructure.InfrastructureInterfaces;
using UnityEngine;

namespace Sources.Infrastructure.DataViewModel
{
	public class ResourcesViewModel : IResourcesProgressViewModel
	{
		private readonly IPersistentProgressService _resourcesData;

		public IResource<int> SoftCurrency => Data.SoftCurrency;

		public event Action<int> ScoreChanged;
		public event Action<int> MoneyChanged;

		private ResourcesData Data => _resourcesData.GameProgress.ResourcesData;

		public ResourcesViewModel()
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