using Sources.InfrastructureInterfaces.Common.Factories;

namespace Sources.Infrastructure.Common.Factory
{
	public abstract class Factory<T> : IFactory<T> where T : class
	{
		public abstract T Create();
	}
}