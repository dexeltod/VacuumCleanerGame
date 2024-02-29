using Sources.ControllersInterfaces.Common;

namespace Sources.Presentation
{
	public interface IAuthorizationPresenter : IPresenter
	{
		void SetChoice(bool isWants);
		void Authorize();
		bool IsAuthorized { get; }
	}
}