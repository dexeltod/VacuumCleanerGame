using Sources.PresentationInterfaces;

namespace Sources.Services.Interfaces
{
	public interface IUIFactory : IUIGetter
	{
		IGameplayInterfaceView Instantiate();
		void                   SetActive(bool isActive);
	}
}