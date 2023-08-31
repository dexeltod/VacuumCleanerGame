using System;
using Sources.DIService;
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

		public IResource<int> SoftCurrency => Model.SoftCurrency;

		public event Action<int> ScoreChanged;
		public event Action<int> MoneyChanged;

		private IResourcesModel Model => _resourcesData.GameProgress.ResourcesModel;

		public ResourcesPresenter()
		{
			_resourcesData = GameServices.Container.Get<IPersistentProgressService>();
		}

		public bool CheckMaxScore()
		{
			if (Model.CurrentSandCount >= Model.MaxFilledScore)
				return false;

			return true;
		}

		public bool AddSand(int newScore)
		{
			int currentScore = Model.CurrentSandCount;
			currentScore += newScore;

			if (currentScore > Model.MaxFilledScore)
				return false;

			int score = Mathf.Clamp(newScore, 0, Model.MaxFilledScore);

			Model.AddSand(score);

			ScoreChanged?.Invoke(Model.CurrentSandCount);
			return true;
		}

		public void SellSand()
		{
			if (Model.CurrentSandCount <= 0)
				return;

			if (Model.CurrentSandCount <= 0)
				return;

			Model.AddMoney(Count);
			Model.DecreaseSand(Count);
			ScoreChanged?.Invoke(Model.CurrentSandCount);
			MoneyChanged?.Invoke(Model.SoftCurrency.Count);
		}

		public void AddMoney(int count)
		{
			Model.AddMoney(count);
			MoneyChanged?.Invoke(Model.SoftCurrency.Count);
		}

		public void DecreaseMoney(int count)
		{
			Model.SoftCurrency.Set(Model.SoftCurrency.Count - count);
			MoneyChanged?.Invoke(Model.SoftCurrency.Count);
		}
	}
}