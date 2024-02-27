using System;
using Sources.Presentation;
using Sources.Presentation.UI.YandexAuthorization;
using Sources.ServicesInterfaces;
using Sources.Utils.Configs.Scripts;

namespace Sources.Infrastructure.Factories.LeaderBoard
{
	public class AuthorizationViewFactory
	{
		private readonly IAssetFactory _assetFactory;

		public AuthorizationViewFactory(IAssetFactory assetFactory) =>
			_assetFactory = assetFactory ?? throw new ArgumentNullException(nameof(assetFactory));

		private string EditorAuthorizationView => ResourcesAssetPath.Scene.UIResources.Editor.AuthorizationView;
		private string YandexAuthorizationView => ResourcesAssetPath.Scene.UIResources.Yandex.YandexAuthorizationView;

		public IAuthorizationView Create()
		{
#if YANDEX_CODE
			return GetYandexAuthorizationHandler();
#endif
			return GetEditorAuthorizationView();
		}

		private IAuthorizationView GetYandexAuthorizationHandler() =>
			_assetFactory.InstantiateAndGetComponent<YandexAuthorizationView>(YandexAuthorizationView);

		private IAuthorizationView GetEditorAuthorizationView() =>
			_assetFactory.InstantiateAndGetComponent<EditorAuthorizationView>(EditorAuthorizationView);
	}
}