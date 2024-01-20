using System;


namespace Sources.ApplicationServicesInterfaces
{
	public interface IRewardService 
	{
		void ShowAd(Action onOpened, Action onRewarded, Action onClosed);
	}
}