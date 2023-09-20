using Sources.DIService;
using Sources.DomainInterfaces;
using Sources.DomainInterfaces.DomainServicesInterfaces;
using Sources.InfrastructureInterfaces;
using Sources.InfrastructureInterfaces.Scene;
using Sources.Services.PlayerServices;
using Sources.ServicesInterfaces.UI;
using UnityEngine;

namespace Sources.View
{
	public class UpgradeTriggerObserver : MonoBehaviour
	{
		private IUpgradeWindowGetter _upgradeWindowGetter;
		private IUpgradeWindow _upgradeWindow;
		private ISceneLoadInformer _sceneLoadInformer;
		private ISaveLoadDataService _saveLoadService;

		private bool _isCanSave;

		private void Start()
		{
			_sceneLoadInformer = GameServices.Container.Get<ISceneLoadInformer>();
			_sceneLoadInformer.SceneLoaded += OnLoaded;
		}

		private void OnLoaded()
		{
			_upgradeWindowGetter = GameServices.Container.Get<IUpgradeWindowGetter>();
			_saveLoadService = GameServices.Container.Get<ISaveLoadDataService>();
			
			_upgradeWindow = _upgradeWindowGetter.UpgradeWindow;
			_sceneLoadInformer.SceneLoaded -= OnLoaded;
		}

		private void OnTriggerEnter(Collider other)
		{
			if (other.TryGetComponent(out IPlayer _))
				_upgradeWindow.SetActiveYesNoButtons(true);
		}

		private async void OnTriggerExit(Collider other)
		{
			if (other.TryGetComponent(out IPlayer _))
			{
				_isCanSave = false;
				_upgradeWindow.SetActiveYesNoButtons(false);
			}

			if (other.TryGetComponent(out IPlayer _))
			{
				if (_isCanSave == false)
					return;

				await _saveLoadService.SaveToCloud(() => _isCanSave = true);
			}
		}
	}
}