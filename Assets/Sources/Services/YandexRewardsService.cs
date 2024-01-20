using System;
using Sources.ApplicationServicesInterfaces;

namespace Sources.Services
{
	public class YandexRewardsService : IRewardService
	{
		public YandexRewardsService() { }

		public void ShowAd(Action onOpened, Action onRewarded, Action onClosed) =>
			throw new NotImplementedException();
	}
}