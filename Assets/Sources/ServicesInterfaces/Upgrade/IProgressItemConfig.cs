using System.Collections.Generic;
using Sources.Utils;

namespace Sources.ServicesInterfaces.Upgrade
{
	public interface IProgressItemConfig
	{
		string Title { get; }
		string Description { get; }
		int MaxProgressCount { get; }
		IReadOnlyList<int> Stats { get; }
		IReadOnlyList<int> Prices { get; }
		ProgressType Type { get; }
		int Id { get; }
		bool IsModifiable { get; }
	}
}