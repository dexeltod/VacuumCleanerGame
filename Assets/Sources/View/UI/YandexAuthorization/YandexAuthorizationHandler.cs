using System;
using Sources.PresentationInterfaces;
using UnityEngine;
using UnityEngine.UI;

public class YandexAuthorizationHandler : MonoBehaviour, IYandexAuthorizationHandler
{
	[SerializeField] private GameObject _loadingText;
	[SerializeField] private GameObject _yesNoButtons;
	[SerializeField] private Button _yesButton;
	[SerializeField] private Button _noButton;

	private Action<bool> _isPlayerWantsAuthorize;
	private Action _callback;

	private void OnEnable()
	{
		_loadingText.SetActive(false);

		_yesButton.onClick.AddListener(OnYesButtonClicked);
		_noButton.onClick.AddListener(OnNoButtonClicked);
	}

	private void OnDisable()
	{
		_loadingText.SetActive(true);

		_yesButton.onClick.RemoveListener(OnYesButtonClicked);
		_noButton.onClick.RemoveListener(OnNoButtonClicked);
	}

	private void DisableWindow()
	{
		_loadingText.gameObject.SetActive(true);
		_yesNoButtons.gameObject.SetActive(false);
	}

	public void IsWantsAuthorization(Action<bool> isPlayerWantsAuthorize, Action callback = null)
	{
		_loadingText.SetActive(false);
		_yesNoButtons.SetActive(true);
		
		_callback = callback;
		_isPlayerWantsAuthorize = isPlayerWantsAuthorize;
	}

	private void OnNoButtonClicked()
	{
		_isPlayerWantsAuthorize.Invoke(false);
		_callback.Invoke();
		DisableWindow();
	}

	private void OnYesButtonClicked()
	{
		_isPlayerWantsAuthorize.Invoke(true);
		_callback.Invoke();
		DisableWindow();
	}
}