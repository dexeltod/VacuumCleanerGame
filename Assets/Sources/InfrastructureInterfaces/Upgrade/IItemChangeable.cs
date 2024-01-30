using System;

namespace Sources.Infrastructure.ScriptableObjects.Shop
{
	public interface IItemChangeable
	{
		event Action<int> PriceChanged;
	}
}