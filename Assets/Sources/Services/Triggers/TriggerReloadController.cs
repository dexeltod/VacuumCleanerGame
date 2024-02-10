using System;
using System.Collections;
using Sources.ControllersInterfaces;
using Sources.DomainInterfaces;
using Sources.InfrastructureInterfaces.GameObject;
using Sources.InfrastructureInterfaces.Providers;
using Sources.UseCases.Scene;
using UnityEngine;
using VContainer;

namespace Sources.Services.Triggers
{
	public class TriggerReloadController : MonoBehaviour
	{
		private IResourcesProgressPresenterProvider _progressPresenterProvider;
		private ICoroutineRunnerProvider _coroutineRunnerProvider;
		private Coroutine _coroutine;
		private IPersistentProgressService _persistentProgressService;
		private ICoroutineRunner CoroutineRunner => _coroutineRunnerProvider.Implementation;
		private IResourcesProgressPresenter ResourceProgress => _progressPresenterProvider.Implementation;
		private int CurrentScore => _persistentProgressService.GameProgress.ResourcesModel.CurrentCashScore;

		[Inject]
		private void Construct(
			IResourcesProgressPresenterProvider progressPresenterProvider,
			ICoroutineRunnerProvider coroutineRunner,
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
				_coroutine = CoroutineRunner.Run(SellRoutine());
		}

		private void OnTriggerExit(Collider other)
		{
			if (!other.TryGetComponent(out IPlayer _))
				return;

			if (_coroutine != null)
				CoroutineRunner.StopCoroutineRunning(_coroutine);
		}

		private IEnumerator SellRoutine()
		{
			_progressPresenterProvider.Implementation.SellSand();
			return null;
		}
	}
}