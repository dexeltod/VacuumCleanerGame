using Sources.Core.Application.StateMachine.GameStates;
using Sources.Infrastructure.InfrastructureInterfaces;
using UnityEngine;

namespace Sources.Core.Application
{
	public class Bootstrapper : MonoBehaviour, ICoroutineRunner
	{
		[SerializeField] private LoadingCurtain _loadingCurtain;
		
		private Game _game;

		private void Awake()
		{
			var loadingCurtain = GetLoadingCurtain();
			loadingCurtain.gameObject.SetActive(false);
			
			_game = new Game(this, loadingCurtain);
			_game.StateMachine.Enter<InitializeServicesAndProgressState>();
			
			DontDestroyOnLoad(this);
		}

		// private MusicSetter GetMusicSetter() =>
		// 	Instantiate(_musicSetter);
		
		private LoadingCurtain GetLoadingCurtain() => 
			Instantiate(_loadingCurtain);
	}
}