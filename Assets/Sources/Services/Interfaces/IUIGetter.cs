
using Sources.PresentationInterfaces;

namespace Sources.Services.Interfaces
{
	public interface IUIGetter 
	{
		IGameplayInterfaceView GameplayInterface { get; }
	}
}