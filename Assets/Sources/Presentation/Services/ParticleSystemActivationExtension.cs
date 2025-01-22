using System;
using Sources.PresentationInterfaces;
using UnityEngine;

namespace Sources.Presentation.Services
{
	public class ParticleSystemActivationExtension : MonoBehaviour
	{
		private ICollectable _collectable;
		private ParticleSystem _particleSystem;

		private void OnDisable() =>
			_collectable.Collected -= OnCollected;

		private void OnDestroy() =>
			_collectable.Collected -= OnCollected;

		public void Construct(ParticleSystem particles, ICollectable collectable)
		{
			_collectable = collectable ?? throw new ArgumentNullException(nameof(collectable));
			_particleSystem = particles ? particles : throw new ArgumentNullException(nameof(particles));
			_collectable.Collected += OnCollected;
		}

		private void OnCollected() =>
			_particleSystem.Stop();
	}
}