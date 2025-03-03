using System;
using Sources.ControllersInterfaces;
using Sources.Domain.Interfaces;
using Sources.PresentationInterfaces;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Sources.Presentation.UI.Shop
{
	public class UpgradeWindowActivator : MonoBehaviour, IDisposable, IUpgradeWindowActivator
	{
		[FormerlySerializedAs("_phrases")]
		[SerializeField]
		private TextPhrasesList _phrasesList;

		[SerializeField] private Button _yes;
		[SerializeField] private Button _no;
		[SerializeField] private GameObject _container;
		[SerializeField] private AudioSource _audioSource;

		private IUpgradeWindowPresenter _presenter;
		private IUpgradeTriggerObserver _upgradeTrigger;

		public void Dispose()
		{
			_yes.onClick.RemoveListener(OnYes);
			_no.onClick.RemoveListener(OnNo);
			_upgradeTrigger.TriggerEntered -= OnEnter;
		}

		public ITextPhrases PhrasesList => _phrasesList;
		public GameObject Container => _container;

		public void Construct(IUpgradeWindowPresenter upgradeWindowPresentation, IUpgradeTriggerObserver upgradeTrigger)
		{
			_upgradeTrigger = upgradeTrigger ?? throw new ArgumentNullException(nameof(upgradeTrigger));
			_presenter = upgradeWindowPresentation ?? throw new ArgumentNullException(nameof(upgradeWindowPresentation));

			_yes.onClick.AddListener(OnYes);
			_no.onClick.AddListener(OnNo);

			_upgradeTrigger.TriggerEntered += OnEnter;
		}

		private void OnEnter(bool isActive) => _container.SetActive(isActive);

		private void OnNo()
		{
			_audioSource.Play();
			_container.SetActive(false);
		}

		private void OnYes()
		{
			_audioSource.Play();

			_presenter.Enable();
			_container.SetActive(false);
		}
	}
}
