using System;
using Sources.ServicesInterfaces.Authorization;

namespace Sources.ApplicationServicesInterfaces
{
	public interface IYandexAuthorizationView: IAuthorization
	{
		void IsWantsAuthorization(
			Action<bool> isPlayerWantsAuthorizeCallback = null,
			Action onProcessCompleted = null
		);
	}
}