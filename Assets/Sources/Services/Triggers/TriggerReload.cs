using Sources.InfrastructureInterfaces.GameObject;
using UnityEngine;

namespace Sources.Services.Triggers
{
	public class TriggerReload : MonoBehaviour
	{
		private void OnTriggerStay(Collider other)
		{
			if (other.TryGetComponent(out IPlayer playerMoney))
				playerMoney.SellSand();
		}
	}
}