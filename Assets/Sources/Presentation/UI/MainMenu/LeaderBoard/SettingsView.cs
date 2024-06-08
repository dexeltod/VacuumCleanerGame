using UnityEngine;
using UnityEngine.UI;

namespace Sources.Presentation.UI.MainMenu.LeaderBoard
{
	public class SettingsView : MonoBehaviour
	{
		[SerializeField] private Slider _soundVolumeSlider;

		public float _soundVolume;
		
		private void OnEnable()
		{
			_soundVolumeSlider.onValueChanged.AddListener(OnSoundChanged);
			
		}

		private void OnDisable()
		{
			_soundVolumeSlider.onValueChanged.RemoveListener(OnSoundChanged);
		}

		private void OnSoundChanged(float value)
		{
			
		}
	}
}