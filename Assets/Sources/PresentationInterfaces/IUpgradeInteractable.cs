using System;
using Sources.InfrastructureInterfaces;

namespace Sources.PresentationInterfaces
{
	public interface IUpgradeInteractable
	{
		event Action<IUpgradeItemData> BuyButtonPressed;
	}
}