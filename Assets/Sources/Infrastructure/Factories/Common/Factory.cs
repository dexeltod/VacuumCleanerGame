using Sources.InfrastructureInterfaces.Common;
using Sources.InfrastructureInterfaces.Common.Factories;

namespace Sources.Infrastructure.Factories.Common
{
	public abstract class Factory<T> : IFactory<T> where T : class, IFactory<T>
	{
		public T Create() =>
			throw new System.NotImplementedException();
	}
}