using Sources.Controllers;
using Sources.ControllersInterfaces;
using Sources.Infrastructure.Common.Provider;
using Sources.InfrastructureInterfaces.Providers;

namespace Sources.Infrastructure.Providers
{
	public class GameplayInterfacePresenterProvider : Provider<IGameplayInterfacePresenter> , IGameplayInterfacePresenterProvider{ }
}