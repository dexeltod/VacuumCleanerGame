using UnityEngine;
using UnityEngine.UI;

namespace UI.UI.GUI.UI_pack.Scripts
{
	public class FillValueNumber : MonoBehaviour
	{
		public Image TargetImage;

		// FixedUpdate is called once per frame
		private void Update()
		{
			float amount = TargetImage.fillAmount * 100;
			gameObject.GetComponent<Text>().text = amount.ToString(
				"F0"
			);
		}
	}
}