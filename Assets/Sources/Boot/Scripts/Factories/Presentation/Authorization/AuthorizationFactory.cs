using System;
using Sources.BusinessLogic;
using Sources.BusinessLogic.Interfaces;
using Sources.BusinessLogic.ServicesInterfaces;
using Sources.Controllers;
using Sources.ControllersInterfaces;
using Sources.PresentationInterfaces;
using UnityEngine;

namespace Sources.Boot.Scripts.Factories.Presentation.Authorization
{
	public sealed class AuthorizationFactory : IAuthorizationFactory
	{
		private readonly IAssetLoader _assetLoader;
		private readonly ICloudServiceSdk _cloudServiceSdk;
		private readonly TranslatorService _localizationService;

		public AuthorizationFactory(
			IAssetLoader assetLoader,
			ICloudServiceSdk cloudServiceSdk,
			TranslatorService localizationService
		)
		{
			_assetLoader = assetLoader ?? throw new ArgumentNullException(nameof(assetLoader));

			_cloudServiceSdk = cloudServiceSdk;
			_localizationService = localizationService ?? throw new ArgumentNullException(nameof(localizationService));
		}

		public IAuthorizationPresenter Create(IMainMenuView view)
		{
			IAuthorizationView authorizationView = new AuthorizationViewFactory(
				_assetLoader,
				_localizationService,
				view
			).Create();

			var authorizationPresenter = new AuthorizationPresenter(_cloudServiceSdk, authorizationView);

			Construct(
				authorizationView,
				authorizationPresenter
			);

			_cloudServiceSdk.SetStatusInitialized();

			return authorizationPresenter;
		}

		private void Construct(IAuthorizationView authorizationView, AuthorizationPresenter authorizationPresenter)
		{
			authorizationView.Construct(
				authorizationPresenter
			);
			authorizationView.RectTransform.localPosition = Vector2.zero;
			authorizationView.Disable();
		}
	}
}