using UnityEngine;
using UnityEngine.UI;

namespace Sources.Presentation.UI.MainMenu
{
	[RequireComponent(typeof(Button))] public class WindowSwitcherButton : MonoBehaviour
	{
		[SerializeField] private GameObject[] _disabledObject;
		[SerializeField] private GameObject[] _enabledObject;

		private Button _button;

		public void Awake() =>
			_button = GetComponent<Button>();

		private void OnEnable() =>
			_button.onClick.AddListener(OnSwitchWindow);

		private void OnDisable() =>
			_button.onClick.RemoveListener(OnSwitchWindow);

		private void OnSwitchWindow()
		{
			foreach (GameObject target in _enabledObject)
				target.SetActive(true);

			foreach (GameObject target in _disabledObject)
				target.SetActive(false);
		}
	}
}