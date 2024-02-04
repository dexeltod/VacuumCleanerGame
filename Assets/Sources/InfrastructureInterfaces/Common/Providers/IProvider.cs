namespace Sources.InfrastructureInterfaces.Common.Providers
{
	public interface IProvider<T>
	{
		T Instance { get; }
		void Register(T instance);
	}
}