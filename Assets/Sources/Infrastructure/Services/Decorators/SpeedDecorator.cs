using System;
using System.Collections;
using Sources.ControllersInterfaces;
using Sources.DomainInterfaces.Entities;
using Sources.Infrastructure.Services.Decorators.Common;
using Sources.InfrastructureInterfaces.Providers;
using Sources.ServicesInterfaces.Advertisement;
using Sources.Utils;
using UnityEngine;

namespace Sources.Infrastructure.Services.Decorators
{
	public class SpeedDecorator : Decorator, ISpeedDecorator
	{
		private readonly IAdvertisement _advertisement;
		private readonly IStat _speed;
		private readonly float _baseSpeed;
		private readonly ICoroutineRunnerProvider _coroutineRunnerProvider;

		private readonly WaitForSeconds _waitForSeconds;

		private float _currentBaseSpeed;

		public SpeedDecorator(
			ICoroutineRunnerProvider coroutineRunnerProvider,
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

		private ICoroutineRunner CoroutineRunner => _coroutineRunnerProvider.Self;

		public bool IsDecorated { get; private set; }

		public override void Disable() =>
			_speed.Clear();

		public void Increase() =>
			_advertisement.ShowAd(OnRewarded);

		private void OnRewarded() =>
			CoroutineRunner.Run(StartSpeedRoutine());

		private IEnumerator StartSpeedRoutine()
		{
			IsDecorated = true;
			_speed.Increase(2);

			yield return _waitForSeconds;

			_speed.Clear();

			IsDecorated = false;
		}
	}
}