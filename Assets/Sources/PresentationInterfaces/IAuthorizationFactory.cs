using Sources.ControllersInterfaces;

namespace Sources.PresentationInterfaces
{
	public interface IAuthorizationFactory
	{
		IAuthorizationPresenter Create(IMainMenuView view);
	}
}
