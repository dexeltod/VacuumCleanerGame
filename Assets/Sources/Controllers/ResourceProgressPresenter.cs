using System;
using System.Collections;
using System.Collections.Generic;
using Sources.BusinessLogic.Providers;
using Sources.BusinessLogic.Repository;
using Sources.BusinessLogic.Services;
using Sources.Controllers.Common;
using Sources.ControllersInterfaces;
using Sources.DomainInterfaces;
using Sources.DomainInterfaces.DomainServicesInterfaces;
using Sources.DomainInterfaces.Entities;
using Sources.PresentationInterfaces;
using Sources.PresentationInterfaces.Common;
using Sources.PresentationInterfaces.Triggers;
using Sources.Utils;
using UnityEngine;

namespace Sources.Controllers
{
	public class ResourcesProgressPresenter : Presenter, IResourcesProgressPresenter
	{
		private readonly ICoroutineRunner _coroutineRunner;
		private readonly IFillMeshShader _fillMeshShader;
		private readonly IGameplayInterfaceView _gameplayInterfacePresenter;
		private readonly IResourceModel _resourceData;
		private readonly IResourceModelReadOnly _resourceReadOnly;
		private readonly ICollection<IResourcePresentation> _rocks;
		private readonly ISandContainerViewProvider _sandContainerView;
		private readonly ISandParticlePlayerSystem _sandParticlePlayerSystem;
		private readonly SceneResourcesRepository _sceneResourcesRepository;
		private readonly IStatReadOnly _statReadOnly;
		private readonly ITriggerSell _triggerSell;

		private Coroutine _coroutine;

		private int _increasedDelta;

		private int _lastCashScore;

		public ResourcesProgressPresenter(
			IResourceModelReadOnly resourceReadOnly,
			IResourceModel resourceModel,
			IFillMeshShader fillMeshShaderProvider,
			ISandParticlePlayerSystem sandParticlePlayerSystem,
			IStatReadOnly maxCashScore,
			ITriggerSell triggerSell,
			ICoroutineRunner coroutineRunner,
			ICollection<IResourcePresentation> rocks,
			SceneResourcesRepository sceneResourcesRepository)
		{
			_resourceReadOnly = resourceReadOnly ?? throw new ArgumentNullException(nameof(resourceReadOnly));
			_resourceData = resourceModel ?? throw new ArgumentNullException(nameof(resourceModel));
			_fillMeshShader = fillMeshShaderProvider ?? throw new ArgumentNullException(nameof(fillMeshShaderProvider));
			_sandParticlePlayerSystem =
				sandParticlePlayerSystem ?? throw new ArgumentNullException(nameof(sandParticlePlayerSystem));
			_statReadOnly = maxCashScore ?? throw new ArgumentNullException(nameof(maxCashScore));
			_triggerSell = triggerSell ?? throw new ArgumentNullException(nameof(triggerSell));
			_coroutineRunner = coroutineRunner ?? throw new ArgumentNullException(nameof(coroutineRunner));
			_rocks = rocks ?? throw new ArgumentNullException(nameof(rocks));
			_sceneResourcesRepository =
				sceneResourcesRepository ?? throw new ArgumentNullException(nameof(sceneResourcesRepository));
		}

		private bool IsCurrentScoreMoreThanZero => CurrentScore > 0;

		private int CurrentScore => _resourceReadOnly.CurrentCashScore;

		private bool IsHalfGlobalScore => IsHalfScoreReached();

		public IReadOnlyProgress<int> SoftCurrency => _resourceReadOnly.SoftCurrency;

		public override void Enable()
		{
			foreach (IResourcePresentation presentation in _rocks) presentation.Collided += () => OnResourceCollided(presentation);

			_resourceReadOnly.TotalAmount.HalfReached += OnHalfScoreReached;
			_triggerSell.OnTriggerStayed += OnTriggerStay;
			SetEnableSand();
		}

		public void ClearTotalResources()
		{
			_resourceData.ClearTotalResources();
			SetView();
		}

		public override void Disable()
		{
			foreach (IResourcePresentation presentation in _rocks) presentation.Collided -= () => OnResourceCollided(presentation);

			_triggerSell.OnTriggerStayed -= OnTriggerStay;
			SetEnableSand();
		}

		public bool TryDecreaseMoney(int count)
		{
			if (_resourceReadOnly.SoftCurrency.ReadOnlyValue - count < 0)
				throw new ArgumentOutOfRangeException($"{SoftCurrency} less than zero");

			bool result = _resourceData.TryDecreaseMoney(count);

			if (result == false)
				throw new ArgumentOutOfRangeException(
					$"Not enough money: {_resourceReadOnly.SoftCurrency.ReadOnlyValue}. Count: {count}"
				);

			return true;
		}

		private bool IsHalfScoreReached() =>
			_resourceReadOnly.CurrentTotalResources >= Mathf.CeilToInt(_resourceReadOnly.MaxTotalResourceCount / 2f);

		private void OnHalfScoreReached()
		{
			if (IsHalfGlobalScore)
				_gameplayInterfacePresenter.SetActiveGoToNextLevelButton(true);
		}

		private void OnResourceCollided(IResourcePresentation resourcePresentation)
		{
			if (_resourceReadOnly.CashScore.ReadOnlyValue >= _resourceReadOnly.CashScore.ReadOnlyMaxValue)
				return;

			int value = _sceneResourcesRepository.Get(resourcePresentation.ID).Value;

			if (_resourceData.TryAddScore(value))
				resourcePresentation.Collect();
		}

		private void OnTriggerStay(bool obj)
		{
			if (obj == false)
				if (_coroutine != null)
					_coroutineRunner.StopCoroutineRunning(_coroutine);

			if (_coroutine != null)
				return;

			if (IsCurrentScoreMoreThanZero)
				_coroutine = _coroutineRunner.Run(SellRoutine());
		}

		private void PlayParticleSystem()
		{
			if (_lastCashScore < _resourceReadOnly.CurrentCashScore)
				_sandParticlePlayerSystem.Play();
		}

		private void Sell()
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

			SetView();
		}

		private IEnumerator SellRoutine()
		{
			Sell();
			return null;
		}

		private void SetEnableSand() =>
			_fillMeshShader.FillArea(
				CurrentScore,
				0,
				_statReadOnly.Value
			);

		private void SetView()
		{
			_gameplayInterfacePresenter.SetTotalResourceScore(_resourceReadOnly.TotalAmount.ReadOnlyValue);
			_gameplayInterfacePresenter.SetCashScore(_resourceReadOnly.CashScore.ReadOnlyValue);
			_fillMeshShader.FillArea(CurrentScore, 0, _statReadOnly.Value);
		}
	}
}