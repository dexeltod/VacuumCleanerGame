using UnityEngine;
using UnityEngine.UI;

namespace Sources.Presentation.UI.MainMenu
{
	[RequireComponent(typeof(Button))]
	public class WindowSwitcherButton : MonoBehaviour
	{
		[SerializeField] private GameObject[] _disabledObject;
		[SerializeField] private GameObject[] _enabledObject;
		[SerializeField] private AudioSource _audioSource;

		private Button _button;

		public void Awake() =>
			_button = GetComponent<Button>();

		private void OnEnable() =>
			_button.onClick.AddListener(OnSwitchWindow);

		private void OnDisable() =>
			_button.onClick.RemoveListener(OnSwitchWindow);

		private void OnSwitchWindow()
		{
			if (_audioSource != null) _audioSource.Play();
			else Debug.Log($"Cant play sound. No audio source in {gameObject.name}");

			foreach (GameObject target in _enabledObject)
				target.SetActive(true);

			foreach (GameObject target in _disabledObject)
				target.SetActive(false);
		}
	}
}