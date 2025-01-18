using Sources.ControllersInterfaces;

namespace Sources.Presentation.Factories
{
	public abstract class PresenterFactory<T> : IPresenterFactory<T> where T : class, IPresenter
	{
		public abstract T Create();
	}
}