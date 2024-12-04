using Sources.ControllersInterfaces.Common;

namespace Sources.InfrastructureInterfaces.Common.Factories.Decorators
{
	public interface IPresenterFactory<out T> : IFactory<T> where T : class, IPresenter { }
}