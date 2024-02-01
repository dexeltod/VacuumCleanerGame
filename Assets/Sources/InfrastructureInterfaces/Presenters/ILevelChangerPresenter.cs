using Sources.PresentationInterfaces;

namespace Sources.Infrastructure.Presenters
{
	public interface ILevelChangerPresenter
	{
		void SetButton(IGoToTextLevelButtonObserver button);
		void Enable();
		void Disable();
	}
}