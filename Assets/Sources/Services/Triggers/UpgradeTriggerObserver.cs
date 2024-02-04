using System;
using Sources.InfrastructureInterfaces.GameObject;
using Sources.ServicesInterfaces;
using UnityEngine;

namespace Sources.Services.Triggers
{
	public class UpgradeTriggerObserver : MonoBehaviour, IUpgradeTriggerObserver
	{
		private bool _isCanSave;
		public event Action<bool> TriggerEntered;

		private void OnTriggerEnter(Collider other)
		{
			if (other.TryGetComponent(out IPlayer _))
				TriggerEntered.Invoke(true);
		}

		private void OnTriggerExit(Collider other)
		{
			if (other.TryGetComponent(out IPlayer _))
				TriggerEntered.Invoke(false);
		}
	}
}