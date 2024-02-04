using System;
using System.Collections.Generic;
using Sources.Controllers;
using Sources.Presentation.Common;
using Sources.PresentationInterfaces;
using Sources.PresentersInterfaces;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Sources.Presentation.UI.Shop
{
	[RequireComponent(typeof(UpgradeWindow))]
	public class UpgradeWindow : PresentableView<IUpgradeWindowPresenter>, IUpgradeWindow
	{
		[SerializeField] private TmpPhrases _phrases;

		[SerializeField] private GameObject _content;
		[SerializeField] private TextMeshProUGUI _money;

		[SerializeField] private Button _closeMenuButton;
		[SerializeField] private Button _noButton;
		[SerializeField] private GameObject _yesNoButtons;
		private IUpgradeWindow _upgradeWindowImplementation;

		public Transform ContainerTransform => _content.transform;

		public List<string> Phrases
		{
			get => _phrases.Phrases;
			set => _phrases.Phrases = value;
		}

		public event Action<bool> ActiveChanged;

		public event Action Destroyed;

		public void Construct(IUpgradeWindowPresenter presenter)
		{
			base.Construct(presenter);
		}

		private void Awake() =>
			enabled = false;

		public void OnEnable() =>
			Presenter.Enable();

		public void OnDisable() =>
			Presenter.Disable();

		public void OnDestroy() =>
			Destroyed?.Invoke();

		public void SetActiveYesNoButtons(bool isActive) =>
			_yesNoButtons.gameObject.SetActive(isActive);

		// private void Construct(IResourceReadOnly<int> resource)
		// {
		// 	resource.ResourceChanged += OnMoneyChanged;
		//
		// 	_money.text = resource.Count.ToString();
		//
		// 	_closeMenuButton.onClick.AddListener(OnEnableJoystick);
		// 	_noButton.onClick.AddListener(OnEnableJoystick);
		// }

		private void OnMoneyChanged(int amount) =>
			_money.text = amount.ToString();

		private void OnEnableJoystick() =>
			Disable();
	}
}