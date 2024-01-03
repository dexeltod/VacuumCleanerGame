using Sources.Services.PlayerServices;
using UnityEngine;

namespace Sources.Services.Triggers
{
	public class TriggerReload : MonoBehaviour
	{
		private void OnTriggerStay(Collider other)
		{
			if (other.TryGetComponent(out PlayerResources playerMoney))
				playerMoney.SellSand();
		}
	}
}