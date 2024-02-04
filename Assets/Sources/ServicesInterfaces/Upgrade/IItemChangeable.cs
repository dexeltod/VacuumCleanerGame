using System;

namespace Sources.ServicesInterfaces.Upgrade
{
	public interface IItemChangeable
	{
		event Action<int> PriceChanged;
	}
}