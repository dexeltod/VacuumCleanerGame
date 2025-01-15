using UnityEngine.UI;

namespace Sources.PresentationInterfaces
{
	public interface ISettingsView : IView
	{
		Slider MasterVolumeSlider { get; }
	}
}
