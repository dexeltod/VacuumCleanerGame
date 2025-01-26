using System;

namespace Sources.PresentationInterfaces
{
	public interface IUpgradeElementChangeableView
	{
		void AddProgressPointColor(int count = 1);
		void SetPriceText(int price);
		event Action<int> BuyButtonPressed;
	}
}
