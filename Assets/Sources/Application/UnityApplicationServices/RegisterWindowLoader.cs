using System;
using Sources.Application.StateMachine.GameStates;
using Sources.ApplicationServicesInterfaces;
using Sources.Presentation.UI.YandexAuthorization;
using Sources.ServicesInterfaces;
using Sources.ServicesInterfaces.Authorization;
using Sources.Utils.Configs.Scripts;
using VContainer;

namespace Sources.Application.UnityApplicationServices
{
	public class RegisterWindowLoader : IRegisterWindowLoader
	{
		private readonly IAssetProvider _assetProvider;

		[Inject]
		public RegisterWindowLoader(IAssetProvider assetProvider) =>
			_assetProvider = assetProvider ?? throw new ArgumentNullException(nameof(assetProvider));

		public IAuthorization Load()
		{
#if YANDEX_CODE
			return _assetProvider.InstantiateAndGetComponent<YandexAuthorizationView>(
				ResourcesAssetPath.Scene.UIResources.Yandex.AuthHandler
			);
#endif
			return _assetProvider.InstantiateAndGetComponent<EditorAuthorization>(
				ResourcesAssetPath.Scene.UIResources.Editor.AuthHandler
			);
		}
	}
}