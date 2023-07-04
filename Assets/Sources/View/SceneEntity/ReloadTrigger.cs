using UnityEngine;

namespace View.SceneEntity
{
	public class ReloadTrigger : MonoBehaviour
	{
		private void OnTriggerStay(Collider other)
		{
			if (other.TryGetComponent(out PlayerResources playerMoney)) 
				playerMoney.SellSand();
		}
	}
}