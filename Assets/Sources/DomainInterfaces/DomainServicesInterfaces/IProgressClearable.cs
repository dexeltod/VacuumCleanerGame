using System;

namespace Sources.DomainInterfaces
{
	public interface IProgressClearable
	{
		event Func<IGameProgressModel> ProgressCleared;
	}
}