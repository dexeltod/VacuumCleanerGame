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
	public class UpgradeWindowPresentation : PresentableView<IUpgradeWindowPresenter>,
		IUpgradeWindowPresentation
	{
		[SerializeField] private TextPhrases _phrases;

		[SerializeField] private GameObject _content;
		[SerializeField] private TextMeshProUGUI _money;

		[SerializeField] private Button _closeMenuButton;
		[SerializeField] private GameObject _upgradeWindowMain;
		[SerializeField] private AudioSource _audio;

		private IUpgradeWindowActivator _activator;

		public Button CloseMenuButton => _closeMenuButton;
		public GameObject UpgradeWindowMain => _upgradeWindowMain;
		public AudioSource AudioSource => _audio;

		public Transform ContainerTransform => _content.transform;

		public List<string> Phrases
		{
			get => _phrases.Phrases;
			set => _phrases.Phrases = value;
		}

		public void Construct(IUpgradeWindowPresenter presenter, int money, IUpgradeWindowActivator activator)
		{
			if (presenter == null) throw new ArgumentNullException(nameof(presenter));
			if (money < 0) throw new ArgumentOutOfRangeException(nameof(money));

			_activator = activator ?? throw new ArgumentNullException(nameof(activator));
			_money.SetText(money.ToString());

			base.Construct(presenter);
		}

		public void SetMoney(int money)
		{
			if (money < 0) throw new ArgumentOutOfRangeException(nameof(money));
			_money.SetText(money.ToString());
		}

		public override void Enable()
		{
			base.Enable();
			enabled = true;
		}

		public override void Disable()
		{
			base.Disable();
			enabled = false;
		}

		public void SetActiveYesNoButtons(bool isActive) =>
			_activator.Container.SetActive(isActive);
	}
}