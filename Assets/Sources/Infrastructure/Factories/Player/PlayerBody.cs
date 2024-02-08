using System.Collections;
using Sources.Infrastructure.Providers;
using Sources.InfrastructureInterfaces;
using Sources.InfrastructureInterfaces.GameObject;
using Sources.InfrastructureInterfaces.Presenters;
using Sources.Services.Triggers;
using Sources.ServicesInterfaces;
using UnityEngine;
using VContainer;

namespace Sources.Infrastructure.Factories.Player
{
	public class PlayerBody : Presenter, IPlayer
	{
		private ResourcesProgressPresenterProvider _progressPresenterProvider; 
		private UnityEngine.Coroutine _sellRoutine;
		private IResourcesProgressPresenter ProgressPresenter => _progressPresenterProvider.Instance;

		[Inject]
		private void Construct(ResourcesProgressPresenterProvider progressPresenter) =>
			_progressPresenterProvider = progressPresenter;

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
			ProgressPresenter.SellSand();
			yield return null;
		}
	}
}