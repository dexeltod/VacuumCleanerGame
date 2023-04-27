using Model.DI;
using Model.Infrastructure.Services.Factories;
using UnityEngine;

namespace Presenter.SceneEntity
{
	public class UpgradeTrigger : MonoBehaviour
	{
		private IUpgradeWindowGetter _upgradeWindowGetter;
		private UpgradeWindow _upgradeWindow;

		private void Awake()
		{
			_upgradeWindowGetter = ServiceLocator.Container.GetSingle<IUpgradeWindowGetter>();
		}

		private void OnTriggerEnter(Collider other)
		{
			if (other.TryGetComponent(out Vacuum vacuum))
			{
				if (_upgradeWindow == null)
					_upgradeWindow = _upgradeWindowGetter.GetUpgradeWindow();

				_upgradeWindow.gameObject.SetActive(true);
			}
		}

		private void OnTriggerExit(Collider other)
		{
			if (other.TryGetComponent(out Vacuum vacuum))
				_upgradeWindow.gameObject.SetActive(false);
		}
	}
}