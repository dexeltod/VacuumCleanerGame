using System.Collections.Generic;
using Sources.DomainInterfaces.Models.Shop.Upgrades;

namespace Sources.DomainInterfaces.Models.Shop
{
	public interface IShopModel
	{
		IReadOnlyList<IStatUpgradeEntityReadOnly> ProgressEntities { get; }
	}
}