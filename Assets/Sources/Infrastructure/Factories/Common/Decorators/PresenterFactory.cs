using Sources.InfrastructureInterfaces.Common.Factories.Decorators;
using Sources.PresentersInterfaces;

namespace Sources.Infrastructure.Factories.Common.Decorators
{
	public abstract class PresenterFactory<T> : IPresenterFactory<T> where T : class, IPresenter
	{
		public abstract T Create();
	}
}