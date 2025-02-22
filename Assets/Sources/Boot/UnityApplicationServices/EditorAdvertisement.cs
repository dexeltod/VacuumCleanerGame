using System;
using Cysharp.Threading.Tasks;
using Sources.BusinessLogic.ServicesInterfaces.Advertisement;
using UnityEngine;

namespace Sources.Boot.UnityApplicationServices
{
	public sealed class EditorAdvertisement : IAdvertisement
	{
		public UniTask ShowVideoAd(Action onClosed, Action onRewarded, Action onOpened, Action onError = null)
		{
			onRewarded.Invoke();

			return UniTask.CompletedTask;
		}

		public UniTask ShowInterstitialAd(
			Action onClosed,
			Action onRewarded,
			Action onOpened,
			Action onError =
				null
		)
		{
			onRewarded.Invoke();
			return UniTask.CompletedTask;
		}

		public UniTask ShowStickAd(Action onClosed, Action onRewarded, Action onOpened, Action onError = null)
		{
			onRewarded.Invoke();
			return UniTask.CompletedTask;
		}

		public event Action Closed;
		public event Action Rewarded;
		public event Action Opened;

		public UniTask ShowVideoAd()
		{
			Debug.Log("ShowVideoAd");
			return UniTask.CompletedTask;
		}
	}
}