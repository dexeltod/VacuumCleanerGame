using Sources.ControllersInterfaces;
using Sources.ServicesInterfaces;
using UnityEngine;

namespace Sources.PresentationInterfaces
{
	public interface IUpgradeWindowActivator
	{
		void Construct(IUpgradeWindowPresenter upgradeWindowPresentation, IUpgradeTriggerObserver upgradeTrigger);
		GameObject Container { get; }
		ITextPhrases Phrases { get; }
	}
}