using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Model.Infrastructure.Services.Factories
{
	public interface IUIGetter : IService
	{
		public GameObject This { get; }
		public Slider ScoreSlider { get; }
		public TextMeshProUGUI ScoreText { get; }
		public TextMeshProUGUI MoneyText { get; }
		public Joystick Joystick { get; }
	}
}