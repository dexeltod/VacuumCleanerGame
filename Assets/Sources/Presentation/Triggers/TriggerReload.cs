using System;
using System.Collections;
using Sources.ControllersInterfaces;
using Sources.DomainInterfaces;
using Sources.Infrastructure.Services.SceneTriggers;
using Sources.PresentationInterfaces;
using Sources.Utils;
using UnityEngine;
using VContainer;

namespace Sources.Presentation.Triggers
{
	public class TriggerReload : MonoBehaviour, ITriggerReload
	{
		private IResourcesProgressPresenter _progressPresenterProvider;
		private ICoroutineRunner _coroutineRunnerProvider;
		private Coroutine _coroutine;
		private IPersistentProgressService _persistentProgressService;
		public IResourcesProgressPresenter ResourceProgress => _progressPresenterProvider;

		public int CurrentScore =>
			_persistentProgressService.GlobalProgress.ResourceModelReadOnly.CurrentCashScore;

		[Inject]
		private void Construct(
			IResourcesProgressPresenter progressPresenterProvider,
			ICoroutineRunner coroutineRunner,
			IPersistentProgressService persistentProgressService
		)
		{
			_persistentProgressService = persistentProgressService ??
			                             throw new ArgumentNullException(nameof(persistentProgressService));
			_coroutineRunnerProvider = coroutineRunner ?? throw new ArgumentNullException(nameof(coroutineRunner));
			_progressPresenterProvider = progressPresenterProvider ??
			                             throw new ArgumentNullException(nameof(progressPresenterProvider));
		}

		private void OnTriggerStay(Collider other)
		{
			if (!other.TryGetComponent(out IPlayer _)) return;

			if (CurrentScore > 0)
				_coroutine = _coroutineRunnerProvider.Run(SellRoutine());
		}

		private void OnTriggerExit(Collider other)
		{
			if (!other.TryGetComponent(out IPlayer _))
				return;

			if (_coroutine != null)
				_coroutineRunnerProvider.StopCoroutineRunning(_coroutine);
		}

		private IEnumerator SellRoutine()
		{
			_progressPresenterProvider.Sell();
			return null;
		}
	}
}