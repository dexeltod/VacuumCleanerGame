using Sources.ControllersInterfaces;
using Sources.Presentation;
using Sources.Presentation.Common;
using Sources.PresentationInterfaces;
using UnityEngine;

namespace Sources.Boot.Scripts.Factories.Presentation.UI
{
	public class EditorAuthorizationView : PresentableCanvas<IAuthorizationPresenter>, IAuthorizationView
	{
		[SerializeField] private ChooseWindow _chooseWindow;

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