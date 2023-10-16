using Sources.DIService;
using Sources.PresentationInterfaces;

namespace Sources.Services.Interfaces
{
	public interface IUIGetter : IService
	{
		IGameplayInterfaceView GameplayInterface { get; }
	}
}