using UnityEngine;

namespace Sources.View.UI.Shop
{
	public class UpgradeWindowEnabler : MonoBehaviour
	{
		[SerializeField] private UpgradeWindow _upgradeCanvas;
		[SerializeField] private GameObject _upgradeDiv;

		public void Enable()
		{
			_upgradeDiv.SetActive(true);
			_upgradeCanvas.enabled = true;
		}

		public void Disable()
		{
			_upgradeDiv.SetActive(false);
			_upgradeCanvas.enabled = false;
		}
	}
}