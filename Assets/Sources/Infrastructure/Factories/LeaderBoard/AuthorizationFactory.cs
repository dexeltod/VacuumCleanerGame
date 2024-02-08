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
		private readonly IAssetFactory _assetFactory;

		[Inject]
		public AuthorizationFactory(IAssetFactory assetFactory) =>
			_assetFactory = assetFactory ?? throw new ArgumentNullException(nameof(assetFactory));

		public IAuthorization Create()
		{
#if !UNITY_EDITOR
			GetYandexAuthorizationHandler();
#endif
			return GetEditorAuthorizationView();
		}

		private IAuthorization GetYandexAuthorizationHandler() =>
			_assetFactory.InstantiateAndGetComponent<YandexAuthorizationView>(
				ResourcesAssetPath.Scene.UIResources.Yandex.AuthHandler
			);

		private IAuthorization GetEditorAuthorizationView() =>
			_assetFactory.InstantiateAndGetComponent<EditorAuthorization>(
				ResourcesAssetPath.Scene.UIResources.Editor.AuthHandler
			);
	}
}