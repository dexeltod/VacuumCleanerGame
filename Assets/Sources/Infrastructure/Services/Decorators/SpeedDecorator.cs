using System;
using System.Collections;
using Sources.ControllersInterfaces;
using Sources.DomainInterfaces;
using Sources.InfrastructureInterfaces.Providers;
using Sources.ServicesInterfaces;
using Sources.ServicesInterfaces.Advertisement;
using Sources.Utils;
using UnityEngine;

namespace Sources.Infrastructure.Services.Decorators
{
	public class SpeedDecorator : ISpeedDecorator
	{
		private readonly IAdvertisement _advertisement;
		private readonly ICoroutineRunnerProvider _coroutineRunnerProvider;

		private readonly IPlayerStatChangeable _playerStat;
		private readonly WaitForSeconds _waitForSeconds;

		public SpeedDecorator(
			IPlayerStatChangeable playerStat,
			ICoroutineRunnerProvider coroutineRunnerProvider,
			IAdvertisement advertisement,
			float time,
			ILevelProgressFacade levelProgressFacade
		)
		{
			if (time < 0) throw new ArgumentOutOfRangeException(nameof(time));
			_playerStat = playerStat ?? throw new ArgumentNullException(nameof(playerStat));
			_coroutineRunnerProvider = coroutineRunnerProvider ??
				throw new ArgumentNullException(nameof(coroutineRunnerProvider));
			_advertisement = advertisement ?? throw new ArgumentNullException(nameof(advertisement));
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
			_playerStat.SetValue(_playerStat.Value + 2);
			yield return _waitForSeconds;

			_playerStat.SetValue(_playerStat.Value - 2);
			IsDecorated = false;
		}
	}
}