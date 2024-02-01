using Sources.DomainInterfaces;
using Sources.Presentation;

namespace Sources.ServicesInterfaces.UI
{
	public interface IUpgradeWindowPresenter
	{
		public void Enable();
		public void Disable();

		void Initialize(
			IUpgradeTriggerObserver observer,
			IUpgradeWindow upgradeWindow,
			IProgressLoadDataService progressLoadDataService
		);
	}
}