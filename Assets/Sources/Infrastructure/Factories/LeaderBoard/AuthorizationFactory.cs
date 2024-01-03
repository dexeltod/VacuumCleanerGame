using System;
using Sources.ApplicationServicesInterfaces;
using Sources.Presentation.UI.YandexAuthorization;
using Sources.ServicesInterfaces;
using Sources.ServicesInterfaces.Authorization;
using Sources.Utils.Configs.Scripts;

namespace Sources.Application.StateMachine.GameStates
{
	public class AuthorizationFactory
	{
		private readonly IAssetProvider _assetProvider;

		public AuthorizationFactory(IAssetProvider assetProvider) =>
			_assetProvider = assetProvider ?? throw new ArgumentNullException(nameof(assetProvider));

		public IAuthorization Create()
		{
#if !UNITY_EDITOR
			GetYandexAuthorizationHandler();
#endif
			return null;
		}

		private IYandexAuthorizationView GetYandexAuthorizationHandler()
		{
			return _assetProvider.InstantiateAndGetComponent<YandexAuthorizationView>(
				ResourcesAssetPath.GameObjects.Yandex.AuthHandler
			);
		}
	}
}