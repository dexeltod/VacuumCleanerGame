using System;
using Sources.ApplicationServicesInterfaces.Authorization;
using Sources.Presentation.UI.YandexAuthorization;
using Sources.ServicesInterfaces;
using Sources.Utils.Configs.Scripts;
using VContainer;

namespace Sources.Infrastructure.Factories.LeaderBoard
{
	public class AuthorizationFactory
	{
		private readonly IAssetResolver _assetResolver;

		[Inject]
		public AuthorizationFactory(IAssetResolver assetResolver) =>
			_assetResolver = assetResolver ?? throw new ArgumentNullException(nameof(assetResolver));

		public IAuthorization Create()
		{
#if !UNITY_EDITOR
			GetYandexAuthorizationHandler();
#endif
			return GetEditorAuthorizationView();
		}

		private IAuthorization GetYandexAuthorizationHandler() =>
			_assetResolver.InstantiateAndGetComponent<YandexAuthorizationView>(
				ResourcesAssetPath.Scene.UIResources.Yandex.AuthHandler
			);

		private IAuthorization GetEditorAuthorizationView() =>
			_assetResolver.InstantiateAndGetComponent<EditorAuthorization>(
				ResourcesAssetPath.Scene.UIResources.Editor.AuthHandler
			);
	}
}