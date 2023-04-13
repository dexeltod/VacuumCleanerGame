using Model.Infrastructure;
using Model.Infrastructure.Services;
using Model.Infrastructure.StateMachine.GameStates;
using UnityEngine;
using UnityEngine.Serialization;

namespace Model
{
	public class Bootstrapper : MonoBehaviour, ICoroutineRunner
	{
		[SerializeField] private LoadingCurtain _loadingCurtain;
		[FormerlySerializedAs("_soundSetter")] [SerializeField] private MusicSetter _musicSetter;
		
		private Game _game;

		private void Awake()
		{
			MusicSetter musicSetter = GetMusicSetter();
			var loadingCurtain = GetLoadingCurtain();
			loadingCurtain.gameObject.SetActive(false);
			
			_game = new Game(this, loadingCurtain, musicSetter);
			_game.StateMachine.Enter<InitializeServicesState>();
			
			DontDestroyOnLoad(this);
		}

		private MusicSetter GetMusicSetter() =>
			Instantiate(_musicSetter);
		
		private LoadingCurtain GetLoadingCurtain() => 
			Instantiate(_loadingCurtain);
	}
}