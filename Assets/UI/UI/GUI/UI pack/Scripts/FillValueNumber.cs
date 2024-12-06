using UnityEngine;
using UnityEngine.UI;

namespace Graphic.GUI.UI_pack.Scripts
{
	public class FillValueNumber : MonoBehaviour
	{
		public Image TargetImage;

		// Update is called once per frame
		void Update()
		{
			float amount = TargetImage.fillAmount * 100;
			gameObject.GetComponent<Text>().text = amount.ToString("F0");
		}
	}
}