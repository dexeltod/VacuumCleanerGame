using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ViewModel.Infrastructure.Services.Factories
{
	public interface IUIGetter : IService
	{
		public GameObject GameObject { get; }
		public Canvas Canvas { get; }
		public Slider ScoreSlider { get; }
		public TextMeshProUGUI ScoreText { get; }
		public TextMeshProUGUI MoneyText { get; }
		public Joystick Joystick { get; }
	}
}