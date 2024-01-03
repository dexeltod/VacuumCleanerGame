using System;
using Sources.ApplicationServicesInterfaces;
using UnityEngine;
using UnityEngine.UI;

namespace Sources.Presentation.UI.YandexAuthorization
{
	public class YandexAuthorizationView : MonoBehaviour, IYandexAuthorizationView
	{
		[SerializeField] private GameObject _yesNoButtons;
		[SerializeField] private Button _yesButton;
		[SerializeField] private Button _noButton;

		private bool _isPlayerWantsAuthorize;

		private Action _callback;

		public void Authorize()
		{
			IsWantsAuthorization();
		}

		public void IsWantsAuthorization(
			Action<bool> isPlayerWantsAuthorizeCallback = null,
			Action onProcessCompleted = null
		)
		{
			_yesNoButtons.SetActive(true);

			isPlayerWantsAuthorizeCallback.Invoke(_isPlayerWantsAuthorize);
			onProcessCompleted.Invoke();
		}

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

		private void OnNoButtonClicked()
		{
			_isPlayerWantsAuthorize = false;
			InvokeCallbackAndDisableWindow();
		}

		private void OnYesButtonClicked()
		{
			_isPlayerWantsAuthorize = true;
			InvokeCallbackAndDisableWindow();
		}

		private void InvokeCallbackAndDisableWindow()
		{
			_callback.Invoke();
			DisableWindow();
		}
	}
}