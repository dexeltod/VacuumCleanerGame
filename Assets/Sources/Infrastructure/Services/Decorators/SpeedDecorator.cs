using System;
using System.Collections;
using Sources.ControllersInterfaces;
using Sources.Domain.Common;
using Sources.Domain.Temp;
using Sources.DomainInterfaces;
using Sources.InfrastructureInterfaces.Providers;
using Sources.ServicesInterfaces.Advertisement;
using Sources.Utils;
using UnityEngine;

namespace Sources.Infrastructure.Services.Decorators
{
	public class SpeedDecorator : ISpeedDecorator
	{
		private readonly IAdvertisement _advertisement;
		private readonly IModifiableStat _speed;
		private readonly int _baseSpeed;
		private readonly ICoroutineRunnerProvider _coroutineRunnerProvider;

		private readonly WaitForSeconds _waitForSeconds;

		private int _currentBaseSpeed;

		public SpeedDecorator(
			ICoroutineRunnerProvider coroutineRunnerProvider,
			IAdvertisement advertisement,
			float time,
			IModifiableStat speed
		)
		{
			if (time < 0) throw new ArgumentOutOfRangeException(nameof(time));

			_coroutineRunnerProvider = coroutineRunnerProvider ??
				throw new ArgumentNullException(nameof(coroutineRunnerProvider));
			_advertisement = advertisement ?? throw new ArgumentNullException(nameof(advertisement));
			_speed = speed;

			_waitForSeconds = new WaitForSeconds(time);
		}

		private ICoroutineRunner CoroutineRunner => _coroutineRunnerProvider.Implementation;

		public bool IsDecorated { get; private set; }

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