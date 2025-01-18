using System;
using Sources.BusinessLogic.Providers;
using Sources.BusinessLogic.Services;
using Sources.Controllers.Common;
using Sources.ControllersInterfaces;
using Sources.DomainInterfaces;
using Sources.DomainInterfaces.DomainServicesInterfaces;
using Sources.DomainInterfaces.Entities;
using UnityEngine;

namespace Sources.Controllers
{
	public class ResourcesProgressPresenter : Presenter, IResourcesProgressPresenter
	{
		private readonly IResourceModel _resourceData;
		private readonly ISandContainerViewProvider _sandContainerViewProvider;
		private readonly IFillMeshShaderController _fillMeshShaderControllerProvider;
		private readonly ISandParticlePlayerSystem _sandParticlePlayerSystem;
		private readonly IStatReadOnly _statReadOnly;
		private readonly IGameplayInterfacePresenter _gameplayInterfacePresenter;
		private readonly IResourceModelReadOnly _resourceReadOnly;

		private int _increasedDelta;
		private int _lastCashScore;

		public ResourcesProgressPresenter(
			IGameplayInterfacePresenter gameplayInterfaceView,
			IResourceModelReadOnly resourceReadOnly,
			IResourceModel persistentProgressService,
			IFillMeshShaderController fillMeshShaderControllerProvider,
			ISandParticlePlayerSystem sandParticlePlayerSystem,
			IStatReadOnly statReadOnly
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
			_statReadOnly = statReadOnly ?? throw new ArgumentNullException(nameof(statReadOnly));
		}

		public IReadOnlyProgress<int> SoftCurrency => _resourceReadOnly.SoftCurrency;

		private int CurrentScore => _resourceReadOnly.CurrentCashScore;

		private bool IsHalfGlobalScore => IsHalfScoreReached();

		public bool IsMaxScoreReached => CheckMaxScore();

		public override void Enable() =>
			SetEnableSand();

		public override void Disable() =>
			SetEnableSand();

		public int GetCalculatedDecreasedMoney(int count)
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

		public void ClearTotalResources()
		{
			_resourceData.ClearTotalResources();
			SetView();
		}

		public bool TryDecreaseMoney(int count)
		{
			if (_resourceReadOnly.SoftCurrency.Value - count < 0)
				throw new ArgumentOutOfRangeException($"{SoftCurrency} less than zero");

			bool result = _resourceData.TryDecreaseMoney(count);

			if (result == false)
				throw new ArgumentOutOfRangeException(
					$"Not enough money: {_resourceReadOnly.SoftCurrency.Value}. Count: {count}"
				);

			SetMoneyTextView();
			return true;
		}

		public bool TryAddSand(int newScore)
		{
			if (newScore <= 0) throw new ArgumentOutOfRangeException(nameof(newScore));

			if (CurrentScore > _statReadOnly.Value)
				return false;

			_lastCashScore = _resourceReadOnly.CurrentCashScore;

			int score = (int)Mathf.Clamp(newScore, 0, _statReadOnly.Value);
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
			_resourceReadOnly.CurrentCashScore < _statReadOnly.Value;

		private void OnHalfScoreReached()
		{
			if (IsHalfGlobalScore == true)
				_gameplayInterfacePresenter.SetActiveGoToNextLevelButton(true);
		}

		private void SetMoneyTextView() =>
			_gameplayInterfacePresenter.SetSoftCurrency(_resourceReadOnly.SoftCurrency.Value);

		private void SetView()
		{
			_gameplayInterfacePresenter.SetTotalResourceCount(_resourceReadOnly.CurrentTotalResources);
			_gameplayInterfacePresenter.SetCashScore(_resourceReadOnly.CurrentCashScore);
			_fillMeshShaderControllerProvider.FillArea(CurrentScore, 0, _statReadOnly.Value);
		}

		private void SetEnableSand() =>
			_fillMeshShaderControllerProvider.FillArea(
				CurrentScore,
				0,
				_statReadOnly.Value
			);

		private bool IsHalfScoreReached() =>
			_resourceReadOnly.CurrentTotalResources >= Mathf.CeilToInt(_resourceReadOnly.MaxTotalResourceCount / 2f);
	}
}
