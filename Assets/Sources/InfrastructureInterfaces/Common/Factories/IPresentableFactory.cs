using Sources.PresentationInterfaces.Common;
using Sources.PresentersInterfaces;

namespace Sources.InfrastructureInterfaces.Common.Factories
{
	public interface IPresentableFactory<out T, in TP> : IFactory<T>
		where T : IPresentableView<TP> where TP : class, IPresenter
	{
	}
}