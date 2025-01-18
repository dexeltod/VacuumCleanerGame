using System;
using System.Collections;
using Sources.BusinessLogic.ServicesInterfaces.Advertisement;
using Sources.DomainInterfaces;
using Sources.DomainInterfaces.Entities;
using Sources.Infrastructure.Services.Decorators.Common;
using Sources.Utils;
using UnityEngine;

namespace Sources.Infrastructure.Services.Decorators
{
	public class SpeedDecorator : Decorator, ISpeedDecorator
	{
		private readonly IAdvertisement _advertisement;
		private readonly IStat _speed;
		private readonly float _baseSpeed;
		private readonly ICoroutineRunner _coroutineRunnerProvider;

		private readonly WaitForSeconds _waitForSeconds;

		private float _currentBaseSpeed;

		public SpeedDecorator(
			ICoroutineRunner coroutineRunnerProvider,
			IAdvertisement advertisement,
			float time,
			IStat speed
		)
		{
			if (time < 0) throw new ArgumentOutOfRangeException(nameof(time));

			_coroutineRunnerProvider = coroutineRunnerProvider ??
			                           throw new ArgumentNullException(nameof(coroutineRunnerProvider));
			_advertisement = advertisement ?? throw new ArgumentNullException(nameof(advertisement));
			_speed = speed ?? throw new ArgumentNullException(nameof(speed));

			_waitForSeconds = new WaitForSeconds(time);
		}

		private ICoroutineRunner CoroutineRunner => _coroutineRunnerProvider;

		public bool IsDecorated { get; private set; }

		public override void Enable()
		{
			_advertisement.Rewarded += OnRewarded;
			_speed.SetDefault();
		}

		public override void Disable()
		{
			_advertisement.Rewarded -= OnRewarded;
			_speed.SetDefault();
		}

		public void Increase() =>
			_advertisement.ShowInterstitialAd(OnClosed, OnRewarded);

		private void OnClosed() =>
			_speed.SetDefault();

		private void OnRewarded() =>
			CoroutineRunner.Run(StartSpeedRoutine());

		private IEnumerator StartSpeedRoutine()
		{
			IsDecorated = true;
			_speed.Increase(2);

			yield return _waitForSeconds;

			_speed.SetDefault();

			IsDecorated = false;
		}
	}
}
