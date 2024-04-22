using System.Collections.Generic;
using Sources.Domain.Temp;

namespace Sources.DomainInterfaces
{
	public interface IShopEntity : ISubEntity
	{
		IReadOnlyList<IProgressEntity> ProgressEntities { get; }
	}
}