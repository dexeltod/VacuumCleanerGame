using System;
using Sources.Domain.Settings;
using Sources.Utils;
using UnityEngine;
using UnityEngine.Audio;

namespace Sources.Controllers.MainMenu
{
	public class SettingsPresenter
	{
		private const string MasterVolumeName = "Master";

		private readonly AudioMixer _audioMixer;
		private readonly SoundSettings _soundSettings;

		public SettingsPresenter(AudioMixer audioMixer, SoundSettings soundSettings)
		{
			_audioMixer = audioMixer ?? throw new ArgumentNullException(nameof(audioMixer));
			_soundSettings = soundSettings ?? throw new ArgumentNullException(nameof(soundSettings));
		}

		public void SetSoundVolume(float value)
		{
			_soundSettings.SetMasterVolume(value);
			_audioMixer.SetFloat(MasterVolumeName, _soundSettings.MasterVolume);
			PlayerPrefs.SetFloat(SettingsPlayerPrefsNames.MasterVolumeName, value);
		}
	}
}