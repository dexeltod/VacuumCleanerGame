using System;

namespace Sources.ServicesInterfaces.Authorization
{
	public interface IAuthorization
	{
		event Action<bool> AuthorizeCallback;
		void EnableAuthorizeWindow();
		void DisableAuthorizeWindow();
	}
}