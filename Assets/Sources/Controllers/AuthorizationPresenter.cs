using System;
using Sources.ApplicationServicesInterfaces;
using Sources.Controllers.Common;
using Sources.Presentation;

namespace Sources.Controllers
{
	public class AuthorizationPresenter : Presenter, IAuthorizationPresenter
	{
		private readonly ICloudPlayerDataService _cloudPlayerDataService;
		private readonly IAuthorizationView _authorizationView;

		public AuthorizationPresenter(
			ICloudPlayerDataService cloudPlayerDataService,
			IAuthorizationView authorizationView
		)
		{
			_cloudPlayerDataService = cloudPlayerDataService ??
				throw new ArgumentNullException(nameof(cloudPlayerDataService));
			_authorizationView = authorizationView ??
				throw new ArgumentNullException(nameof(authorizationView));
		}

		public void Authorize() =>
			_authorizationView.Enable();

		public void SetChoice(bool isWants)
		{
			if (isWants)
				_cloudPlayerDataService.Authorize();

			_authorizationView.Disable();
		}
	}
}