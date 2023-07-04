using UnityEngine;

namespace Infrastructure.Services
{
	[RequireComponent(typeof(AudioSource))]
	public class MusicSetter : MonoBehaviour
	{
		private AudioSource _audio;
		private AudioClip _clip;
		
		private void Awake()
		{
			_audio = GetComponent<AudioSource>();
			DontDestroyOnLoad(gameObject);
		}

		public void SetAudioClip(AudioClip clip)
		{
			_clip = clip;
			_audio.clip = _clip;
			_audio.Play();
		}

		public void Stop()
		{
			if (_audio.isPlaying)
				_audio.Stop();
		}
	}
}