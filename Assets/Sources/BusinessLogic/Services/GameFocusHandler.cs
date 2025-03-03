using System;
using Agava.WebUtility;
using Sources.DomainInterfaces;
using Sources.DomainInterfaces.Entities;
using Sources.Utils.ConstantNames;
using Sources.Utils.Scene;
using UnityEngine;
using UnityEngine.Audio;

namespace Sources.BusinessLogic.Services
{
	public class GameFocusHandler
	{
		private readonly ApplicationQuitHandler _applicationQuitHandler;
		private readonly AudioMixer _audioMixer;
		private readonly ISoundSettings _settings;

		private bool _isEnabled;

		public GameFocusHandler(
			IPersistentProgressService settings,
			AudioMixer audioMixer,
			ApplicationQuitHandler applicationQuitHandler)
		{
			if (settings == null) throw new ArgumentNullException(nameof(settings));

			_settings = settings.GlobalProgress.SoundSettings;

			_audioMixer = audioMixer ?? throw new ArgumentNullException(nameof(audioMixer));
			_applicationQuitHandler = applicationQuitHandler ?? throw new ArgumentNullException(nameof(applicationQuitHandler));
		}

		private string MasterVolume => ConstantNames.Sound.SoundMixerNames.MasterVolume;

		public void Disable()
		{
			_isEnabled = false;
			_applicationQuitHandler.ApplicationClosed -= Disable;
			Application.focusChanged -= OnFocusChanged;
			WebApplication.InBackgroundChangeEvent -= OnInBackgroundChanged;
		}

		public void Enable()
		{
			if (_isEnabled)
				return;

			_isEnabled = true;
			_applicationQuitHandler.ApplicationClosed += Disable;
			Application.focusChanged += OnFocusChanged;
			WebApplication.InBackgroundChangeEvent += OnInBackgroundChanged;
		}

		private void OnFocusChanged(bool isFocused)
		{
			SetTimePause(!isFocused);
			SetMute(!isFocused);
		}

		private void OnInBackgroundChanged(bool isBackground)
		{
			SetTimePause(isBackground);
			SetMute(isBackground);
		}

		private void SetMute(bool isMute)
		{
			if (isMute)
			{
				_settings.SetMasterVolume(-80f);
				_audioMixer.SetFloat(MasterVolume, _settings.Entity.Value);
			}
			else
			{
				_settings.Unmute();
				_audioMixer.SetFloat(MasterVolume, 0f);
			}
		}

		private void SetTimePause(bool isPaused) => Time.timeScale = isPaused ? 0f : 1f;
	}
}
