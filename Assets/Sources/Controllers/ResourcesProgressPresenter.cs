using System;
using Sources.DomainInterfaces;
using Sources.DomainInterfaces.DomainServicesInterfaces;
using Sources.PresentationInterfaces;
using Sources.ServicesInterfaces;
using UnityEngine;

namespace Sources.Controllers
{
	public class ResourcesProgressPresenter : IResourcesProgressPresenter, IResourceProgressEventHandler
	{
		private readonly IResourcesModel _resourcesData;
		private readonly IGameplayInterfaceView _gameplayInterfaceView;

		private int _increasedDelta;

		public IResourceReadOnly<int> SoftCurrency => _resourcesData.SoftCurrency;

		public int GlobalScore { get; private set; }

		private bool _isHalfScoreAlreadyReached = false;

		public bool IsMaxScoreReached => CheckMaxScore();

		public event Action<int> SoftCurrencyChanged;
		public event Action<int> CashScoreChanged;
		public event Action<int> GlobalScoreChanged;
		public event Action<int> MaxGlobalScoreChanged;
		public event Action<int> MaxCashScoreChanged;
		public event Action<bool> HalfGlobalScoreReached;

		public ResourcesProgressPresenter(
			IResourcesModel persistentProgressService
		) =>
			_resourcesData = persistentProgressService ??
				throw new ArgumentNullException(nameof(persistentProgressService));

		public void ClearScores()
		{
			_resourcesData.ClearScores();
			OnGlobalScoreChanged();
			OnCashScoreChanged();
		}

		public bool CheckMaxScore() =>
			_resourcesData.CurrentCashScore < _resourcesData.MaxCashScore;

		public bool TryAddSand(int newScore)
		{
			int currentScore = _resourcesData.CurrentCashScore;

			if (currentScore > _resourcesData.MaxCashScore)
				return false;

			int score = Mathf.Clamp(newScore, 0, _resourcesData.MaxCashScore);

			int lastCashScore = _resourcesData.CurrentCashScore;
			_resourcesData.AddCashScore(score);

			if (lastCashScore != _resourcesData.CurrentCashScore)
				OnCashScoreChanged();

			if (GlobalScore != _resourcesData.GlobalSandCount)
			{
				GlobalScore = _resourcesData.GlobalSandCount;
				OnGlobalScoreChanged();
			}

			OnHalfScoreReached();

			return true;
		}

		private void OnGlobalScoreChanged() =>
			GlobalScoreChanged.Invoke(_resourcesData.GlobalSandCount);

		private void OnHalfScoreReached()
		{
			if (IsHalfScoreReached() == true && !_isHalfScoreAlreadyReached)
				HalfGlobalScoreReached(true);
		}

		public void SellSand()
		{
			if (_resourcesData.CurrentCashScore <= 0)
				return;

			_increasedDelta++;

			if (_resourcesData.CurrentCashScore - _increasedDelta < 0)
			{
				_increasedDelta = 0;
				return;
			}

			_resourcesData.AddMoney(_increasedDelta);
			_resourcesData.DecreaseCashScore(_increasedDelta);

			OnMoneyChanged();
			OnCashScoreChanged();
		}

		public void AddMoney(int count)
		{
			_resourcesData.AddMoney(count);
			OnMoneyChanged();
		}

		public void DecreaseMoney(int count)
		{
			if (_resourcesData.SoftCurrency.Count - count < 0)
				throw new ArgumentOutOfRangeException($"{SoftCurrency} less than zero");

			_resourcesData.DecreaseMoney(count);
			OnMoneyChanged();
		}

		public int GetDecreasedMoney(int count)
		{
			if (count <= 0)
				throw new ArgumentOutOfRangeException(nameof(count));

			return _resourcesData.SoftCurrency.Count - count;
		}

		private void OnMoneyChanged() =>
			SoftCurrencyChanged.Invoke(SoftCurrency.Count);

		private void OnCashScoreChanged() =>
			CashScoreChanged.Invoke(_resourcesData.CurrentCashScore);

		private bool IsHalfScoreReached()
		{
			int halfScore = _resourcesData.MaxCashScore / 2;

			return _resourcesData.Score.Count >= halfScore;
		}
	}
}