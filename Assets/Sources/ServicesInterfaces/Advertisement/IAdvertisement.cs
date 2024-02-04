using System;
using Cysharp.Threading.Tasks;

namespace Sources.ServicesInterfaces.Advertisement
{
	public interface IAdvertisement
	{
		public UniTask ShowAd(Action onOpenCallback, Action onRewardsCallback, Action onCloseCallback);
	}
}