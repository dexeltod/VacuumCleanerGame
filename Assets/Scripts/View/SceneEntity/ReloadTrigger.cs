using Presenter.SceneEntity;
using UnityEngine;

public class ReloadTrigger : MonoBehaviour
{
	private void OnTriggerStay(Collider other)
	{
		if (other.TryGetComponent(out PlayerResources playerMoney)) 
			playerMoney.SellSand();
	}
}