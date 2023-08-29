using System.Collections;
using Sources.DIService;
using Sources.InfrastructureInterfaces;
using Sources.Services.Triggers;
using Sources.ServicesInterfaces;
using UnityEngine;

namespace Sources.Infrastructure.Factories.Player
{
	public class Player : Presenter, IPlayer
	{
		private IResourcesProgressPresenter _progressPresenter;
		private Coroutine _sellRoutine;

		private void Awake()
		{
			_progressPresenter = GameServices.Container.Get<IResourcesProgressPresenter>();
		}

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