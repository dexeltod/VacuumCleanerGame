using System;
using System.Collections;
using InfrastructureInterfaces;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Infrastructure.StateMachine.GameStates
{
	public class SceneLoader
	{
		private readonly ICoroutineRunner _coroutineRunner;

		public SceneLoader(ICoroutineRunner coroutineRunner) =>
			_coroutineRunner = coroutineRunner;

		public void Load(string name, Action onLoaded = null) =>
			_coroutineRunner.StartCoroutine(LoadScene(name, onLoaded));

		private IEnumerator LoadScene(string nextScene, Action onLoaded = null)
		{
			if (SceneManager.GetActiveScene().name == nextScene)
			{
				onLoaded?.Invoke();
				yield break;
			}
		
			AsyncOperation waitNextTime = SceneManager.LoadSceneAsync(nextScene);

			while (waitNextTime.isDone == false)
				yield return null;

			onLoaded?.Invoke();
		}
	}
}