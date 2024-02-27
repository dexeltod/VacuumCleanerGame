using System;
using Sources.ApplicationServicesInterfaces;
using Sources.Controllers;
using Sources.Infrastructure.Factories.LeaderBoard;
using Sources.Presentation;
using Sources.ServicesInterfaces;

namespace Sources.Infrastructure.StateMachine.GameStates
{
	public sealed class AuthorizationFactory
	{
		private readonly IAssetFactory _assetFactory;

		public AuthorizationFactory(IAssetFactory assetFactory) =>
			_assetFactory = assetFactory ?? throw new ArgumentNullException(nameof(assetFactory));

		public IAuthorizationPresenter Create()
		{
			IAuthorizationView authorizationView = new AuthorizationViewFactory(_assetFactory).Create();

			ICloudPlayerDataService сloudPlayerDataService = new CloudPlayerDataServiceFactory().Create();

			IAuthorizationPresenter authorizationPresenter =
				new AuthorizationPresenter(
					сloudPlayerDataService,
					authorizationView
				);

			authorizationView.Construct(authorizationPresenter);

			сloudPlayerDataService.SetStatusInitialized();

			return authorizationPresenter;
		}
	}
}