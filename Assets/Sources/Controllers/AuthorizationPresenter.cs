using System;
using Sources.BusinessLogic.Interfaces;
using Sources.Controllers.Common;
using Sources.ControllersInterfaces;
using Sources.PresentationInterfaces;

namespace Sources.Controllers
{
	public class AuthorizationPresenter : Presenter, IAuthorizationPresenter
	{
		private readonly ICloudServiceSdk _cloudServiceSdk;
		private readonly IAuthorizationView _authorizationView;

		public AuthorizationPresenter(
			ICloudServiceSdk cloudServiceSdk,
			IAuthorizationView authorizationView
		)
		{
			_cloudServiceSdk = cloudServiceSdk ??
			                   throw new ArgumentNullException(nameof(cloudServiceSdk));
			_authorizationView = authorizationView ??
			                     throw new ArgumentNullException(nameof(authorizationView));
		}

		public bool IsAuthorized => _cloudServiceSdk.IsAuthorized;

		public void Authorize() =>
			_authorizationView.Enable();

		public void SetChoice(bool isWants)
		{
			if (isWants)
				_cloudServiceSdk.Authorize();

			_authorizationView.Disable();
		}
	}
}
