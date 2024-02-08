using Sources.ControllersInterfaces.Common;
using Sources.InfrastructureInterfaces.Common.Factories;
using Sources.PresentationInterfaces.Common;

namespace Sources.Infrastructure.Common.Factory.Decorators
{
	public abstract class PresentableFactory<T, TP> : IPresentableFactory<T, TP>
		where T : IPresentableView<TP>
		where TP : class, IPresenter
	{
		public abstract T Create();

	}
}