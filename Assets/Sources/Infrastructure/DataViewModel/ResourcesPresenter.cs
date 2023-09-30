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
		private readonly IPersistentProgressService _resourcesData;

		private int _increasedDelta;

		public IResource<int> SoftCurrency => Model.SoftCurrency;

		public event Action<int> ScoreChanged;
		public event Action<int> MoneyChanged;
		public event Action<int> GlobalScoreChanged;

		private IResourcesModel Model =>
			_resourcesData
				.GameProgress
				.ResourcesModel;

		public ResourcesPresenter() =>
			_resourcesData = GameServices.Container.Get<IPersistentProgressService>();

		public bool CheckMaxScore() =>
			Model.CurrentSandCount < Model.MaxFilledScore;

		public bool TryAddSand(int newScore)
		{
			int currentScore = Model.CurrentSandCount;

			if (currentScore > Model.MaxFilledScore)
				return false;

			int score = Mathf.Clamp(newScore, 0, Model.MaxFilledScore);

			Model.AddSand(score);

			ScoreChanged?.Invoke(Model.CurrentSandCount);
			GlobalScoreChanged.Invoke(Model.GlobalSandCount);
			return true;
		}

		public void SellSand()
		{
			if (Model.CurrentSandCount <= 0)
				return;

			_increasedDelta++;

			if (Model.CurrentSandCount - _increasedDelta < 0)
			{
				_increasedDelta = 0;
				return;
			}

			Model.AddMoney(_increasedDelta);
			Model.DecreaseSand(_increasedDelta);
			ScoreChanged?.Invoke(Model.CurrentSandCount);
			MoneyChanged?.Invoke(SoftCurrency.Count);
		}

		public void AddMoney(int count)
		{
			Model.AddMoney(count);
			MoneyChanged?.Invoke(SoftCurrency.Count);
		}

		public void DecreaseMoney(int count)
		{
			if (Model.SoftCurrency.Count - count < 0)
				throw new ArgumentOutOfRangeException($"{SoftCurrency} less than zero");

			Model.SoftCurrency.Set(SoftCurrency.Count - count);
			MoneyChanged?.Invoke(SoftCurrency.Count);
		}

		public int GetDecreasedMoney(int count) =>
			Model.SoftCurrency.Count - count;
	}
}