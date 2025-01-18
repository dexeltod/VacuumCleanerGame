using System;
using Sources.BusinessLogic.ServicesInterfaces;
using Sources.PresentationInterfaces;
using UnityEngine;

namespace Sources.Infrastructure.Services.SceneTriggers
{
	public class UpgradeTriggerObserver : MonoBehaviour, IUpgradeTriggerObserver
	{
		private bool _isCanSave;
		public event Action<bool> TriggerEntered;

		private void OnTriggerEnter(Collider other)
		{
			if (other.TryGetComponent(out IPlayer _))
				TriggerEntered!.Invoke(true);
		}

		private void OnTriggerExit(Collider other)
		{
			if (other.TryGetComponent(out IPlayer _))
				TriggerEntered!.Invoke(false);
		}
	}
}
