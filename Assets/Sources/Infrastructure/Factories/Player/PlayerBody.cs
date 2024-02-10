using System;
using System.Collections;
using Sources.ControllersInterfaces;
using Sources.Infrastructure.Providers;
using Sources.InfrastructureInterfaces.GameObject;
using Sources.Services.Triggers;
using Sources.UseCases.Scene;
using UnityEngine;
using VContainer;

namespace Sources.Infrastructure.Factories.Player
{
	public class PlayerBody : Presenter, IPlayer
	{
		// private ResourcesProgressPresenterProvider _progressPresenterProvider;
		// private Coroutine _sellRoutine;
		// private ICoroutineRunnerProvider _coroutineRunnerProvider;
		// private IResourcesProgressPresenter ProgressPresenter => _progressPresenterProvider.Implementation;
		// private ICoroutineRunner CoroutineRunner => _coroutineRunnerProvider.Implementation;
		//
		// [Inject]
		// private void Construct(
		// 	ResourcesProgressPresenterProvider progressPresenter,
		// 	ICoroutineRunnerProvider coroutineRunner
		// )
		// {
		// 	_coroutineRunnerProvider = coroutineRunner ?? throw new ArgumentNullException(nameof(coroutineRunner));
		// 	_progressPresenterProvider
		// 		= progressPresenter ?? throw new ArgumentNullException(nameof(progressPresenter));
		// }
		//
		// private void OnCollisionEnter(Collision collisionInfo)
		// {
		// 	if (!collisionInfo.collider.TryGetComponent(out TriggerReloadController _))
		// 		return;
		//
		// 	if (_sellRoutine != null)
		// 		CoroutineRunner.Run(SellRoutine());
		//
		// 	_sellRoutine = StartCoroutine(SellRoutine());
		// }
		//
		// private void OnCollisionExit(Collision collisionInfo)
		// {
		// 	if (collisionInfo.collider.TryGetComponent(out TriggerReloadController _))
		// 		StopCoroutine(SellRoutine());
		// }
		//
		// private IEnumerator SellRoutine()
		// {
		// 	ProgressPresenter.SellSand();
		// 	yield return null;
		// }
	}
}