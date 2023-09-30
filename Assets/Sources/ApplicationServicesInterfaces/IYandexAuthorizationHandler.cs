using System;
using System.Collections;
using Cysharp.Threading.Tasks;

namespace Sources.PresentationInterfaces
{
	public interface IYandexAuthorizationHandler
	{
		void IsWantsAuthorization(Action<bool> callback, Action isPlayerWantsAuthorizeCallback = null);
	}
}