using System;
using Sources.Application.Bootstrapp;
using Sources.ApplicationServicesInterfaces;
using Sources.Controllers;
using Sources.Infrastructure.Factories.LeaderBoard;
using Sources.Presentation;
using Sources.Presentation.UI;
using Sources.Services.Localization;
using Sources.ServicesInterfaces;
using UnityEngine;

namespace Sources.Infrastructure.StateMachine.GameStates
{
	public sealed class AuthorizationFactory
	{
		private readonly IAssetFactory _assetFactory;
		private readonly CloudServiceSdkFacadeProvider _cloudServiceSdkFacadeProvider;
		private readonly ITranslatorService _localizationService;
		private readonly MainMenuView _mainMenuView;

		public AuthorizationFactory(
			IAssetFactory assetFactory,
			CloudServiceSdkFacadeProvider cloudServiceSdkFacadeProvider,
			MainMenuView mainMenuView,
			ITranslatorService localizationService
		)
		{
			_assetFactory = assetFactory ?? throw new ArgumentNullException(nameof(assetFactory));
			_cloudServiceSdkFacadeProvider = cloudServiceSdkFacadeProvider;
			_localizationService = localizationService ?? throw new ArgumentNullException(nameof(localizationService));
			_mainMenuView = mainMenuView ? mainMenuView : throw new ArgumentNullException(nameof(mainMenuView));
		}

		public IAuthorizationPresenter Create()
		{
			
				IAuthorizationView authorizationView = new AuthorizationViewFactory(_assetFactory, _localizationService).Create();

				ICloudPlayerDataService сloudPlayerDataService = new CloudPlayerDataServiceFactory().Create();

				IAuthorizationPresenter authorizationPresenter =
					new AuthorizationPresenter(
						сloudPlayerDataService,
						authorizationView
					);

				Construct(authorizationView, authorizationPresenter);

				сloudPlayerDataService.SetStatusInitialized();

				_cloudServiceSdkFacadeProvider.Register(сloudPlayerDataService);

				return authorizationPresenter;
		}

		private void Construct(IAuthorizationView authorizationView, IAuthorizationPresenter authorizationPresenter)
		{
			authorizationView.Construct(authorizationPresenter);
			authorizationView.SetParent(_mainMenuView.transform);
			authorizationView.RectTransform.localPosition = Vector2.zero;
			authorizationView.Disable();
		}
	}
}