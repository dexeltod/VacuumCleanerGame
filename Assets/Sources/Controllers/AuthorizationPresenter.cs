using System;
using Sources.Controllers.Common;
using Sources.ControllersInterfaces;
using Sources.InfrastructureInterfaces;
using Sources.PresentationInterfaces;

namespace Sources.Controllers
{
	public class AuthorizationPresenter : Presenter, IAuthorizationPresenter
	{
		private readonly ICloudServiceSdkFacade _cloudServiceSdkFacade;
		private readonly IAuthorizationView _authorizationView;

		public AuthorizationPresenter(
			ICloudServiceSdkFacade cloudServiceSdkFacade,
			IAuthorizationView authorizationView
		)
		{
			_cloudServiceSdkFacade = cloudServiceSdkFacade ??
				throw new ArgumentNullException(nameof(cloudServiceSdkFacade));
			_authorizationView = authorizationView ??
				throw new ArgumentNullException(nameof(authorizationView));
		}

		public bool IsAuthorized => _cloudServiceSdkFacade.IsAuthorized;

		public void Authorize() =>
			_authorizationView.Enable();

		public void SetChoice(bool isWants)
		{
			if (isWants == true)
				_cloudServiceSdkFacade.Authorize();

			_authorizationView.Disable();
		}
	}
}