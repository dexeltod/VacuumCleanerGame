using System.Collections.Generic;
using Sources.BusinessLogic.Interfaces.Configs;

namespace Sources.BusinessLogic.Configs
{
	public interface IProgressItemConfig
	{
		string Title { get; }
		string Description { get; }
		int MaxProgressCount { get; }
		int Id { get; }
		bool IsModifiable { get; }

		int MaxProgress { get; }
		int StartProgress { get; }
		IEnumerable<IShopProgress> Items { get; }
	}
}
