using System.Collections.Generic;
using Sources.Utils.Enums;

namespace Sources.InfrastructureInterfaces.Configs
{
	public interface IProgressItemConfig
	{
		string Title { get; }
		string Description { get; }
		int MaxProgressCount { get; }
		IReadOnlyList<float> Stats { get; }
		IReadOnlyList<int> Prices { get; }
		ProgressType Type { get; }
		int Id { get; }
		bool IsModifiable { get; }
	}
}