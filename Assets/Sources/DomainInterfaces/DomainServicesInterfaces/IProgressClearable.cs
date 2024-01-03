using System;

namespace Sources.DomainInterfaces.DomainServicesInterfaces
{
	public interface IProgressClearable
	{
		event Func<IGameProgressModel> ProgressCleared;
	}
}