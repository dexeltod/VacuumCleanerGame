using Application.DI;
using InfrastructureInterfaces;
using UnityEngine;

namespace View.SceneEntity.Triggers
{
	public class UpgradeTrigger : MonoBehaviour
	{
		private IUpgradeWindowGetter _upgradeWindowGetter;
		private UpgradeWindow _upgradeWindow;
		private ISceneLoadInformer _sceneLoadInformer;

		private void Start()
		{
			_sceneLoadInformer = ServiceLocator.Container.GetSingle<ISceneLoadInformer>();
			_sceneLoadInformer.SceneLoaded += OnLoaded;			
		}

		private void OnLoaded()
		{
			_upgradeWindowGetter = ServiceLocator.Container.GetSingle<IUpgradeWindowGetter>();
			_upgradeWindow = _upgradeWindowGetter.UpgradeWindow;
			_sceneLoadInformer.SceneLoaded -= OnLoaded;
		}

		private void OnTriggerEnter(Collider other)
		{
			if (other.TryGetComponent(out Player vacuum))
			{
				_upgradeWindow.SetActiveYesNoButtons(true);
			}
		}
		private void OnTriggerExit(Collider other)
		{
			if (other.TryGetComponent(out Player vacuum))
			{
				_upgradeWindow.SetActiveYesNoButtons(false);
			}
		}
	}
}