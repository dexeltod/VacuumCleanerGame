using System;
using Agava.YandexGames;
using Cysharp.Threading.Tasks;
using Sources.ServicesInterfaces.Advertisement;

namespace Sources.Infrastructure.Yandex
{
	public sealed class YandexAdvertisement : IAdvertisement
	{
		public event Action Opened;

		public async UniTask ShowAd(Action onRewardsCallback, Action onCloseCallback)
		{
			bool isClosed = false;
			bool isRewarded = false;

			VideoAd.Show(
				() => Opened!.Invoke(),
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