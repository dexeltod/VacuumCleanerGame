using System;
using Agava.WebUtility;
using Sources.Utils.ConstantNames;
using Sources.Utils.Scene;
using UnityEngine;
using UnityEngine.Audio;

namespace Sources.Infrastructure.Services
{
	public class GameFocusHandler
	{
		private readonly AudioMixer _audioMixer;
		private readonly ApplicationQuitHandler _applicationQuitHandler;

		private bool _isEnabled = false;

		public GameFocusHandler(AudioMixer audioMixer, ApplicationQuitHandler applicationQuitHandler)
		{
			_audioMixer = audioMixer ?? throw new ArgumentNullException(nameof(audioMixer));
			_applicationQuitHandler = applicationQuitHandler ??
			                          throw new ArgumentNullException(nameof(applicationQuitHandler));
		}

		private string MasterVolume => ConstantNames.Sound.SoundMixerNames.MasterVolume;

		public void Enable()
		{
			if (_isEnabled)
				return;

			_isEnabled = true;
			_applicationQuitHandler.ApplicationClosed += Disable;
			Application.focusChanged += OnFocusChanged;
			WebApplication.InBackgroundChangeEvent += OnInBackgroundChanged;
		}

		private void Disable()
		{
			_isEnabled = false;
			_applicationQuitHandler.ApplicationClosed -= Disable;
			Application.focusChanged -= OnFocusChanged;
			WebApplication.InBackgroundChangeEvent -= OnInBackgroundChanged;
		}

		private void OnInBackgroundChanged(bool isBackground)
		{
			SetTimePause(isBackground);
			SetMute(isBackground);
		}

		private void OnFocusChanged(bool isFocused)
		{
			SetTimePause(!isFocused);
			SetMute(!isFocused);
		}

		private void SetTimePause(bool isPaused) =>
			Time.timeScale = isPaused ? 0f : 1f;

		private void SetMute(bool isMute)
		{
			if (isMute)
				_audioMixer.SetFloat(MasterVolume, -80f);
			else
				_audioMixer.SetFloat(MasterVolume, 0f);
		}
	}
}
