using System;
using System.Collections;
using System.Collections.Generic;
using Sources.BusinessLogic.Repository;
using Sources.BusinessLogic.Services;
using Sources.Controllers.Common;
using Sources.ControllersInterfaces;
using Sources.DomainInterfaces;
using Sources.DomainInterfaces.DomainServicesInterfaces;
using Sources.DomainInterfaces.Entities;
using Sources.PresentationInterfaces.Common;
using Sources.PresentationInterfaces.Triggers;
using Sources.Utils;
using UnityEngine;
using static UnityEngine.Mathf;

namespace Sources.Controllers
{
	public class ResourcesProgressPresenter : Presenter, IResourcesProgressPresenter
	{
		private readonly ICoroutineRunner _coroutineRunner;
		private readonly IFillMeshShader _fillMeshShader;
		private readonly IResourceModel _resourceData;
		private readonly IResourceModelReadOnly _resourceReadOnly;
		private readonly Dictionary<int, IResourcePresentation> _rocks;
		private readonly ISandParticlePlayerSystem _sandParticlePlayerSystem;
		private readonly SceneResourcesRepository _sceneResourcesRepository;
		private readonly IStatReadOnly _statReadOnly;
		private readonly ITriggerSell _triggerSell;

		private Coroutine _coroutine;

		private int _increasedDelta;
		private bool _isSelling;

		private int _lastCashScore;

		public ResourcesProgressPresenter(
			IResourceModelReadOnly resourceReadOnly,
			IResourceModel resourceModel,
			IFillMeshShader fillMeshShaderProvider,
			ISandParticlePlayerSystem sandParticlePlayerSystem,
			IStatReadOnly maxCashScore,
			ITriggerSell triggerSell,
			ICoroutineRunner coroutineRunner,
			Dictionary<int, IResourcePresentation> rocks,
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
			foreach (IResourcePresentation presentation in _rocks.Values)
				presentation.Collided += OnResourceCollided;

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
			foreach (IResourcePresentation presentation in _rocks.Values)
				presentation.Collided -= OnResourceCollided;

			_triggerSell.OnTriggerStayed -= OnTriggerStay;
			SetEnableSand();
		}

		private bool IsHalfScoreReached() =>
			_resourceReadOnly.CurrentTotalResources >= CeilToInt(_resourceReadOnly.MaxTotalResourceCount / 2f);

		private void OnResourceCollided(int id)
		{
			if (_resourceReadOnly.CashScore.ReadOnlyValue >= _resourceReadOnly.CashScore.ReadOnlyMaxValue)
				return;

			IResourcePresentation rock = _rocks[id];
			ISceneResourceEntity sceneResourceEntity = _sceneResourcesRepository.Get(rock.ID);

			if (_resourceData.TryAddScore(sceneResourceEntity.Value))
				rock.Collect();
		}

		private void OnTriggerStay(bool isStay)
		{
			_isSelling = isStay;

			if (isStay == false && _coroutine != null)
			{
				_coroutineRunner.StopCoroutineRunning(_coroutine);
				_coroutine = null;
			}

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
			while (_isSelling)
			{
				Sell();
				yield return null;
			}
		}

		private void SetEnableSand() =>
			_fillMeshShader.FillArea(
				CurrentScore,
				0,
				_statReadOnly.Value
			);

		private void SetView()
		{
			_fillMeshShader.FillArea(CurrentScore, 0, _statReadOnly.Value);
		}
	}
}
