namespace Sources.ControllersInterfaces
{
	public interface IAuthorizationPresenter : IPresenter
	{
		bool IsAuthorized { get; }
		void Authorize();
		void SetChoice(bool isWants);
	}
}