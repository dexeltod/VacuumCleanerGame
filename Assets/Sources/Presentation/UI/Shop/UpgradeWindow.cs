using System;
using System.Collections.Generic;
using Sources.ControllersInterfaces;
using Sources.Presentation.Common;
using Sources.PresentationInterfaces;
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

		public Transform ContainerTransform => _content.transform;

		public List<string> Phrases
		{
			get => _phrases.Phrases;
			set => _phrases.Phrases = value;
		}

		public void Construct(IUpgradeWindowPresenter presenter, int money)
		{
			if (presenter == null) throw new ArgumentNullException(nameof(presenter));
			if (money < 0) throw new ArgumentOutOfRangeException(nameof(money));

			_money.SetText(money.ToString());
			base.Construct(presenter);
		}

		public void OnEnable() =>
			Presenter.Enable();

		public void OnDisable() =>
			Presenter.Disable();

		public void SetActiveYesNoButtons(bool isActive) =>
			_yesNoButtons.gameObject.SetActive(isActive);

		private void OnEnableJoystick() =>
			Disable();

		private void Awake() =>
			enabled = false;
	}
}