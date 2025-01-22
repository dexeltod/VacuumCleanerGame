namespace Sources.BusinessLogic.Interfaces
{
	public interface IProvider<T>
	{
		public T Self { get; }

		TI GetContract<TI>() where TI : class;

		public T Register(T instance);

		T Register<TI>(T instance);

		void Unregister();
	}
}