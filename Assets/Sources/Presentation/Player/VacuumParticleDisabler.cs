using UnityEngine;

namespace Sources.Presentation.Player
{
	public class VacuumParticleDisabler : MonoBehaviour
	{
		[SerializeField] private ParticleSystem _particle;

		public void SetActiveParticle(bool isActive)
		{
			if (isActive)
				_particle.Play();
			else
				_particle.Stop();
		}
	}
}