using System;
using Sources.Controllers;
using Sources.ControllersInterfaces;
using Sources.Infrastructure.Factories.Domain;
using Sources.Infrastructure.Providers;
using Sources.InfrastructureInterfaces;
using Sources.Presentation.UI;
using Sources.PresentationInterfaces;
using Sources.Services.Localization;
using Sources.ServicesInterfaces;
using UnityEngine;
using VContainer;

namespace Sources.Infrastructure.Factories.Authorization
{
	public sealed class AuthorizationFactory
	{
		private readonly IAssetFactory _assetFactory;
		private readonly ICloudServiceSdk _cloudServiceSdk;
		private readonly ITranslatorService _localizationService;
		private readonly MainMenuView _mainMenuView;

		public AuthorizationFactory(
			IAssetFactory assetFactory,
			ICloudServiceSdk cloudServiceSdk,
			MainMenuView mainMenuView,
			ITranslatorService localizationService
		)
		{
			_assetFactory = assetFactory ??
			                throw new ArgumentNullException(
				                nameof(assetFactory)
			                );

			_cloudServiceSdk = cloudServiceSdk;
			_localizationService = localizationService ??
			                       throw new ArgumentNullException(
				                       nameof(localizationService)
			                       );
			_mainMenuView = mainMenuView
				? mainMenuView
				: throw new ArgumentNullException(
					nameof(mainMenuView)
				);
		}

		public IAuthorizationPresenter Create()
		{
			IAuthorizationView authorizationView = new AuthorizationViewFactory(
				_assetFactory,
				_localizationService,
				_mainMenuView
			).Create();

			IAuthorizationPresenter authorizationPresenter =
				new AuthorizationPresenter(
					_cloudServiceSdk,
					authorizationView
				);

			Construct(
				authorizationView,
				authorizationPresenter
			);

			// сloudServiceSdkFacade.SetStatusInitialized();

			return authorizationPresenter;
		}

		private void Construct(IAuthorizationView authorizationView, IAuthorizationPresenter authorizationPresenter)
		{
			authorizationView.Construct(
				authorizationPresenter
			);
			authorizationView.RectTransform.localPosition = Vector2.zero;
			authorizationView.Disable();
		}
	}
}
