using System;
using Agava.YandexGames;
using Cysharp.Threading.Tasks;
using Sources.Application.YandexSDK;

namespace Sources.Application
{
	public class YandexAdvertisement : IAdvertisement
	{
		public async UniTask ShowAd(Action onOpenCallback, Action onRewardsCallback, Action onCloseCallback)
		{
			bool isClosed = false;
			bool isRewarded = false;

			VideoAd.Show(
				() => onOpenCallback?.Invoke(),
				() => isClosed = true,
				() => isRewarded = true
			);

			await UniTask.WaitWhile(() => isClosed == true || isRewarded == true);

			if (isClosed == true)
				onCloseCallback.Invoke();
			else if (isRewarded == true)
				onRewardsCallback.Invoke();
		}
	}
}