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

		private IUpgradeWindow UpgradeWindow => _upgradeWindowGetter.UpgradeWindow;

		[Inject]
		private void Construct(
			IUpgradeWindowGetter upgradeWindowGetter,
			IProgressLoadDataService progressLoadDataService
		)
		{
			_upgradeWindowGetter = upgradeWindowGetter ?? throw new ArgumentNullException(nameof(upgradeWindowGetter));
			_progressLoadService = progressLoadDataService ??
				throw new ArgumentNullException(nameof(progressLoadDataService));
		}

		private void OnTriggerEnter(Collider other)
		{
			if (other.TryGetComponent(out IPlayer _))
				UpgradeWindow.SetActiveYesNoButtons(true);
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