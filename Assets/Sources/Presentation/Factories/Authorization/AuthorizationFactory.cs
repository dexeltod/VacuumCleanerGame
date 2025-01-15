using System;
using Sources.BuisenessLogic;
using Sources.BuisenessLogic.Interfaces;
using Sources.Controllers;
using Sources.Infrastructure.Services;
using Sources.Presentation.UI;
using Sources.PresentationInterfaces;
using UnityEngine;

namespace Sources.Presentation.Factories.Authorization
{
	public sealed class AuthorizationFactory
	{
		private readonly AssetFactory _assetFactory;
		private readonly ICloudServiceSdk _cloudServiceSdk;
		private readonly TranslatorService _localizationService;
		private readonly MainMenuView _mainMenuView;

		public AuthorizationFactory(
			AssetFactory assetFactory,
			ICloudServiceSdk cloudServiceSdk,
			MainMenuView mainMenuView,
			TranslatorService localizationService
		)
		{
			_assetFactory = assetFactory ?? throw new ArgumentNullException(nameof(assetFactory));

			_cloudServiceSdk = cloudServiceSdk;
			_localizationService = localizationService ?? throw new ArgumentNullException(nameof(localizationService));
			_mainMenuView = mainMenuView ? mainMenuView : throw new ArgumentNullException(nameof(mainMenuView));
		}

		public AuthorizationPresenter Create()
		{
			IAuthorizationView authorizationView = new AuthorizationViewFactory(
				_assetFactory,
				_localizationService,
				_mainMenuView
			).Create();

			AuthorizationPresenter authorizationPresenter =
				new AuthorizationPresenter(
					_cloudServiceSdk,
					authorizationView
				);

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