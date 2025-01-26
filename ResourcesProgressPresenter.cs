using System;
using System.Collections;
using Sources.BusinessLogic.Providers;
using Sources.BusinessLogic.Repository;
using Sources.BusinessLogic.Services;
using Sources.ControllersInterfaces;
using Sources.DomainInterfaces;
using Sources.DomainInterfaces.DomainServicesInterfaces;
using Sources.DomainInterfaces.Entities;
using Sources.PresentationInterfaces;
using Sources.PresentationInterfaces.Triggers;
using Sources.Utils;
using UnityEngine;

public class ResourcesProgressPresenter :  IResourcesProgressPresenter
{
	private readonly ICoroutineRunner _coroutineRunner;
	private readonly IFillMeshShader _fillMeshShader;
	private readonly IGameplayInterfaceView _gameplayInterfacePresenter;
	private readonly IResourceModel _resourceData;
	private readonly IResourceModelReadOnly _resourceReadOnly;
	private readonly ISandContainerViewProvider _sandContainerView;
	private readonly ISandParticlePlayerSystem _sandParticlePlayerSystem;
	private readonly IStatReadOnly _statReadOnly;
	private readonly ITriggerSell _triggerSell;

	private Coroutine _coroutine;

	private int _increasedDelta;
	private int _lastCashScore;

	public ResourcesProgressPresenter(IResourceModelReadOnly resourceReadOnly,
		IResourceModel resourceModel,
		IFillMeshShader fillMeshShaderProvider,
		ISandParticlePlayerSystem sandParticlePlayerSystem,
		IStatReadOnly statReadOnly,
		ITriggerSell triggerSell,
		ICoroutineRunner coroutineRunner)
	{
		_resourceReadOnly = resourceReadOnly ?? throw new ArgumentNullException(nameof(resourceReadOnly));
		_resourceData = resourceModel ??
		                throw new ArgumentNullException(nameof(resourceModel));
		_fillMeshShader = fillMeshShaderProvider ??
		                  throw new ArgumentNullException(nameof(fillMeshShaderProvider));
		_sandParticlePlayerSystem = sandParticlePlayerSystem ??
		                            throw new ArgumentNullException(nameof(sandParticlePlayerSystem));
		_statReadOnly = statReadOnly ?? throw new ArgumentNullException(nameof(statReadOnly));
		_triggerSell = triggerSell ?? throw new ArgumentNullException(nameof(triggerSell));
		_coroutineRunner = coroutineRunner ?? throw new ArgumentNullException(nameof(coroutineRunner));
	}

	private bool IsCurrentScoreMoreThanZero => CurrentScore > 0;

	private int CurrentScore => _resourceReadOnly.CurrentCashScore;

	private bool IsHalfGlobalScore => IsHalfScoreReached();

	public IReadOnlyProgress<int> SoftCurrency => _resourceReadOnly.SoftCurrency;

	public bool IsMaxScoreReached => CheckMaxScore();

	public override void Enable()
	{
		_triggerSell.OnTriggerStayed += OnTriggerStay;
		SetEnableSand();
	}

	public override void Disable()
	{
		_triggerSell.OnTriggerStayed -= OnTriggerStay;
		SetEnableSand();
	}

	public int GetCalculatedDecreasedMoney(int count)
	{
		if (count <= 0)
			throw new ArgumentOutOfRangeException(nameof(count));

		return _resourceReadOnly.SoftCurrency.ReadOnlyValue - count;
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
		if (_resourceReadOnly.SoftCurrency.ReadOnlyValue - count < 0)
			throw new ArgumentOutOfRangeException($"{SoftCurrency} less than zero");

		bool result = _resourceData.TryDecreaseMoney(count);

		if (result == false)
			throw new ArgumentOutOfRangeException(
				$"Not enough money: {_resourceReadOnly.SoftCurrency.ReadOnlyValue}. Count: {count}"
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

		var score = (int)Mathf.Clamp(newScore, 0, _statReadOnly.Value);
		_resourceData.AddScore(score);

		PlayParticleSystem();
		SetView();

		OnHalfScoreReached();

		return true;
	}

	private bool CheckMaxScore() =>
		_resourceReadOnly.CurrentCashScore < _statReadOnly.Value;

	private bool IsHalfScoreReached() =>
		_resourceReadOnly.CurrentTotalResources >= Mathf.CeilToInt(_resourceReadOnly.MaxTotalResourceCount / 2f);

	private void OnHalfScoreReached()
	{
		if (IsHalfGlobalScore)
			_gameplayInterfacePresenter.SetActiveGoToNextLevelButton(true);
	}

	private void OnTriggerStay(bool obj)
	{
		if (obj == false)
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

		SetMoneyTextView();
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

	private void SetMoneyTextView() =>
		_gameplayInterfacePresenter.SetCashScore(_resourceReadOnly.SoftCurrency.ReadOnlyValue);

	private void SetView()
	{
		_gameplayInterfacePresenter.SetTotalResourceScore(_resourceReadOnly.TotalAmount.ReadOnlyValue);
		_gameplayInterfacePresenter.SetCashScore(_resourceReadOnly.CashScore.ReadOnlyValue);
		_fillMeshShader.FillArea(CurrentScore, 0, _statReadOnly.Value);
	}
}
