namespace Sources.InfrastructureInterfaces.Common.Providers
{
	public interface IProvider<T>
	{
		public T Instance { get; }
		public void Register(T instance);
		void Unregister();
	}
}