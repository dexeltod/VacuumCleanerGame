using Cysharp.Threading.Tasks;
using UnityEngine;

namespace ViewModel.Infrastructure.Services
{
	public class MusicService : IMusicService
	{
		private readonly MusicSetter _musicSetter;
		private readonly IAssetProvider _assetProvider;
		private string _currentSound;
		private AudioClip _sound;

		public MusicService(MusicSetter musicSetter, IAssetProvider assetProvider)
		{
			_musicSetter = musicSetter;
			_assetProvider = assetProvider;
		}

		public async UniTask Set(string audioName)
		{
			if (_currentSound == audioName || string.IsNullOrWhiteSpace(audioName) == true)
				return;
			
			_sound = await _assetProvider.LoadAsyncWithoutCash<AudioClip>(audioName);
			_musicSetter.SetAudioClip(_sound);
			_currentSound = audioName;
		}

		public void Stop() =>
			_musicSetter.Stop();
	}
}