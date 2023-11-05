using UnityEngine;

namespace Sources.View.SceneEntity
{
	public class BaseTrigger : MonoBehaviour
	{
		private void OnTriggerEnter(Collider other)
		{
			if (other.TryGetComponent(out VacuumColliderDisabler disabler))
				disabler.DisableColliders();
		}

		private void OnTriggerExit(Collider other)
		{
			if (other.TryGetComponent(out VacuumColliderDisabler disabler))
				disabler.EnableColliders();
		}
	}
}