using System;
using Sources.ApplicationServicesInterfaces;
using Sources.ApplicationServicesInterfaces.Authorization;
using Sources.Infrastructure.Factories.LeaderBoard;
using Sources.ServicesInterfaces;
using Sources.Utils.Configs.Scripts;
using VContainer;

namespace Sources.Application.UnityApplicationServices
{
	public class RegisterWindowLoader : IRegisterWindowLoader
	{
		private readonly IAssetFactory _assetFactory;

		private string EditorAuthHandler => ResourcesAssetPath.Scene.UIResources.Editor.AuthHandler;

		[Inject]
		public RegisterWindowLoader(IAssetFactory assetFactory) =>
			_assetFactory = assetFactory ?? throw new ArgumentNullException(nameof(assetFactory));

		public IAuthorization Load()
		{
#if YANDEX_CODE
			return _assetProvider.InstantiateAndGetComponent<YandexAuthorizationView>(
				ResourcesAssetPath.Scene.UIResources.Yandex.AuthHandler
			);
#endif

			return _assetFactory.InstantiateAndGetComponent<EditorAuthorization>(EditorAuthHandler);
		}
	}
}