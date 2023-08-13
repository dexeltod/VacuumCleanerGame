using Sources.DIService;
using Sources.InfrastructureInterfaces;
using Sources.Services.PlayerServices;
using Sources.ServicesInterfaces.UI;
using UnityEngine;

namespace Sources.View
{
	public class UpgradeTrigger : MonoBehaviour
	{
		private IUpgradeWindowGetter _upgradeWindowGetter;
		private IUpgradeWindow _upgradeWindow;
		private ISceneLoadInformer _sceneLoadInformer;

		private void Start()
		{
			_sceneLoadInformer = GameServices.Container.Get<ISceneLoadInformer>();
			_sceneLoadInformer.SceneLoaded += OnLoaded;			
		}

		private void OnLoaded()
		{
			_upgradeWindowGetter = GameServices.Container.Get<IUpgradeWindowGetter>();
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