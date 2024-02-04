using System;

namespace Sources.ApplicationServicesInterfaces.Authorization
{
	public interface IAuthorization
	{
		event Action<bool> AuthorizeCallback;
		void EnableAuthorizeWindow();
		void DisableAuthorizeWindow();
	}
}