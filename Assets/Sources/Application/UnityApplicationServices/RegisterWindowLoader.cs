using System;
using Sources.Application.StateMachine.GameStates;
using Sources.ApplicationServicesInterfaces;
using Sources.ApplicationServicesInterfaces.Authorization;
using Sources.Infrastructure.Factories.LeaderBoard;
using Sources.Presentation.UI.YandexAuthorization;
using Sources.ServicesInterfaces;
using Sources.Utils.Configs.Scripts;
using VContainer;

namespace Sources.Application.UnityApplicationServices
{
	public class RegisterWindowLoader : IRegisterWindowLoader
	{
		private readonly IAssetResolver _assetResolver;

		[Inject]
		public RegisterWindowLoader(IAssetResolver assetResolver) =>
			_assetResolver = assetResolver ?? throw new ArgumentNullException(nameof(assetResolver));

		public IAuthorization Load()
		{
#if YANDEX_CODE
			return _assetProvider.InstantiateAndGetComponent<YandexAuthorizationView>(
				ResourcesAssetPath.Scene.UIResources.Yandex.AuthHandler
			);
#endif
			return _assetResolver.InstantiateAndGetComponent<EditorAuthorization>(
				ResourcesAssetPath.Scene.UIResources.Editor.AuthHandler
			);
		}
	}
}