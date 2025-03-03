using System;
using Sources.PresentationInterfaces;
using UnityEngine;

namespace Sources.Boot.Scripts.Factories.Presentation.Scene
{
	public class ParticleSystemActivationExtension : MonoBehaviour
	{
		private ICollideable _collideable;
		private ParticleSystem _particleSystem;

		private void OnDisable() => _collideable.Collided -= OnCollided;

		private void OnDestroy() => _collideable.Collided -= OnCollided;

		public void Construct(ParticleSystem particles, ICollideable collideable)
		{
			_collideable = collideable ?? throw new ArgumentNullException(nameof(collideable));
			_particleSystem = particles ? particles : throw new ArgumentNullException(nameof(particles));
			_collideable.Collided += OnCollided;
		}

		private void OnCollided(int value) => _particleSystem.Stop();
	}
}