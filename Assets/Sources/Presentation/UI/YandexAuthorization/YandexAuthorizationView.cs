using System;
using Sources.ApplicationServicesInterfaces;
using Sources.Controllers;
using Sources.Presentation.Common;
using UnityEngine;
using UnityEngine.UI;

namespace Sources.Presentation.UI.YandexAuthorization
{
	public class YandexAuthorizationView : PresentableView<IAuthorizationPresenter>, IAuthorizationView
	{
		[SerializeField] private GameObject _yesNoButtons;
		[SerializeField] private Button _yesButton;
		[SerializeField] private Button _noButton;

		private ICloudPlayerDataService _cloudPlayerDataService;
		private AuthorizationPresenter _authorizationPresenter;

		public void Construct(AuthorizationPresenter authorizationPresenter)
		{
			base.Construct(authorizationPresenter);
		}

		public override void Enable()
		{
			_yesNoButtons.SetActive(true);
			_yesButton.onClick.AddListener(OnYesButtonClicked);
			_noButton.onClick.AddListener(OnNoButtonClicked);
		}

		public override void Disable()
		{
			_yesNoButtons.SetActive(false);
			_yesButton.onClick.RemoveListener(OnYesButtonClicked);
			_noButton.onClick.RemoveListener(OnNoButtonClicked);
		}

		private void OnNoButtonClicked() =>
			Presenter.SetChoice(false);

		private void OnYesButtonClicked() =>
			Presenter.SetChoice(true);
	}
}