using System;
using System.Collections;
using Sources.PresentationInterfaces.Player;
using Sources.Utils;
using UnityEngine;

namespace Sources.Services
{
	public class SandParticlePlayerSystem
	{
		private readonly ISandParticleSystem _sandParticleSystem;
		private readonly ICoroutineRunner _coroutineRunner;
		private readonly WaitForSeconds _waitForSeconds;

		public SandParticlePlayerSystem(
			ISandParticleSystem sandParticleSystem,
			ICoroutineRunner coroutineRunner,
			int playTime
		)
		{
			_sandParticleSystem = sandParticleSystem ?? throw new ArgumentNullException(nameof(sandParticleSystem));
			_coroutineRunner = coroutineRunner ?? throw new ArgumentNullException(nameof(coroutineRunner));

			if (playTime <= 0) throw new ArgumentOutOfRangeException(nameof(playTime));
			_waitForSeconds = new WaitForSeconds(playTime);
		}

		public void Play() =>
			_coroutineRunner.Run(PlayParticleSystem());

		private IEnumerator PlayParticleSystem()
		{
			_sandParticleSystem.Play();
			
			yield return _waitForSeconds;

			_sandParticleSystem.Stop();
		}
	}
}