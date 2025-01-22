using System.Collections.Generic;
using UnityEngine;

namespace Sources.Presentation.SceneEntity
{
	public class VacuumColliderDisabler : MonoBehaviour
	{
		[SerializeField] private List<Collider> _colliders;

		public void DisableColliders()
		{
			foreach (Collider collider in _colliders)
				collider.enabled = false;
		}

		public void EnableColliders()
		{
			foreach (Collider collider in _colliders)
				collider.enabled = true;
		}
	}
}