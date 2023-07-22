using Sources.Core.DI;
using Sources.Infrastructure.InfrastructureInterfaces;
using Sources.View.Interfaces;
using Sources.View.Services.UI;
using UnityEngine;

namespace Sources.View.SceneEntity.Triggers
{
	public class UpgradeTrigger : MonoBehaviour
	{
		private IUpgradeWindowGetter _upgradeWindowGetter;
		private IUpgradeWindow _upgradeWindow;
		private ISceneLoadInformer _sceneLoadInformer;

		private void Start()
		{
			_sceneLoadInformer = ServiceLocator.Container.Get<ISceneLoadInformer>();
			_sceneLoadInformer.SceneLoaded += OnLoaded;			
		}

		private void OnLoaded()
		{
			_upgradeWindowGetter = ServiceLocator.Container.Get<IUpgradeWindowGetter>();
			_upgradeWindow = _upgradeWindowGetter.UpgradeWindow;
			_sceneLoadInformer.SceneLoaded -= OnLoaded;
		}

		private void OnTriggerEnter(Collider other)
		{
			if (other.TryGetComponent(out Player _)) 
				_upgradeWindow.SetActiveYesNoButtons(true);
		}
		
		private void OnTriggerExit(Collider other)
		{
			if (other.TryGetComponent(out Player _)) 
				_upgradeWindow.SetActiveYesNoButtons(false);
		}
	}
}