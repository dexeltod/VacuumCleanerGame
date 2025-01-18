using Sources.ControllersInterfaces;

namespace Sources.Infrastructure.Factories.Presentation
{
	public abstract class PresenterFactory<T> : IPresenterFactory<T> where T : class, IPresenter
	{
		public abstract T Create();
	}
}