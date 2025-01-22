using System;
using Sources.PresentationInterfaces;
using Sources.PresentationInterfaces.Triggers;
using UnityEngine;

namespace Sources.Presentation.Triggers
{
	public class TriggerSell : MonoBehaviour, ITriggerSell
	{
		private void OnTriggerEnter(Collider other)
		{
			if (!other.TryGetComponent(out IPlayer _)) return;

			OnTriggerStayed!.Invoke(true);
		}

		private void OnTriggerExit(Collider other)
		{
			if (!other.TryGetComponent(out IPlayer _))
				return;

			OnTriggerStayed!.Invoke(false);
		}

		public event Action<bool> OnTriggerStayed;
	}
}