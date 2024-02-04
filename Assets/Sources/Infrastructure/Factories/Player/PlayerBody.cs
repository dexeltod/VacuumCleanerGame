using System.Collections;

using Sources.InfrastructureInterfaces;
using Sources.InfrastructureInterfaces.GameObject;
using Sources.Services.Triggers;
using Sources.ServicesInterfaces;
using UnityEngine;
using VContainer;

namespace Sources.Infrastructure.Factories.Player
{
	public class PlayerBody : Presenter, IPlayer
	{
		private IResourcesProgressPresenter _progressPresenter;
		private UnityEngine.Coroutine _sellRoutine;

		[Inject]
		private void Construct(IResourcesProgressPresenter progressPresenter) =>
			_progressPresenter = progressPresenter;

		private void OnCollisionEnter(Collision collisionInfo)
		{
			if (collisionInfo.collider.TryGetComponent(out TriggerReload _))
			{
				if (_sellRoutine != null)
					StopCoroutine(_sellRoutine);

				_sellRoutine = StartCoroutine(SellRoutine());
			}
		}

		private void OnCollisionExit(Collision collisionInfo)
		{
			if (collisionInfo.collider.TryGetComponent(out TriggerReload _))
				StopCoroutine(SellRoutine());
		}

		private IEnumerator SellRoutine()
		{
			_progressPresenter.SellSand();
			yield return null;
		}
	}
}