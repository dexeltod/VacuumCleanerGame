using UnityEngine;

namespace Sources.Presentation.UI.Shop
{
	public class UpgradeWindowEnabler : MonoBehaviour
	{
		[SerializeField] private UpgradeWindowPresentation _upgradeCanvas;
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