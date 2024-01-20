using System;

using Sources.DomainInterfaces;
using Sources.InfrastructureInterfaces;
using Sources.InfrastructureInterfaces.Scene;
using Sources.ServicesInterfaces.UI;
using UnityEngine;
using VContainer;

namespace Sources.Presentation
{
	public class UpgradeTriggerObserver : MonoBehaviour
	{
		private IUpgradeWindowGetter _upgradeWindowGetter;
		private IUpgradeWindow _upgradeWindow;
		private ISceneLoadInformer _sceneLoadInformer;
		private IProgressLoadDataService _progressLoadService;

		private bool _isCanSave;

		[Inject]
		private void Construct(
			ISceneLoadInformer sceneLoadInformer,
			IUpgradeWindowGetter upgradeWindowGetter,
			IProgressLoadDataService progressLoadDataService
		)
		{
			_sceneLoadInformer = sceneLoadInformer;
			_upgradeWindowGetter = upgradeWindowGetter;
			_progressLoadService = progressLoadDataService;
		}

		private void Start()
		{
			_upgradeWindow = _upgradeWindowGetter.UpgradeWindow;
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
	}
}