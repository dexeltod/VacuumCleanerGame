using Sources.PresentersInterfaces;

namespace Sources.PresentationInterfaces.Common
{
	public interface IPresentableView<in T> where T : class, IPresenter
	{
		void Construct(T presenter);
	}
}