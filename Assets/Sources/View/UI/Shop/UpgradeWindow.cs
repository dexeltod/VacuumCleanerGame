using System;
using System.Collections.Generic;
using Sources.DIService;
using Sources.DomainInterfaces.DomainServicesInterfaces;
using Sources.Services.Interfaces;
using Sources.ServicesInterfaces;
using Sources.ServicesInterfaces.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Sources.View.UI.Shop
{
	[RequireComponent(typeof(UpgradeWindow))]
	public class UpgradeWindow : MonoBehaviour, IUpgradeWindow
	{
		[SerializeField] private GameObject      _content;
		[SerializeField] private TextMeshProUGUI _money;

		[SerializeField] private Button     _closeMenuButton;
		[SerializeField] private Button     _noButton;
		[SerializeField] private GameObject _yesNoButtons;

		private IUIGetter                  _uiGetter;
		private List<UpgradeElementPrefab> _buttons;

		public Transform ContainerTransform => _content.transform;

		public event Action<bool> ActiveChanged;
		public event Action       Destroyed;

		private void Awake() =>
			enabled = false;

		public void OnEnable()
		{
			_uiGetter.GameplayInterface.Canvas.enabled = false;
			ActiveChanged?.Invoke(true);
		}

		public void OnDisable()
		{
			if (_uiGetter.GameplayInterface.Canvas != null)
				_uiGetter.GameplayInterface.Canvas.enabled = true;

			ActiveChanged?.Invoke(false);
		}

		public void OnDestroy() =>
			Destroyed?.Invoke();

		public void SetActiveYesNoButtons(bool isActive) =>
			_yesNoButtons.gameObject.SetActive(isActive);

		public void Construct(IResource<int> resource)
		{
			_uiGetter = GameServices.Container.Get<IUIGetter>();
			
			resource.ResourceChanged += OnMoneyChanged;

			_money.text = resource.Count.ToString();
			Debug.Log
			(
				resource.Count.ToString()
			);

			_closeMenuButton.onClick.AddListener(OnEnableJoystick);
			_noButton.onClick.AddListener(OnEnableJoystick);
		}

		~UpgradeWindow()
		{
			_closeMenuButton.onClick.RemoveListener(OnEnableJoystick);
			_noButton.onClick.RemoveListener(OnEnableJoystick);
		}

		private void OnMoneyChanged(int amount) =>
			_money.text = amount.ToString();

		private void OnEnableJoystick() =>
			_uiGetter.GameplayInterface.Joystick.enabled = true;
	}
}