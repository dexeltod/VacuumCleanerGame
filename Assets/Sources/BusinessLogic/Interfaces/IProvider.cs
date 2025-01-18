namespace Sources.BuisenessLogic.Interfaces
{
	public interface IProvider<T>
	{
		public T Self { get; }

		public T Register(T instance);

		T Register<TI>(T instance);

		void Unregister();

		TI GetContract<TI>() where TI : class;
	}
}