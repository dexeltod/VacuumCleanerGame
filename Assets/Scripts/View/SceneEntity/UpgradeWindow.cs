using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Model.DI;
using Model.Infrastructure.Services.Factories;
using Model.ScriptableObjects.UpgradeItems.SO;
using UnityEngine;
using UnityEngine.UI;

namespace Presenter.SceneEntity
{
	public class UpgradeWindow : MonoBehaviour
	{
		[SerializeField] private GameObject _upgradeElementsContainer;

		[SerializeField] private Button _closeMenuButton;
		[SerializeField] private Button _noButton;
		[SerializeField] private GameObject _yesNoButtons;

		private IUIGetter _gameplayInterface;
		private List<UpgradeElement> _buttons;
		public GameObject UpgradeElementsContainer => _upgradeElementsContainer;

		~UpgradeWindow()
		{
			_closeMenuButton.onClick.RemoveListener(OnEnableJoystick);
			_noButton.onClick.RemoveListener(OnEnableJoystick);
			UnsubscribeButtons(_buttons);
		}

		public void SetActiveYesNoButtons(bool isActive) =>
			_yesNoButtons.gameObject.SetActive(isActive);

		public void Construct(List<UpgradeElement> buttons)
		{
			_buttons = buttons;
			_gameplayInterface = ServiceLocator.Container.GetSingle<IUIGetter>();
			_closeMenuButton.onClick.AddListener(OnEnableJoystick);
			_noButton.onClick.AddListener(OnEnableJoystick);
			SubscribeOnButtons(_buttons);
		}

		private void OnButtonPressed(UpgradeItemScriptableObject.Upgrade type)
		{
			Debug.Log(type);
		}

		private void SubscribeOnButtons(List<UpgradeElement> elements)
		{
			foreach (var element in elements)
				element.BuyButtonPressed += OnButtonPressed;
		}

		private void UnsubscribeButtons(List<UpgradeElement> elements)
		{
			foreach (var element in elements)
				element.BuyButtonPressed -= OnButtonPressed;
		}

		private void OnEnableJoystick() =>
			_gameplayInterface.Joystick.enabled = true;
	}
}