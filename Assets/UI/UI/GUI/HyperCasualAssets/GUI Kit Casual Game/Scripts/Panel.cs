using UnityEngine;

namespace UI.UI.GUI.HyperCasualAssets.GUI_Kit_Casual_Game.Scripts
{
	public class Panel : MonoBehaviour
	{
		[SerializeField] private GameObject[] otherPanels;

		public void OnEnable()
		{
			for (int i = 0; i < otherPanels.Length; i++)
				otherPanels[i].SetActive(
					true
				);
		}

		public void OnDisable()
		{
			for (int i = 0; i < otherPanels.Length; i++)
				otherPanels[i].SetActive(
					false
				);
		}
	}
}