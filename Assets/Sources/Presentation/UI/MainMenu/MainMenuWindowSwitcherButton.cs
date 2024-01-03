using UnityEngine;
using UnityEngine.UI;

namespace Sources.Presentation.UI.MainMenu
{
	[RequireComponent(typeof(Button))] public class MainMenuWindowSwitcherButton : MonoBehaviour
	{
		[SerializeField] private GameObject _disabledObject;
		[SerializeField] private GameObject _enabledObject;

		private Button _button;

		public void Awake() =>
			_button = GetComponent<Button>();

		private void OnEnable() =>
			_button.onClick.AddListener(OnSwitchWindow);

		private void OnDisable() =>
			_button.onClick.RemoveListener(OnSwitchWindow);

		private void OnSwitchWindow()
		{
			_disabledObject.SetActive(false);
			_enabledObject.SetActive(true);
		}
	}
}