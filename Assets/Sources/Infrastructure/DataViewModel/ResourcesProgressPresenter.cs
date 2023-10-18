using System;
using Sources.DIService;
using Sources.DomainInterfaces;
using Sources.DomainInterfaces.DomainServicesInterfaces;
using Sources.PresentationInterfaces;
using Sources.ServicesInterfaces;
using UnityEngine;

namespace Sources.Infrastructure.DataViewModel
{
	public class ResourcesProgressPresenter : IResourcesProgressPresenter
	{
		private readonly IResourcesModel        _resourcesData;
		private readonly IGameplayInterfaceView _gameplayInterfaceView;

		private int _increasedDelta;

		public IResource<int> SoftCurrency => ResourcesModel.SoftCurrency;

		public int GlobalScore { get; private set; }

		private bool _isHalfScoreAlreadyReached = false;

		public IResourcesModel ResourcesModel => _resourcesData;

		public ResourcesProgressPresenter
		(
			IResourcesModel        persistentProgressService,
			IGameplayInterfaceView gameplayInterfaceView
		)
		{
			_resourcesData         = persistentProgressService;
			_gameplayInterfaceView = gameplayInterfaceView;
		}

		public bool CheckMaxScore() =>
			ResourcesModel.CurrentSandCount < ResourcesModel.MaxCashScore;

		public bool TryAddSand(int newScore)
		{
			int currentScore = ResourcesModel.CurrentSandCount;

			if (currentScore > ResourcesModel.MaxCashScore)
				return false;

			int score = Mathf.Clamp(newScore, 0, ResourcesModel.MaxCashScore);

			ResourcesModel.AddSand(score);

			_gameplayInterfaceView.SetScore(ResourcesModel.CurrentSandCount);

			GlobalScore = ResourcesModel.GlobalSandCount;

			_gameplayInterfaceView.SetGlobalScore(GlobalScore);

			if (IsHalfScoreReached() == true && !_isHalfScoreAlreadyReached)
			{
				_gameplayInterfaceView.SetActiveGoToNextLevelButton(true);
				_isHalfScoreAlreadyReached = true;
			}

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

			_gameplayInterfaceView.SetScore(ResourcesModel.CurrentSandCount);
			_gameplayInterfaceView.SetMoney(SoftCurrency.Count);
		}

		public void AddMoney(int count)
		{
			ResourcesModel.AddMoney(count);
			SetMoneyInPresentation();
		}

		public void DecreaseMoney(int count)
		{
			if (ResourcesModel.SoftCurrency.Count - count < 0)
				throw new ArgumentOutOfRangeException($"{SoftCurrency} less than zero");

			ResourcesModel.SoftCurrency.Set(SoftCurrency.Count - count);
			SetMoneyInPresentation();
		}

		public int GetDecreasedMoney(int count) =>
			ResourcesModel.SoftCurrency.Count - count;

		private void SetMoneyInPresentation() =>
			_gameplayInterfaceView.SetMoney(SoftCurrency.Count);

		private bool IsHalfScoreReached()
		{
			int halfScore = ResourcesModel.MaxCashScore / 2;

			return ResourcesModel.Score.Count >= halfScore;
		}
	}
}