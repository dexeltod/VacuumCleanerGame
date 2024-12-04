using Sources.PresentationInterfaces.Player;
using UnityEngine;

namespace Sources.Presentation.Player
{
	public class SandParticleView : MonoBehaviour, ISandParticleSystem
	{
		[SerializeField] private ParticleSystem[] _particleSystem;

		public void Play()
		{
			foreach (ParticleSystem particle in _particleSystem)
				particle.Play();
		}

		public void Stop()
		{
			foreach (ParticleSystem particle in _particleSystem)
				particle.Stop();
		}
	}
}