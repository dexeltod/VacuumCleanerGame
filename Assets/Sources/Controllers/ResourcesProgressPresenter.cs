using System;
using Sources.Controllers.Common;
using Sources.DomainInterfaces;
using Sources.DomainInterfaces.DomainServicesInterfaces;
using Sources.InfrastructureInterfaces.Presenters;
using Sources.PresentationInterfaces;
using Sources.ServicesInterfaces;
using UnityEngine;

namespace Sources.Controllers
{
	public class ResourcesProgressPresenter : Presenter, IResourcesProgressPresenter, IResourceProgressEventHandler
	{
		private readonly IResourcesModel _resourcesData;
		private readonly IGameplayInterfaceView _gameplayInterfaceView;

		private int _increasedDelta;

		public IResourceReadOnly<int> SoftCurrency => _resourcesData.SoftCurrency;

		public int GlobalScore { get; private set; }

		private int CurrentScore => _resourcesData.CurrentCashScore;
		private bool IsHalfScore => IsHalfScoreReached();
		public bool IsMaxScoreReached => CheckMaxScore();

		public event Action<int> CashScoreChanged;

		public ResourcesProgressPresenter(
			IGameplayInterfaceView gameplayInterfaceView,
			IResourcesModel persistentProgressService
		)
		{
			_gameplayInterfaceView
				= gameplayInterfaceView ?? throw new ArgumentNullException(nameof(gameplayInterfaceView));
			_resourcesData = persistentProgressService ??
				throw new ArgumentNullException(nameof(persistentProgressService));
		}

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
			if (CurrentScore > _resourcesData.MaxCashScore)
				return false;

			int score = Mathf.Clamp(newScore, 0, _resourcesData.MaxCashScore);

			int lastCashScore = AddCashScoreInData(score);

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

		private int AddCashScoreInData(int score)
		{
			int lastCashScore = _resourcesData.CurrentCashScore;
			_resourcesData.AddCashScore(score);
			return lastCashScore;
		}

		private void OnGlobalScoreChanged() =>
			_gameplayInterfaceView.SetGlobalScore(GlobalScore);

		private void OnHalfScoreReached()
		{
			if (IsHalfScore == true)
				_gameplayInterfaceView.SetActiveGoToNextLevelButton(true);
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
			_gameplayInterfaceView.SetSoftCurrency(SoftCurrency.Count);

		private void OnCashScoreChanged()
		{
			_gameplayInterfaceView.SetCashScore(_resourcesData.CurrentCashScore);
			CashScoreChanged!.Invoke(_resourcesData.CurrentCashScore);
		}

		private bool IsHalfScoreReached()
		{
			int halfScore = _resourcesData.MaxCashScore / 2;

			return _resourcesData.Score.Count >= halfScore;
		}
	}
}