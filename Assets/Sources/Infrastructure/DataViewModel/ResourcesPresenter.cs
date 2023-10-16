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

		public IResource<int> SoftCurrency => ResourcesModel.SoftCurrency;

		public int GlobalScore { get; private set; }

		public event Action<int> ScoreChanged;
		public event Action<int> MoneyChanged;
		public event Action      HalfGlobalScoreReached;
		public event Action      GlobalScoreChanged;

		private IResourcesModel ResourcesModel =>
			_resourcesData
				.GameProgress
				.ResourcesModel;

		public ResourcesPresenter() =>
			_resourcesData = GameServices.Container.Get<IPersistentProgressService>();

		public bool CheckMaxScore() =>
			ResourcesModel.CurrentSandCount < ResourcesModel.MaxScore;

		public bool TryAddSand(int newScore)
		{
			int currentScore = ResourcesModel.CurrentSandCount;

			if (currentScore > ResourcesModel.MaxScore)
				return false;

			int score = Mathf.Clamp(newScore, 0, ResourcesModel.MaxScore);

			ResourcesModel.AddSand(score);

			ScoreChanged?.Invoke(ResourcesModel.CurrentSandCount);

			GlobalScore = ResourcesModel.GlobalSandCount;
			GlobalScoreChanged?.Invoke();

			if (IsHalfScoreReached() == true)
				HalfGlobalScoreReached.Invoke();

			return true;
		}

		public void SellSand()
		{
			if (ResourcesModel.CurrentSandCount <= 0)
				return;

			_increasedDelta++;

			if (ResourcesModel.CurrentSandCount - _increasedDelta < 0)
			{
				_increasedDelta = 0;
				return;
			}

			ResourcesModel.AddMoney(_increasedDelta);
			ResourcesModel.DecreaseSand(_increasedDelta);
			ScoreChanged?.Invoke(ResourcesModel.CurrentSandCount);
			MoneyChanged?.Invoke(SoftCurrency.Count);
		}

		public void AddMoney(int count)
		{
			ResourcesModel.AddMoney(count);
			MoneyChanged?.Invoke(SoftCurrency.Count);
		}

		public void DecreaseMoney(int count)
		{
			if (ResourcesModel.SoftCurrency.Count - count < 0)
				throw new ArgumentOutOfRangeException($"{SoftCurrency} less than zero");

			ResourcesModel.SoftCurrency.Set(SoftCurrency.Count - count);
			MoneyChanged?.Invoke(SoftCurrency.Count);
		}

		public int GetDecreasedMoney(int count) =>
			ResourcesModel.SoftCurrency.Count - count;

		private bool IsHalfScoreReached()
		{
			int halfScore = ResourcesModel.MaxScore / 2;

			return ResourcesModel.Score.Count >= halfScore;
		}
	}
}