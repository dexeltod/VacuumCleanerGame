using System;
using Sources.DIService;
using Sources.DomainInterfaces;
using Sources.InfrastructureInterfaces;
using Sources.InfrastructureInterfaces.Scene;
using Sources.ServicesInterfaces.UI;
using UnityEngine;

namespace Sources.Presentation
{
	public class UpgradeTriggerObserver : MonoBehaviour, IDisposable
	{
		private IUpgradeWindowGetter _upgradeWindowGetter;
		private IUpgradeWindow _upgradeWindow;
		private ISceneLoadInformer _sceneLoadInformer;
		private IProgressLoadDataService _progressLoadService;

		private bool _isCanSave;

		private void Start()
		{
			_sceneLoadInformer = ServiceLocator.Container.Get<ISceneLoadInformer>();
			_sceneLoadInformer.SceneLoaded += OnLoaded;
		}

		private void OnDisable() =>
			_sceneLoadInformer.SceneLoaded -= OnLoaded;

		private void OnLoaded()
		{
			_upgradeWindowGetter = ServiceLocator.Container.Get<IUpgradeWindowGetter>();
			_progressLoadService = ServiceLocator.Container.Get<IProgressLoadDataService>();

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

				await _progressLoadService.SaveToCloud(() => _isCanSave = true);
			}
		}

		public void Dispose() =>
			_sceneLoadInformer.SceneLoaded -= OnLoaded;
	}
}