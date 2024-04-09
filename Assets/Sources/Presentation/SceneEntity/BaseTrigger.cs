using Sources.Presentation.Player;
using UnityEngine;

namespace Sources.Presentation.SceneEntity
{
	public class BaseTrigger : MonoBehaviour
	{
		private void OnTriggerEnter(Collider other)
		{
			if (other.TryGetComponent(out VacuumColliderDisabler colliderDisabler))
				colliderDisabler.DisableColliders();
			if (other.TryGetComponent(out VacuumParticleDisabler vacuumDisabler))
				vacuumDisabler.SetActiveParticle(false);
		}

		private void OnTriggerExit(Collider other)
		{
			if (other.TryGetComponent(out VacuumColliderDisabler colliderDisabler))
				colliderDisabler.EnableColliders();

			if (other.TryGetComponent(out VacuumParticleDisabler vacuumDisabler))
				vacuumDisabler.SetActiveParticle(true);
		}
	}
}