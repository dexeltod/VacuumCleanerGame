using System;

namespace Sources.ApplicationServicesInterfaces
{
	public interface IYandexAuthorizationChoiceable
	{
		event Func<bool> PlayerNotAuthorized;
	}
}