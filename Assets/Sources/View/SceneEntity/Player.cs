using System.Collections;
using Application.DI;
using Domain.Scene;
using InfrastructureInterfaces;
using UnityEngine;

namespace View.SceneEntity
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
			if (collisionInfo.collider.TryGetComponent(out ReloadTrigger _))
			{
				if (_sellRoutine != null)
					StopCoroutine(_sellRoutine);

				_sellRoutine = StartCoroutine(SellRoutine());
			}
		}

		private void OnCollisionExit(Collision collisionInfo)
		{
			if (collisionInfo.collider.TryGetComponent(out ReloadTrigger _))
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