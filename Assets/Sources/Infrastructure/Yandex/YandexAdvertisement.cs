using System;
using System.Collections;
using Agava.YandexGames;
using Cysharp.Threading.Tasks;
using Sources.BusinessLogic.ServicesInterfaces.Advertisement;
using Sources.Utils;
using UnityEngine;

namespace Sources.Infrastructure.Yandex
{
	public sealed class YandexAdvertisement : IAdvertisement
	{
		private const int Cooldown = 60;
		private readonly WaitForSeconds _waitForSeconds = new(Cooldown);

		private bool _canSeeReward;

		private Coroutine _interstitialAdCooldown;

		private bool _isClosed;
		private bool _isRewarded;

		public YandexAdvertisement(ICoroutineRunner coroutineRunner)
		{
			_canSeeReward = true;
			CoroutineRunner = coroutineRunner ?? throw new ArgumentNullException(nameof(coroutineRunner));
		}

		private ICoroutineRunner CoroutineRunner { get; }

		public event Action Closed;
		public event Action Opened;
		public event Action Rewarded;

		public async UniTask ShowVideoAd(Action onClosed, Action onRewarded, Action onOpened, Action onError = null)
		{
			_isClosed = false;
			_isClosed = false;

			VideoAd.Show(
				() =>
				{
					Debug.Log("VideoAd opened");
					Opened!.Invoke();
					onOpened!.Invoke();
				},
				() =>
				{
					_isClosed = true;
					Closed!.Invoke();
					onClosed!.Invoke();
				},
				() =>
				{
					Rewarded!.Invoke();
					_isRewarded = true;
					onRewarded!.Invoke();
				}
			);

			await UniTask.WaitWhile(() => _isClosed == false || _isRewarded);
		}

		public async UniTask ShowInterstitialAd(
			Action onClosed,
			Action onRewarded,
			Action onOpened,
			Action onError = null
		)
		{
			if (_canSeeReward == false)
				throw new Exception("Can't show interstitial ad");

			Debug.Log("Interstitial Ad opened");

			_isClosed = false;
			_isClosed = false;

			InterstitialAd.Show(
				() =>
				{
					_isRewarded = true;
					StartInterstitialAdCooldown();
					onOpened!.Invoke();
				},
				closed =>
				{
					_isClosed = closed;
					onClosed!.Invoke();
				},
				result =>
				{
					_isClosed = true;
					onRewarded!.Invoke();
				},
				() => throw new Exception("You are offline")
			);

			await UniTask.WaitWhile(() => _isClosed == false || _isRewarded);
		}

		public UniTask ShowStickAd(Action onClosed, Action onRewarded, Action onOpened, Action onError = null)
		{
			Debug.Log("StickAd opened");
			StickyAd.Show();

			return default;
		}

		private IEnumerator RunInterstitialAdCooldownCoroutine()
		{
			_canSeeReward = false;

			yield return _waitForSeconds;

			_canSeeReward = true;
		}

		private void StartInterstitialAdCooldown()
		{
			if (_interstitialAdCooldown != null)
				CoroutineRunner.StopCoroutineRunning(_interstitialAdCooldown);

			_interstitialAdCooldown = CoroutineRunner.Run(RunInterstitialAdCooldownCoroutine());
		}
	}
}