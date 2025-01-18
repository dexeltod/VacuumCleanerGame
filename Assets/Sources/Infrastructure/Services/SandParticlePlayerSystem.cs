using System;
using System.Collections;
using Sources.BusinessLogic.Services;
using Sources.PresentationInterfaces.Player;
using Sources.Utils;
using UnityEngine;

namespace Sources.Infrastructure.Services
{
	public class SandParticlePlayerSystem : ISandParticlePlayerSystem
	{
		private readonly ISandParticleView _sandParticleView;
		private readonly ICoroutineRunner _coroutineRunner;
		private readonly WaitForSeconds _waitForSeconds;
		private ISandParticleView _particleView;

		public SandParticlePlayerSystem(
			ISandParticleView sandParticleView,
			ICoroutineRunner coroutineRunner,
			int playTime
		)
		{
			_sandParticleView = sandParticleView ?? throw new ArgumentNullException(nameof(sandParticleView));
			_coroutineRunner = coroutineRunner ?? throw new ArgumentNullException(nameof(coroutineRunner));

			if (playTime <= 0) throw new ArgumentOutOfRangeException(nameof(playTime));

			_waitForSeconds = new WaitForSeconds(playTime);
		}

		public void Play() =>
			_coroutineRunner.Run(PlayParticleSystem());

		private IEnumerator PlayParticleSystem()
		{
			_particleView = _sandParticleView;
			_particleView.Play();

			yield return _waitForSeconds;

			_particleView.Stop();
		}
	}
}
