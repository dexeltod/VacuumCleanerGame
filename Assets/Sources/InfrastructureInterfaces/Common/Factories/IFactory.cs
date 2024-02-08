using System.Collections.Generic;

namespace Sources.InfrastructureInterfaces.Common.Factories
{
	public interface IFactory<out T>
	{
		T Create();
	}
}