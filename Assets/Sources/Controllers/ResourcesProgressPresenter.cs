using System;
using Sources.Controllers.Common;
using Sources.ControllersInterfaces;
using Sources.DomainInterfaces;
using Sources.DomainInterfaces.DomainServicesInterfaces;
using Sources.InfrastructureInterfaces.Providers;
using Sources.PresentationInterfaces;
using Sources.PresentationInterfaces.Player;
using Sources.Services;
using Sources.Utils;
using UnityEngine;

namespace Sources.Controllers
{
	public class ResourcesProgressPresenter : Presenter, IResourcesProgressPresenter
	{
		private const float NormalizeThreshold = 1;
		private const float MinThreshold = -1;

		private readonly IResourcesModel _resourcesData;
		private readonly ISandContainerViewProvider _sandContainerViewProvider;
		private readonly IFillMeshShaderControllerProvider _fillMeshShaderControllerProvider;
		private readonly IGameplayInterfaceProvider _gameplayInterfaceView;
		private readonly SandParticlePlayerSystem _sandParticleParticlePlayerSystem;

		private int _increasedDelta;
		private int _lastCashScore;

		public IResourceReadOnly<int> SoftCurrency => _resourcesData.SoftCurrency;

		private IGameplayInterfaceView GameplayInterface =>
			_gameplayInterfaceView.GetContract<IGameplayInterfaceView>();

		private IFillMeshShaderController FillMeshShaderController => _fillMeshShaderControllerProvider.Implementation;

		private int CurrentScore => _resourcesData.CurrentCashScore;
		private bool IsHalfScore => IsHalfScoreReached();
		public bool IsMaxScoreReached => CheckMaxScore();

		public ResourcesProgressPresenter(
			IGameplayInterfaceProvider gameplayInterfaceView,
			IResourcesModel persistentProgressService,
			IFillMeshShaderControllerProvider fillMeshShaderControllerProvider,
			ISandParticleSystem sandParticleSystem,
			ICoroutineRunner coroutineRunner
		)
		{
			_gameplayInterfaceView
				= gameplayInterfaceView ?? throw new ArgumentNullException(nameof(gameplayInterfaceView));
			_resourcesData = persistentProgressService ??
				throw new ArgumentNullException(nameof(persistentProgressService));
			_fillMeshShaderControllerProvider = fillMeshShaderControllerProvider ??
				throw new ArgumentNullException(nameof(fillMeshShaderControllerProvider));

			const int PlayTime = 1;
			_sandParticleParticlePlayerSystem
				= new SandParticlePlayerSystem(sandParticleSystem, coroutineRunner, PlayTime);
		}

		public override void Enable()
		{
			base.Enable();
			SetEnableSand();
		}

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

		public override void Disable()
		{
			base.Disable();
			SetEnableSand();
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
				_sandParticleParticlePlayerSystem.Play();
		}

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

			SetMoneyTextView();
			SetView();
		}

		public void AddMoney(int count)
		{
			//When the user will be watching video
			_resourcesData.AddMoney(count);
			SetMoneyTextView();
		}

		private void SetMoneyTextView() =>
			GameplayInterface.SetSoftCurrency(SoftCurrency.Count);

		private void SetView()
		{
			GameplayInterface.SetGlobalScore(_resourcesData.GlobalScoreCount);
			GameplayInterface.SetCashScore(_resourcesData.CurrentCashScore);
			FillMeshShaderController.FillArea(CurrentScore, 0, _resourcesData.MaxCashScore);
		}

		private bool IsHalfScoreReached()
		{
			int halfScore = _resourcesData.MaxGlobalScore / 2;

			return _resourcesData.Score.Count >= halfScore;
		}
	}
}