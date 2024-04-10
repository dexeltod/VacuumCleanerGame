using System;
using System.Collections;
using Sources.InfrastructureInterfaces.Providers;
using Sources.PresentationInterfaces.Player;
using UnityEngine;

namespace Sources.Services
{
	public class SandParticlePlayerSystem
	{
		private readonly ISandParticleSystemProvider _sandParticleSystem;
		private readonly ICoroutineRunnerProvider _coroutineRunner;
		private readonly WaitForSeconds _waitForSeconds;
		private ISandParticleSystem _particleSystem;

		public SandParticlePlayerSystem(
			ISandParticleSystemProvider sandParticleSystem,
			ICoroutineRunnerProvider coroutineRunner,
			int playTime
		)
		{
			_sandParticleSystem = sandParticleSystem ?? throw new ArgumentNullException(nameof(sandParticleSystem));
			_coroutineRunner = coroutineRunner ?? throw new ArgumentNullException(nameof(coroutineRunner));

			if (playTime <= 0) throw new ArgumentOutOfRangeException(nameof(playTime));
			_waitForSeconds = new WaitForSeconds(playTime);
		}

		public void Play() =>
			_coroutineRunner.Implementation.Run(PlayParticleSystem());

		private IEnumerator PlayParticleSystem()
		{
			_particleSystem = _sandParticleSystem.Implementation;
			_particleSystem.Play();

			yield return _waitForSeconds;

			_particleSystem.Stop();
		}
	}
}