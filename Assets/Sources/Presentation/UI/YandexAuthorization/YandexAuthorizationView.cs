using System;
using Sources.ApplicationServicesInterfaces;
using Sources.ApplicationServicesInterfaces.Authorization;
using UnityEngine;
using UnityEngine.UI;

namespace Sources.Presentation.UI.YandexAuthorization
{
	public class YandexAuthorizationView : MonoBehaviour, IAuthorization
	{
		[SerializeField] private GameObject _yesNoButtons;
		[SerializeField] private Button _yesButton;
		[SerializeField] private Button _noButton;

		public event Action<bool> AuthorizeCallback;

		public void EnableAuthorizeWindow() =>
			_yesNoButtons.SetActive(true);

		public void DisableAuthorizeWindow() =>
			_yesNoButtons.SetActive(false);

		private void OnEnable()
		{
			_yesButton.onClick.AddListener(OnYesButtonClicked);
			_noButton.onClick.AddListener(OnNoButtonClicked);
		}

		private void OnDisable()
		{
			_yesButton.onClick.RemoveListener(OnYesButtonClicked);
			_noButton.onClick.RemoveListener(OnNoButtonClicked);
		}

		private void DisableWindow() =>
			_yesNoButtons.gameObject.SetActive(false);

		private void OnNoButtonClicked() =>
			AuthorizeCallback.Invoke(false);

		private void OnYesButtonClicked() =>
			AuthorizeCallback.Invoke(true);
	}
}