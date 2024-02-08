using Sources.PresentationInterfaces;
using Sources.ServicesInterfaces;

namespace Sources.InfrastructureInterfaces.Factory
{
	public interface IUIFactory
	{
		IGameplayInterfaceView Create();
	}
}