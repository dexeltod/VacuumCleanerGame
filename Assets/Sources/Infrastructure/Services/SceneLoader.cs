using System;
using System.Collections;
using Cysharp.Threading.Tasks;
using Sources.BusinessLogic.ServicesInterfaces;
using Sources.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Sources.Infrastructure.Services
{
	public class SceneLoader : ISceneLoader
	{
		private readonly ICoroutineRunner _coroutineRunner;

		private bool _isSceneLoaded;

		public SceneLoader(ICoroutineRunner coroutineRunner) =>
			_coroutineRunner = coroutineRunner ?? throw new ArgumentNullException(nameof(coroutineRunner));

		public void Load(string nextScene)
		{
			SceneManager.LoadScene(nextScene);
		}

		public async UniTask LoadAsync(string nextScene)
		{
			await SceneManager.LoadSceneAsync(nextScene);
		}

		public void StartLoadCoroutine(string nextScene)
		{
			_coroutineRunner.Run(LoadYourAsyncScene(nextScene));
		}

		public void StartLoadCoroutine(string nextScene, Action onLoaded)
		{
			_isSceneLoaded = false;
			_coroutineRunner.Run(LoadYourAsyncScene(nextScene, onLoaded));

			onLoaded.Invoke();
		}

		private IEnumerator LoadYourAsyncScene(string nextScene)
		{
			AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(nextScene)
			                           ?? throw new ArgumentNullException($"Scene {nextScene} not found");

			while (!asyncLoad.isDone) yield return null;

			Debug.Log("scene loaded");
		}

		private IEnumerator LoadYourAsyncScene(string nextScene, Action onLoaded)
		{
			AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(nextScene)
			                           ?? throw new ArgumentNullException($"Scene {nextScene} not found");

			while (!asyncLoad.isDone) yield return null;

			onLoaded.Invoke();
			Debug.Log("scene loaded");
		}
	}
}
