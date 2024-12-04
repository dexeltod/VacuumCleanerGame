using Sources.PresentationInterfaces;
using UnityEngine;
using UnityEngine.UI;

namespace Sources.Presentation.UI.MainMenu.LeaderBoard
{
	public class SettingsView : View, ISettingsView
	{
		[SerializeField] private Slider _soundVolumeSlider;

		public Slider MasterVolumeSlider => _soundVolumeSlider;
	}
}