using UnityEngine;

namespace Sources.View.SceneEntity
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