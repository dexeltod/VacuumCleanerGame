using System;
using Cysharp.Threading.Tasks;

namespace Sources.BusinessLogic.ServicesInterfaces.Advertisement
{
	public interface IAdvertisement
	{
		public UniTask ShowInterstitialAd(
			Action onClosed,
			Action onRewarded,
			Action onOpened = null,
			Action onError = null
		);

		public UniTask ShowStickAd(
			Action onClosed,
			Action onRewarded,
			Action onOpened,
			Action onError = null
		);

		public UniTask ShowVideoAd(
			Action onClosed,
			Action onRewarded,
			Action onOpened,
			Action onError = null
		);

		event Action Closed;
		event Action Rewarded;
		event Action Opened;
	}
}