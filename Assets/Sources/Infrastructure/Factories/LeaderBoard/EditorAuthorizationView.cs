using Sources.ApplicationServicesInterfaces;
using Sources.Controllers;
using Sources.Presentation;
using Sources.Presentation.Common;
using UnityEngine;

namespace Sources.Infrastructure.Factories.LeaderBoard
{
	public class EditorAuthorizationView : PresentableCanvas<IAuthorizationPresenter>, IAuthorizationView
	{
		[SerializeField] private ChooseWindow _chooseWindow;

		private ICloudPlayerDataService _cloudPlayerDataService;
		private AuthorizationPresenter _authorizationPresenter;

		public override void Enable()
		{
			base.Enable();
			_chooseWindow.gameObject.SetActive(true);
			_chooseWindow.YesButton.onClick.AddListener(OnYesButtonClicked);
			_chooseWindow.NoButton.onClick.AddListener(OnNoButtonClicked);
		}

		public override void Disable()
		{
			base.Disable();
			_chooseWindow.gameObject.SetActive(false);
			_chooseWindow.YesButton.onClick.AddListener(OnYesButtonClicked);
			_chooseWindow.NoButton.onClick.AddListener(OnNoButtonClicked);
		}

		private void OnNoButtonClicked() =>
			Presenter.SetChoice(false);

		private void OnYesButtonClicked() =>
			Presenter.SetChoice(true);
	}
}