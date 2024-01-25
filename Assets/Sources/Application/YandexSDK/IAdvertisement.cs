using System;
using Cysharp.Threading.Tasks;

namespace Sources.Application.YandexSDK
{
	public interface IAdvertisement
	{
		public UniTask ShowAd(Action onOpenCallback, Action onRewardsCallback, Action onCloseCallback);
	}
}