using Model.Character;
using UnityEngine;

namespace Presenter.SceneEntity
{
	public class Vacuum : Presenter
	{
		private VacuumModel _model;

		private void VacuumTerrain()
		{
			_model = (VacuumModel)Model;
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