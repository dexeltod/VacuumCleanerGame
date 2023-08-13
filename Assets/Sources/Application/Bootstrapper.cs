using Sources.Application.StateMachine.GameStates;
using Sources.View.SceneEntity;
using UnityEngine;

namespace Sources.Application
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
		
		private LoadingCurtain GetLoadingCurtain() => 
			Instantiate(_loadingCurtain);
	}
}