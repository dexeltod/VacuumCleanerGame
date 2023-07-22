using System;
using System.Collections.Generic;
using Sources.Core.DI;
using Sources.View.Interfaces;
using Sources.View.Services.UI;
using Sources.View.UI.Shop;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Sources.View.SceneEntity
{
	[RequireComponent(typeof(UpgradeWindow))]
	public class UpgradeWindow : MonoBehaviour, IUpgradeWindow
	{
		[SerializeField] private GameObject _content;

		[SerializeField] private Button _closeMenuButton;
		[SerializeField] private Button _noButton;
		[SerializeField] private GameObject _yesNoButtons;
		[SerializeField] private TextMeshProUGUI _money;

		private IUIGetter _gameplayInterface;
		private List<UpgradeElementView> _buttons;
		public Transform UpgradeElementsTransform => _content.transform;

		public event Action<bool> ActiveChanged;
		public event Action Destroyed;

		private void Awake() => 
			GetComponent<UpgradeWindow>().enabled = false;

		public void OnEnable()
		{
			_gameplayInterface.Canvas.enabled = false;
			ActiveChanged?.Invoke(true);
		}

		public void OnDisable()
		{
			if (_gameplayInterface.Canvas != null)
				_gameplayInterface.Canvas.enabled = true;
			
			ActiveChanged?.Invoke(false);
		}

		public void OnDestroy() =>
			Destroyed?.Invoke();

		~UpgradeWindow()
		{
			_closeMenuButton.onClick.RemoveListener(OnEnableJoystick);
			_noButton.onClick.RemoveListener(OnEnableJoystick);
		}

		public void SetActiveYesNoButtons(bool isActive) =>
			_yesNoButtons.gameObject.SetActive(isActive);

		public void Construct()
		{
			_gameplayInterface = ServiceLocator.Container.Get<IUIGetter>();
			_money.text = "";
			_closeMenuButton.onClick.AddListener(OnEnableJoystick);
			_noButton.onClick.AddListener(OnEnableJoystick);
		}

		private void OnEnableJoystick() =>
			_gameplayInterface.Joystick.enabled = true;
	}
}