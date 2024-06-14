using System;
using UnityEngine;

namespace Sources.Controllers
{
	public class GameplayInterfaceSoundPlayer
	{
		private readonly AudioClip _soundBuy;
		private readonly AudioClip _soundClose;
		private readonly AudioSource _audioSource;

		public GameplayInterfaceSoundPlayer(AudioClip soundBuy, AudioClip soundClose, AudioSource audioSource)
		{
			_soundBuy = soundBuy ?? throw new ArgumentNullException(nameof(soundBuy));
			_soundClose = soundClose ?? throw new ArgumentNullException(nameof(soundClose));
			_audioSource = audioSource ?? throw new ArgumentNullException(nameof(audioSource));
		}

		public void PlayBuySound() =>
			_audioSource.PlayOneShot(_soundBuy);

		public void PlayCloseSound() =>
			_audioSource.PlayOneShot(_soundClose);
	}
}