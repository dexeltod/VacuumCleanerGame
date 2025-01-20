using Sources.ControllersInterfaces;
using Sources.Presentation.Common;
using Sources.PresentationInterfaces;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Sources.Presentation
{
	public class GameMenuView : PresentableView<IGameMenuPresenter>, IGameMenuView
	{
		[SerializeField] private Button _openMenuButton;
		[SerializeField] private Button _closeMenuButton;
		[SerializeField] private Button _goToMainMenuButton;

		[SerializeField] private GameObject _menuWindow;

		[FormerlySerializedAs("_audio")]
		[SerializeField]
		private AudioSource _audioSource;

		public Button OpenMenuButton => _openMenuButton;
		public AudioSource AudioSource => _audioSource;

		public override void Enable()
		{
			base.Enable();
			Presenter.Enable();
			_openMenuButton.onClick.AddListener(() => OnSetActiveMenuWindow(true));
			_closeMenuButton.onClick.AddListener(() => OnSetActiveMenuWindow(false));
			_goToMainMenuButton.onClick.AddListener(OnGoToMainMenu);
		}

		public override void Disable()
		{
			base.Disable();
			Presenter.Disable();

			_goToMainMenuButton.onClick.RemoveListener(OnGoToMainMenu);
			_openMenuButton.onClick.RemoveListener(() => OnSetActiveMenuWindow(true));
			_closeMenuButton.onClick.RemoveListener(() => OnSetActiveMenuWindow(false));
		}

		private void OnGoToMainMenu()
		{
			_audioSource.Play();
			Presenter.GoToMainMenu();
		}

		private void OnSetActiveMenuWindow(bool isActive)
		{
			_audioSource.Play();
			_openMenuButton.gameObject.SetActive(!isActive);
			_menuWindow.gameObject.SetActive(isActive);
		}
	}
}
