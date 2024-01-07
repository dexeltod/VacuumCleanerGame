using System;
using Sources.DIService;

namespace Sources.ApplicationServicesInterfaces
{
	public interface IRewardService : IService
	{
		void ShowAd(Action onOpened, Action onRewarded, Action onClosed);
	}
}