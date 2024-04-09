using System;
using Sources.Controllers.Common;
using Sources.ControllersInterfaces;
using Sources.DomainInterfaces;
using Sources.DomainInterfaces.DomainServicesInterfaces;
using Sources.InfrastructureInterfaces.Providers;
using Sources.Services;
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

		private int _increasedDelta;
		private int _lastCashScore;

		public ResourcesProgressPresenter(
			IGameplayInterfacePresenterProvider gameplayInterfaceView,
			IResourcesModel persistentProgressService,
			IFillMeshShaderControllerProvider fillMeshShaderControllerProvider,
			SandParticlePlayerSystem sandParticlePlayerSystem
		)
		{
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

		public void DecreaseMoney(int count)
		{
			if (_resourcesData.SoftCurrency.Count - count < 0)
				throw new ArgumentOutOfRangeException($"{SoftCurrency} less than zero");

			_resourcesData.DecreaseMoney(count);
			SetMoneyTextView();
		}

		private void SetEnableSand() =>
			_fillMeshShaderControllerProvider.Implementation.FillArea(CurrentScore, 0, _resourcesData.MaxCashScore);

		public void ClearScores()
		{
			_resourcesData.ClearScores();
			SetView();
		}

		private bool CheckMaxScore() =>
			_resourcesData.CurrentCashScore < _resourcesData.MaxCashScore;

		public bool TryAddSand(int newScore)
		{
			if (newScore <= 0) throw new ArgumentOutOfRangeException(nameof(newScore));

			if (CurrentScore > _resourcesData.MaxCashScore)
				return false;

			int score = Mathf.Clamp(newScore, 0, _resourcesData.MaxCashScore);

			_lastCashScore = _resourcesData.CurrentCashScore;

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

		private void OnHalfScoreReached()
		{
			if (IsHalfGlobalScore == true)
				GameplayInterfacePresenter.SetActiveGoToNextLevelButton(true);
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
			//TODO: When the user will be watching video
			_resourcesData.AddMoney(count);
			SetMoneyTextView();
		}

		private void SetMoneyTextView()
		{
			GameplayInterfacePresenter.SetSoftCurrency(_resourcesData.SoftCurrency.Count);
		}

		private void SetView()
		{
			GameplayInterfacePresenter.SetGlobalScore(_resourcesData.CurrentTotalResources);
			GameplayInterfacePresenter.SetCashScore(_resourcesData.CurrentCashScore);
			FillMeshShaderController.FillArea(CurrentScore, 0, _resourcesData.MaxCashScore);
		}

		private bool IsHalfScoreReached()
		{
			int halfScore = Mathf.CeilToInt(_resourcesData.MaxGlobalScore / 2f);

			return _resourcesData.CurrentTotalResources >= halfScore;
		}
	}
}