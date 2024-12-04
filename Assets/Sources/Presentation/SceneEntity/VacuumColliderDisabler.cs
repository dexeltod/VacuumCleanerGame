using System.Collections.Generic;
using UnityEngine;

namespace Sources.Presentation.SceneEntity
{
	public class VacuumColliderDisabler : MonoBehaviour
	{
		[SerializeField] private List<Collider> _colliders;

		public void DisableColliders()
		{
			foreach (var collider in _colliders)
				collider.enabled = false;
		}

		public void EnableColliders()
		{
			foreach (var collider in _colliders)
				collider.enabled = true;
		}
	}
}