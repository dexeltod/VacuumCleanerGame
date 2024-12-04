using Sources.ControllersInterfaces.Common;
using Sources.PresentationInterfaces.Common;

namespace Sources.InfrastructureInterfaces.Common.Factories
{
	public interface IPresentableFactory<out T, in TP> : IFactory<T>
		where T : IPresentableView<TP> where TP : class, IPresenter
	{
	}
}