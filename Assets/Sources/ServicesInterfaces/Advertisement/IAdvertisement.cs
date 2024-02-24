using System;
using Cysharp.Threading.Tasks;

namespace Sources.ServicesInterfaces.Advertisement
{
	public interface IAdvertisement
	{
		event Action Opened;
		public UniTask ShowAd(Action onRewardsCallback, Action onCloseCallback = null);
	}
}