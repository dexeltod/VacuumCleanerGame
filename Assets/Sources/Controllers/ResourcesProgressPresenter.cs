using System;
using Sources.Controllers.Common;
using Sources.ControllersInterfaces;
using Sources.DomainInterfaces;
using Sources.DomainInterfaces.DomainServicesInterfaces;
using Sources.InfrastructureInterfaces;
using Sources.InfrastructureInterfaces.Providers;
using Sources.Services;
using Sources.ServicesInterfaces;
using UnityEngine;

namespace Sources.Controllers
{
	public class ResourcesProgressPresenter : Presenter, IResourcesProgressPresenter
	{
		private readonly IResourcesModel _resourcesData;
		private readonly ISandContainerViewProvider _sandContainerViewProvider;
		private readonly IFillMeshShaderControllerProvider _fillMeshShaderControllerProvider;
		private readonly SandParticlePlayerSystem _sandParticlePlayerSystem;
		private readonly IGameplayInterfacePresenterProvider _gameplayInterfacePresenter;
		private readonly IGameProgress _upgradeProgressModel;
		private readonly IPlayerStatsServiceProvider _playerStatsServiceProvider;
		private readonly IPlayerStatsNames _playerStatsNames;

		private int _increasedDelta;
		private int _lastCashScore;

		public ResourcesProgressPresenter(
			IGameplayInterfacePresenterProvider gameplayInterfaceView,
			IResourcesModel persistentProgressService,
			IFillMeshShaderControllerProvider fillMeshShaderControllerProvider,
			SandParticlePlayerSystem sandParticlePlayerSystem,
			IGameProgress upgradeProgressModel,
			IPlayerStatsServiceProvider playerStatsServiceProvider,
			IPlayerStatsNames playerStatsNames
		)
		{
			_upgradeProgressModel
				= upgradeProgressModel ?? throw new ArgumentNullException(nameof(upgradeProgressModel));
			_playerStatsServiceProvider = playerStatsServiceProvider ??
				throw new ArgumentNullException(nameof(playerStatsServiceProvider));
			_playerStatsNames = playerStatsNames;
			_gameplayInterfacePresenter
				= gameplayInterfaceView ?? throw new ArgumentNullException(nameof(gameplayInterfaceView));
			_resourcesData = persistentProgressService ??
				throw new ArgumentNullException(nameof(persistentProgressService));
			_fillMeshShaderControllerProvider = fillMeshShaderControllerProvider ??
				throw new ArgumentNullException(nameof(fillMeshShaderControllerProvider));
			_sandParticlePlayerSystem = sandParticlePlayerSystem ??
				throw new ArgumentNullException(nameof(sandParticlePlayerSystem));
		}

		public IResourceReadOnly<int> SoftCurrency => _resourcesData.SoftCurrency;
		private IFillMeshShaderController FillMeshShaderController => _fillMeshShaderControllerProvider.Implementation;
		private IGameplayInterfacePresenter GameplayInterfacePresenter => _gameplayInterfacePresenter.Implementation;
		private IPlayerStatsService PlayerStats => _playerStatsServiceProvider.Implementation;

		private int MaxScoreCash =>
			_playerStatsServiceProvider.Implementation.Get(
				_playerStatsNames
					.MaxScoreCash
			).Value;

		private int CurrentScore => _resourcesData.CurrentCashScore;

		private bool IsHalfGlobalScore => IsHalfScoreReached();

		public bool IsMaxScoreReached => CheckMaxScore();

		public override void Enable() =>
			SetEnableSand();

		public override void Disable() =>
			SetEnableSand();

		public int GetDecreasedMoney(int count)
		{
			if (count <= 0)
				throw new ArgumentOutOfRangeException(nameof(count));

			return _resourcesData.SoftCurrency.Count - count;
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

			SetMoneyTextView();
			SetView();
		}

		public void AddMoney(int count)
		{
			_resourcesData.AddMoney(count);
			SetMoneyTextView();
		}

		public void DecreaseMoney(int count)
		{
			if (_resourcesData.SoftCurrency.Count - count < 0)
				throw new ArgumentOutOfRangeException($"{SoftCurrency} less than zero");

			_resourcesData.DecreaseMoney(count);
			SetMoneyTextView();
		}

		private void SetEnableSand() =>
			_fillMeshShaderControllerProvider.Implementation.FillArea(
				CurrentScore,
				0,
				PlayerStats.Get
					(_playerStatsNames.ScoreCash).Value
			);

		public void ClearTotalResources()
		{
			_resourcesData.ClearTotalResources();
			SetView();
		}

		public bool TryAddSand(int newScore)
		{
			if (newScore <= 0) throw new ArgumentOutOfRangeException(nameof(newScore));

			if (CurrentScore > _resourcesData.MaxCashScore)
				return false;

			_lastCashScore = _resourcesData.CurrentCashScore;

			int score = Mathf.Clamp(newScore, 0, MaxScoreCash);
			_resourcesData.AddScore(score);

			PlayParticleSystem();
			SetView();

			OnHalfScoreReached();

			return true;
		}

		private void PlayParticleSystem()
		{
			if (_lastCashScore < _resourcesData.CurrentCashScore)
				_sandParticlePlayerSystem.Play();
		}

		private bool CheckMaxScore() =>
			_resourcesData.CurrentCashScore < _resourcesData.MaxCashScore;

		private void OnHalfScoreReached()
		{
			if (IsHalfGlobalScore == true)
				GameplayInterfacePresenter.SetActiveGoToNextLevelButton(true);
		}

		private void SetMoneyTextView()
		{
			GameplayInterfacePresenter.SetSoftCurrency(_resourcesData.SoftCurrency.Count);
		}

		private void SetView()
		{
			GameplayInterfacePresenter.SetTotalResourceCount(_resourcesData.CurrentTotalResources);
			GameplayInterfacePresenter.SetCashScore(_resourcesData.CurrentCashScore);
			FillMeshShaderController.FillArea(CurrentScore, 0, _resourcesData.MaxCashScore);
		}

		private bool IsHalfScoreReached()
		{
			int halfScore = Mathf.CeilToInt(_resourcesData.MaxTotalResourceCount / 2f);

			return _resourcesData.CurrentTotalResources >= halfScore;
		}
	}
}