using System;
using System.Collections;
using Sources.DomainInterfaces;
using Sources.PresentationInterfaces;
using Sources.ServicesInterfaces;
using Sources.UseCases.Scene;
using UnityEngine;

namespace Sources.Infrastructure.Presenters
{
	public class SandContainerPresenter : IDisposable
	{
		private const float TopValue = 1;

		private readonly IResourcesModel _resourcesModel;
		private readonly ISandContainerView _sandContainerView;
		private readonly IResourceProgressEventHandler _resourceProgressEventHandler;
		private readonly ISandParticleSystem _sandParticle;
		private readonly ICoroutineRunner _coroutineRunner;
		private readonly WaitForSeconds _waitForSeconds;

		private Coroutine _currentRoutine;

		public SandContainerPresenter(
			IResourcesModel resourcesModel,
			ISandContainerView sandContainerView,
			IResourceProgressEventHandler resourceProgressEventHandler,
			ISandParticleSystem sandParticle,
			ICoroutineRunner coroutineRunner
		)
		{
			_resourcesModel = resourcesModel ?? throw new ArgumentNullException(nameof(resourcesModel));
			_sandContainerView = sandContainerView ?? throw new ArgumentNullException(nameof(sandContainerView));
			_resourceProgressEventHandler = resourceProgressEventHandler ??
				throw new ArgumentNullException(nameof(resourceProgressEventHandler));
			_sandParticle = sandParticle ?? throw new ArgumentNullException(nameof(sandParticle));
			_coroutineRunner = coroutineRunner ?? throw new ArgumentNullException(nameof(coroutineRunner));

			_waitForSeconds = new WaitForSeconds(1f);

			_resourceProgressEventHandler.CashScoreChanged += OnCashCoreChanged;
		}

		public void Dispose() =>
			_resourceProgressEventHandler.CashScoreChanged -= OnCashCoreChanged;

		private void OnCashCoreChanged(int cashScore)
		{
			if (cashScore < 0 || cashScore > _resourcesModel.MaxCashScore)
				throw new ArgumentOutOfRangeException(nameof(cashScore));

			StartSandFalling();

			float normalized = NormalizeValue(
				TopValue,
				cashScore,
				_resourcesModel.MaxCashScore
			);

			_sandContainerView.SetSand(normalized);
		}

		private void StartSandFalling()
		{
			if (_currentRoutine == null)
				_coroutineRunner.StartCoroutine(SellFallingRoutine());
			else
			{
				_coroutineRunner.StopCoroutineRunning(_currentRoutine);
				_currentRoutine = _coroutineRunner.StartCoroutine(SellFallingRoutine());
			}
		}

		private IEnumerator SellFallingRoutine()
		{
			_sandParticle.Play();
			yield return _waitForSeconds;
			_sandParticle.Stop();
		}

		private float NormalizeValue(float topValue, int newScore, int currentMaxScore) =>
			topValue / currentMaxScore * newScore;
	}
}