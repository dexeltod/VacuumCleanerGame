using System.Collections;
using Sources.Core.DI;
using Sources.Infrastructure.InfrastructureInterfaces;
using Sources.Infrastructure.Scene;
using UnityEngine;

namespace Sources.View.SceneEntity
{
	public class Player : Presenter
	{
		private PlayerTransformable _model;
		private IResourcesProgressViewModel _progressViewModel;
		private Coroutine _sellRoutine;

		private void VacuumTerrain()
		{
			_model = (PlayerTransformable)Model;
		}

		private void Awake()
		{
			_progressViewModel = ServiceLocator.Container.GetSingle<IResourcesProgressViewModel>();
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

#if UNITY_EDITOR
		private void OnDrawGizmos()
		{
			if (_model != null)
				Gizmos.DrawLine(transform.position, transform.position + transform.forward * _model.VacuumDistance);
		}
#endif
	}
}