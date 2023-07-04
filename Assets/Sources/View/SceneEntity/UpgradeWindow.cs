using System;
using System.Collections.Generic;
using Application.DI;
using InfrastructureInterfaces;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using View.UI.Shop;

namespace View.SceneEntity
{
	public class UpgradeWindow : MonoBehaviour
	{
		[SerializeField] private GameObject _upgradeElementsContainer;

		[SerializeField] private Button _closeMenuButton;
		[SerializeField] private Button _noButton;
		[SerializeField] private GameObject _yesNoButtons;
		[SerializeField] private TextMeshProUGUI _money;

		private IUIGetter _gameplayInterface;
		private List<UpgradeElementView> _buttons;
		public Transform UpgradeElementsTransform => _upgradeElementsContainer.transform;

		public event Action<bool> ActiveChanged;
		public event Action Destroyed;

		private void OnEnable()
		{
			_gameplayInterface.Canvas.enabled = false;
			ActiveChanged?.Invoke(true);
		}

		private void OnDisable()
		{
			if (_gameplayInterface.Canvas != null)
				_gameplayInterface.Canvas.enabled = true;
			
			ActiveChanged?.Invoke(false);
		}

		private void OnDestroy() =>
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
			_gameplayInterface = ServiceLocator.Container.GetSingle<IUIGetter>();
			_money.text = "";
			_closeMenuButton.onClick.AddListener(OnEnableJoystick);
			_noButton.onClick.AddListener(OnEnableJoystick);
		}

		private void OnEnableJoystick() =>
			_gameplayInterface.Joystick.enabled = true;
	}
}