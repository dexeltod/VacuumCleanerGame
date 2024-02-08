using Sources.ControllersInterfaces.Common;

namespace Sources.PresentationInterfaces.Common
{
	public interface IPresentableView<in T> where T : class, IPresenter
	{
		void Construct(T presenter);
	}
}