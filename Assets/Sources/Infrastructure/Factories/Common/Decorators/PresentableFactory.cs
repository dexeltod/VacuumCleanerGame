using Sources.InfrastructureInterfaces.Common;
using Sources.InfrastructureInterfaces.Common.Factories;
using Sources.PresentationInterfaces.Common;
using Sources.PresentersInterfaces;

namespace Sources.Infrastructure.Factories.Common.Decorators
{
	public abstract class PresentableFactory<T, TP> : IPresentableFactory<T, TP>
		where T : IPresentableView<TP> where TP : class, IPresenter
	{
		public abstract T Create();

	}
}