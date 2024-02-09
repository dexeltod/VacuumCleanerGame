namespace Sources.InfrastructureInterfaces.Common.Providers
{
	public interface IProvider<T>
	{
		public T Implementation { get; }
		public void Register(T instance);
		void Register<TI>(T instance);
		void Unregister();
		TI GetContract<TI>() where TI : class;
	}
}