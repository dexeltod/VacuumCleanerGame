using Sources.ServicesInterfaces;

namespace Sources.PresentersInterfaces
{
	public interface ILevelChangerPresenter : IPresenter
	{
		void SetButton(IGoToTextLevelButtonObserver button);
	}
}