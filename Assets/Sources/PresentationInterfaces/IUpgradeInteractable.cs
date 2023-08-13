using System;
using Sources.PresetrationInterfaces;

namespace Sources.View
{
	public interface IUpgradeInteractable
	{
		event Action<IUpgradeItemView> BuyButtonPressed;
	}
}