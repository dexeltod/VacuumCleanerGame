using UnityEngine;
using UnityEngine.UI;

namespace Sources.Presentation
{
	public class ChooseWindow : MonoBehaviour
	{
		[SerializeField] private Button _yesButton;
		[SerializeField] private Button _noButton;

		public Button YesButton => _yesButton;

		public Button NoButton => _noButton;
	}
}
