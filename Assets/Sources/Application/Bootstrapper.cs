using Cysharp.Threading.Tasks;
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
			DontDestroyOnLoad(this);
			LoadingCurtain loadingCurtain = GetLoadingCurtain();
			loadingCurtain.gameObject.SetActive(true);

			_game = new Game(this, loadingCurtain);
			await StartGame();

		}

		private async UniTask StartGame() => 
			await _game.Start();

		private LoadingCurtain GetLoadingCurtain() =>
			Instantiate(_loadingCurtain);
	}
}