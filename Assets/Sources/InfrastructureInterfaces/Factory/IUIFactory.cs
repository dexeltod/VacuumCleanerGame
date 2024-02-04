using Sources.PresentationInterfaces;
using Sources.ServicesInterfaces;

namespace Sources.InfrastructureInterfaces.Factory
{
	public interface IUIFactory
	{
		IGameplayInterfaceView Instantiate();
		void SetActive(bool isActive);
	}
}