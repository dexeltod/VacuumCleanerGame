using System;
using System.Collections;
using Sources.Controllers;
using Sources.Controllers;
using Sources.DomainInterfaces;
using Sources.InfrastructureInterfaces.Providers;
using Sources.ServicesInterfaces.Advertisement;
using Sources.Utils;
using UnityEngine;

namespace Sources.Infrastructure.Services.Decorators
{
	public class SpeedDecorator : ISpeedDecorator
	{
		private const float Time = 10f;

		private readonly IPlayerStatChangeable _playerStat;
		private readonly ICoroutineRunnerProvider _coroutineRunnerProvider;
		private readonly IAdvertisement _advertisement;
		private readonly WaitForSeconds _waitForSeconds = new WaitForSeconds(Time);

		public SpeedDecorator(
			IPlayerStatChangeable playerStat,
			ICoroutineRunnerProvider coroutineRunnerProvider,
			IAdvertisement advertisement
		)
		{
			_playerStat = playerStat ?? throw new ArgumentNullException(nameof(playerStat));
			_coroutineRunnerProvider = coroutineRunnerProvider ??
				throw new ArgumentNullException(nameof(coroutineRunnerProvider));
			_advertisement = advertisement ?? throw new ArgumentNullException(nameof(advertisement));
		}

		private ICoroutineRunner CoroutineRunner => _coroutineRunnerProvider.Implementation;

		public void Increase() =>
			_advertisement.ShowAd(OnRewarded);

		private void OnRewarded() =>
			CoroutineRunner.Run(StartSpeedRoutine());

		private IEnumerator StartSpeedRoutine()
		{
			Debug.Log("speed on");

			_playerStat.SetValue(_playerStat.Value * 2);
			yield return _waitForSeconds;
			_playerStat.SetValue(_playerStat.Value / 2);
		}
	}
}