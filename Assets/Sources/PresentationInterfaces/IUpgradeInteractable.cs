using System;
using Sources.InfrastructureInterfaces;
using Sources.InfrastructureInterfaces.Upgrade;

namespace Sources.PresentationInterfaces
{
	public interface IUpgradeInteractable
	{
		event Action<IUpgradeItemData> BuyButtonPressed;
	}
}