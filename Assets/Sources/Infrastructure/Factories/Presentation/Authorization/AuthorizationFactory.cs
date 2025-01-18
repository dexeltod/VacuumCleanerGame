using System;
using Sources.BusinessLogic;
using Sources.BusinessLogic.Interfaces;
using Sources.Controllers;
using Sources.ControllersInterfaces;
using Sources.Infrastructure.Services;
using Sources.PresentationInterfaces;
using UnityEngine;

namespace Sources.Presentation.Factories.Authorization
{
	public sealed class AuthorizationFactory : IAuthorizationFactory
	{
		private readonly AssetFactory _assetFactory;
		private readonly ICloudServiceSdk _cloudServiceSdk;
		private readonly TranslatorService _localizationService;

		public AuthorizationFactory(
			AssetFactory assetFactory,
			ICloudServiceSdk cloudServiceSdk,
			TranslatorService localizationService
		)
		{
			_assetFactory = assetFactory ?? throw new ArgumentNullException(nameof(assetFactory));

			_cloudServiceSdk = cloudServiceSdk;
			_localizationService = localizationService ?? throw new ArgumentNullException(nameof(localizationService));
		}

		public IAuthorizationPresenter Create(IMainMenuView view)
		{
			IAuthorizationView authorizationView = new AuthorizationViewFactory(
				_assetFactory,
				_localizationService,
				view
			).Create();

			AuthorizationPresenter authorizationPresenter = new AuthorizationPresenter(_cloudServiceSdk, authorizationView);

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
