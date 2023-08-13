using System.Collections;
using Sources.DIService;
using Sources.Services.Triggers;
using Sources.ServicesInterfaces;
using UnityEngine;

namespace Sources.Services.PlayerServices
{
	public class Player : Presenter
	{
		private IResourcesProgressViewModel _progressViewModel;
		private Coroutine _sellRoutine;

		private void Awake()
		{
			_progressViewModel = GameServices.Container.Get<IResourcesProgressViewModel>();
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
			_progressViewModel.SellSand();
			yield return null;
		}
	}
}