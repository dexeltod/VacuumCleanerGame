using Sources.ControllersInterfaces.Common;
using UnityEngine.UI;

namespace Sources.PresentationInterfaces
{
	public interface ISettingsView : IPresenter
	{
		Slider MasterVolumeSlider { get; }
	}
}