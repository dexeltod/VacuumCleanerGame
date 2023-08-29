using Sources.View.SceneEntity;
using UnityEngine;

namespace Sources.Application
{
	public class Bootstrapper : MonoBehaviour, ICoroutineRunner
	{
		[SerializeField] private LoadingCurtain _loadingCurtain;
		
		private Game _game;

		private async void Awake()
		{
			var loadingCurtain = GetLoadingCurtain();
			loadingCurtain.gameObject.SetActive(false);

			_game = new Game(this, loadingCurtain);
			await _game.Start();
			
			DontDestroyOnLoad(this);
		}
		
		private LoadingCurtain GetLoadingCurtain() => 
			Instantiate(_loadingCurtain);
	}
}