using System;
using Sources.Controllers.Common;
using Sources.ControllersInterfaces;
using Sources.DomainInterfaces;
using Sources.DomainInterfaces.DomainServicesInterfaces;
using Sources.InfrastructureInterfaces.Providers;
using Sources.PresentationInterfaces;
using Sources.ServicesInterfaces;
using UnityEngine;

namespace Sources.Controllers
{
	public class ResourcesProgressPresenter : Presenter, IResourcesProgressPresenter
	{
		private const float NormalizeThreshold = 1;

		private readonly IResourcesModel _resourcesData;
		private readonly ISandContainerViewProvider _sandContainerViewProvider;
		private readonly IGameplayInterfaceProvider _gameplayInterfaceView;

		private int _increasedDelta;

		public IResourceReadOnly<int> SoftCurrency => _resourcesData.SoftCurrency;

		public IGameplayInterfaceView GameplayInterface => _gameplayInterfaceView.GetContract<IGameplayInterfaceView>();
		public ISandContainerView SandContainerView => _sandContainerViewProvider.Implementation;

		public int GlobalScore { get; private set; }

		private int CurrentScore => _resourcesData.CurrentCashScore;
		private bool IsHalfScore => IsHalfScoreReached();
		public bool IsMaxScoreReached => CheckMaxScore();

		public ResourcesProgressPresenter(
			IGameplayInterfaceProvider gameplayInterfaceView,
			IResourcesModel persistentProgressService,
			ISandContainerViewProvider sandCarContainerViewProvider
		)
		{
			_gameplayInterfaceView
				= gameplayInterfaceView ?? throw new ArgumentNullException(nameof(gameplayInterfaceView));
			_resourcesData = persistentProgressService ??
				throw new ArgumentNullException(nameof(persistentProgressService));
			_sandContainerViewProvider = sandCarContainerViewProvider ??
				throw new ArgumentNullException(nameof(sandCarContainerViewProvider));
		}

		public override void Enable()
		{
			base.Enable();
			SetEnableSand();
		}

		public override void Disable()
		{
			base.Disable();
			SetEnableSand();
		}

		private void SetEnableSand() =>
			_sandContainerViewProvider.Implementation.SetEnableSand(_resourcesData.CurrentCashScore > 0);

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
			GameplayInterface.SetGlobalScore(GlobalScore);

		private void OnHalfScoreReached()
		{
			if (IsHalfScore == true)
				GameplayInterface.SetActiveGoToNextLevelButton(true);
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
			GameplayInterface.SetSoftCurrency(SoftCurrency.Count);

		private void OnCashScoreChanged()
		{
			GameplayInterface.SetCashScore(_resourcesData.CurrentCashScore);
			SandContainerView.SetSand(NormalizeCashScore());
		}

		private bool IsHalfScoreReached()
		{
			int halfScore = _resourcesData.MaxGlobalScore / 2;

			return _resourcesData.Score.Count >= halfScore;
		}

		private float NormalizeCashScore() =>
			NormalizeThreshold / _resourcesData.MaxCashScore * _resourcesData.CurrentCashScore;
	}
}