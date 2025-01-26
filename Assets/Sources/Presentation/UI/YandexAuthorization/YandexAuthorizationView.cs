using Sources.ControllersInterfaces;
using Sources.Presentation.Common;
using Sources.PresentationInterfaces;
using UnityEngine;
using UnityEngine.UI;

namespace Sources.Presentation.UI.YandexAuthorization
{
	public class YandexAuthorizationView : PresentableCanvas<IAuthorizationPresenter>, IAuthorizationView
	{
		[SerializeField] private GameObject _yesNoButtons;
		[SerializeField] private Button _yesButton;
		[SerializeField] private Button _noButton;

		private IAuthorizationPresenter AuthorizationPresenter => Presenter;

		public override void Enable()
		{
			enabled = true;
			_yesButton.onClick.AddListener(OnYesButtonClicked);
			_noButton.onClick.AddListener(OnNoButtonClicked);
		}

		public override void Disable()
		{
			enabled = false;
			_yesButton.onClick.RemoveListener(OnYesButtonClicked);
			_noButton.onClick.RemoveListener(OnNoButtonClicked);
		}

		private void OnNoButtonClicked() => AuthorizationPresenter.SetChoice(false);

		private void OnYesButtonClicked() => AuthorizationPresenter.SetChoice(true);
	}
}