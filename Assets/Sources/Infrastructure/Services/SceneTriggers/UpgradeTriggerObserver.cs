using System;
using Sources.PresentationInterfaces;
using UnityEngine;

namespace Sources.Infrastructure.Services.SceneTriggers
{
	public class UpgradeTriggerObserver : MonoBehaviour, IUpgradeTriggerObserver
	{
		private bool _isCanSave;

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

		public event Action<bool> TriggerEntered;
	}
}