using System;
using System.Collections;
using Model.Character;
using Model.DI;
using UnityEngine;
using ViewModel;

namespace Presenter.SceneEntity
{
	public class Vacuum : Presenter
	{
		private VacuumModel _model;
		private IGameProgressViewModel _progressViewModel;
		private Coroutine _sellRoutine;

		private void VacuumTerrain()
		{
			_model = (VacuumModel)Model;
		}

		private void Awake()
		{
			_progressViewModel = ServiceLocator.Container.GetSingle<IGameProgressViewModel>();
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