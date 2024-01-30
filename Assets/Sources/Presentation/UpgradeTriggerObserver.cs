using System;
using Sources.InfrastructureInterfaces;
using UnityEngine;

namespace Sources.Presentation
{
	public class UpgradeTriggerObserver : MonoBehaviour
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