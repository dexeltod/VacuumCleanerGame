using System;
using Sources.ControllersInterfaces;
using Sources.PresentationInterfaces;
using Sources.ServicesInterfaces;
using UnityEngine;
using UnityEngine.UI;

namespace Sources.Presentation.UI.Shop
{
	public class UpgradeWindowActivator : MonoBehaviour, IDisposable, IUpgradeWindowActivator
	{
		[SerializeField] private TextPhrases _phrases;

		[SerializeField] private Button _yes;
		[SerializeField] private Button _no;
		[SerializeField] private GameObject _container;
		private IUpgradeWindowPresenter _presenter;
		private IUpgradeTriggerObserver _upgradeTrigger;

		public ITextPhrases Phrases => _phrases;
		public GameObject Container => _container;

		public void Construct(IUpgradeWindowPresenter upgradeWindowPresentation, IUpgradeTriggerObserver upgradeTrigger)
		{
			_upgradeTrigger = upgradeTrigger ?? throw new ArgumentNullException(nameof(upgradeTrigger));
			_presenter = upgradeWindowPresentation ??
				throw new ArgumentNullException(nameof(upgradeWindowPresentation));
			_yes.onClick.AddListener(OnYes);
			_no.onClick.AddListener(OnNo);

			_upgradeTrigger.TriggerEntered += OnEnter;
		}

		public void Dispose()
		{
			_yes.onClick.RemoveListener(OnYes);
			_no.onClick.RemoveListener(OnNo);
			_upgradeTrigger.TriggerEntered -= OnEnter;
		}

		private void OnEnter(bool isActive) =>
			_container.SetActive(isActive);

		private void OnNo() =>
			_container.SetActive(false);

		private void OnYes()
		{
			_presenter.EnableWindow();
			_container.SetActive(false);
		}
	}
}