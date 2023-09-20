using System;

namespace Sources.DomainInterfaces
{
	public interface ISaveClearable
	{
		event Func<IGameProgressModel> ProgressCleared;
	}
}