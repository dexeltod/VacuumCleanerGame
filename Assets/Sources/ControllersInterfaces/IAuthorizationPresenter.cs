using Sources.ControllersInterfaces.Common;

namespace Sources.ControllersInterfaces
{
	public interface IAuthorizationPresenter : IPresenter
	{
		void SetChoice(bool isWants);
		void Authorize();
		bool IsAuthorized { get; }
	}
}