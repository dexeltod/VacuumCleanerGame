using Cysharp.Threading.Tasks;
using Sources.View.SceneEntity;
using UnityEngine;

namespace Sources.Application
{
	public class Bootstrapper : MonoBehaviour, ICoroutineRunner
	{
		[SerializeField] private LoadingCurtain _loadingCurtain;
		[SerializeField] private YandexAuthorizationHandler _authorizationHandler;

		private Game _game;

		private void Awake()
		{
			DontDestroyOnLoad(this);
			_loadingCurtain.gameObject.SetActive(true);

			_game = new Game(this, _loadingCurtain, _authorizationHandler);

			StartGame();
		}

		private void StartGame() =>
			_game.Start();

		public void StopCoroutineRunning(Coroutine coroutine) =>
			StopCoroutine(coroutine);
	}
}