using System.Collections.Generic;
using Sources.Domain.Temp;

namespace Sources.DomainInterfaces
{
	public interface IShopModel
	{
		IReadOnlyList<IUpgradeEntityReadOnly> ProgressEntities { get; }
	}
}