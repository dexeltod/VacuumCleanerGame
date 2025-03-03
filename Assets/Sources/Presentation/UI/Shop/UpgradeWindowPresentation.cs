using System;
using System.Collections.Generic;
using Sources.ControllersInterfaces;
using Sources.Presentation.Common;
using Sources.PresentationInterfaces;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Sources.Presentation.UI.Shop
{
	public class UpgradeWindowPresentation : PresentableView<IUpgradeWindowPresenter>,
		IUpgradeWindowPresentation
	{
		[FormerlySerializedAs("_phrases")]
		[SerializeField]
		private TextPhrasesList _phrasesList;

		[SerializeField] private GameObject _content;
		[SerializeField] private TextMeshProUGUI _money;

		[SerializeField] private Button _closeMenuButton;
		[SerializeField] private GameObject _upgradeWindowMain;
		[SerializeField] private AudioSource _audio;

		public Button CloseMenuButton => _closeMenuButton;
		public GameObject UpgradeWindowMain => _upgradeWindowMain;
		public AudioSource AudioClose => _audio;

		public Transform ContainerTransform => _content.transform;

		public List<string> Phrases
		{
			get => _phrasesList.Phrases;
			set => _phrasesList.Phrases = value;
		}

		public void Construct(IUpgradeWindowPresenter presenter, int money)
		{
			if (presenter == null) throw new ArgumentNullException(nameof(presenter));
			if (money < 0) throw new ArgumentOutOfRangeException(nameof(money));

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
			enabled = true;
		}

		public override void Disable()
		{
			enabled = false;
		}
	}
}
