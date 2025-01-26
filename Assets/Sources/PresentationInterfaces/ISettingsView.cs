using Sources.PresentationInterfaces.Common;
using UnityEngine.UI;

namespace Sources.PresentationInterfaces
{
	public interface ISettingsView : IView
	{
		Slider MasterVolumeSlider { get; }
	}
}