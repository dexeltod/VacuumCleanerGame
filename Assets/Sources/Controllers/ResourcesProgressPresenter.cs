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
		private readonly IResourceModelModifiable _resourceData;
		private readonly ISandContainerViewProvider _sandContainerViewProvider;
		private readonly IFillMeshShaderControllerProvider _fillMeshShaderControllerProvider;
		private readonly SandParticlePlayerSystem _sandParticlePlayerSystem;
		private readonly IGameplayInterfacePresenterProvider _gameplayInterfacePresenter;
		private readonly IResourceModelReadOnly _resourceReadOnly;

		private int _increasedDelta;
		private int _lastCashScore;

		public ResourcesProgressPresenter(
			IGameplayInterfacePresenterProvider gameplayInterfaceView,
			IResourceModelReadOnly resourceReadOnly,
			IResourceModelModifiable persistentProgressService,
			IFillMeshShaderControllerProvider fillMeshShaderControllerProvider,
			SandParticlePlayerSystem sandParticlePlayerSystem
		)
		{
			_gameplayInterfacePresenter
				= gameplayInterfaceView ?? throw new ArgumentNullException(nameof(gameplayInterfaceView));
			_resourceReadOnly = resourceReadOnly ?? throw new ArgumentNullException(nameof(resourceReadOnly));
			_resourceData = persistentProgressService ??
				throw new ArgumentNullException(nameof(persistentProgressService));
			_fillMeshShaderControllerProvider = fillMeshShaderControllerProvider ??
				throw new ArgumentNullException(nameof(fillMeshShaderControllerProvider));
			_sandParticlePlayerSystem = sandParticlePlayerSystem ??
				throw new ArgumentNullException(nameof(sandParticlePlayerSystem));
		}

		public IReadOnlyProgressValue<int> SoftCurrency => _resourceReadOnly.SoftCurrency;
		private IFillMeshShaderController FillMeshShaderController => _fillMeshShaderControllerProvider.Implementation;
		private IGameplayInterfacePresenter GameplayInterfacePresenter => _gameplayInterfacePresenter.Implementation;

		private int CurrentScore => _resourceReadOnly.CurrentCashScore;

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

			return _resourceReadOnly.SoftCurrency.Value - count;
		}

		public void Sell()
		{
			if (_resourceReadOnly.CurrentCashScore <= 0)
				return;

			_increasedDelta++;

			if (_resourceReadOnly.CurrentCashScore - _increasedDelta < 0)
			{
				_increasedDelta = 0;
				return;
			}

			_resourceData.AddMoney(_increasedDelta);
			_resourceData.DecreaseCashScore(_increasedDelta);

			SetMoneyTextView();
			SetView();
		}

		public void AddMoney(int count)
		{
			_resourceData.AddMoney(count);
			SetMoneyTextView();
		}

		public void DecreaseMoney(int count)
		{
			if (_resourceReadOnly.SoftCurrency.Value - count < 0)
				throw new ArgumentOutOfRangeException($"{SoftCurrency} less than zero");

			_resourceData.TryDecreaseMoney(count);
			SetMoneyTextView();
		}

		private void SetEnableSand() =>
			_fillMeshShaderControllerProvider.Implementation.FillArea(
				CurrentScore,
				0,
				_resourceReadOnly.MaxCashScore
			);

		public void ClearTotalResources()
		{
			_resourceData.ClearTotalResources();
			SetView();
		}

		public bool TryAddSand(int newScore)
		{
			if (newScore <= 0) throw new ArgumentOutOfRangeException(nameof(newScore));

			if (CurrentScore > _resourceReadOnly.MaxCashScore)
				return false;

			_lastCashScore = _resourceReadOnly.CurrentCashScore;

			int score = Mathf.Clamp(newScore, 0, _resourceReadOnly.MaxCashScore);
			_resourceData.AddScore(score);

			PlayParticleSystem();
			SetView();

			OnHalfScoreReached();

			return true;
		}

		private void PlayParticleSystem()
		{
			if (_lastCashScore < _resourceReadOnly.CurrentCashScore)
				_sandParticlePlayerSystem.Play();
		}

		private bool CheckMaxScore() =>
			_resourceReadOnly.CurrentCashScore < _resourceReadOnly.MaxCashScore;

		private void OnHalfScoreReached()
		{
			if (IsHalfGlobalScore == true)
				GameplayInterfacePresenter.SetActiveGoToNextLevelButton(true);
		}

		private void SetMoneyTextView() =>
			GameplayInterfacePresenter.SetSoftCurrency(_resourceReadOnly.SoftCurrency.Value);

		private void SetView()
		{
			GameplayInterfacePresenter.SetTotalResourceCount(_resourceReadOnly.CurrentTotalResources);
			GameplayInterfacePresenter.SetCashScore(_resourceReadOnly.CurrentCashScore);
			FillMeshShaderController.FillArea(CurrentScore, 0, _resourceReadOnly.MaxCashScore);
		}

		private bool IsHalfScoreReached() =>
			_resourceReadOnly.CurrentTotalResources >= Mathf.CeilToInt(_resourceReadOnly.MaxTotalResourceCount / 2f);
	}
}