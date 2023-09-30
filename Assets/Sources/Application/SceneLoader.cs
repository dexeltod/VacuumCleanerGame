using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Sources.Application
{
	public class SceneLoader
	{
		public async UniTask Load(string nextScene)
		{
			if (SceneManager.GetActiveScene().name == nextScene)
				return;

			AsyncOperation waitNextTime = SceneManager.LoadSceneAsync(nextScene);

			await UniTask.WaitWhile(() => waitNextTime.isDone == false);
		}
	}
}