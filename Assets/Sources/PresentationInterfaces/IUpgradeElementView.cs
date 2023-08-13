using System;
using Sources.InfrastructureInterfaces;

namespace Sources.PresetrationInterfaces
{
	public interface IUpgradeElementView
	{
		event Action BuyButtonPressed;
		IUpgradeElementView Construct(IUpgradeItem item, IUpgradeItemView viewInfo);
		void AddProgressPointColor(int count);
	}
}