using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Sources.Application
{
	public class SceneLoader : ISceneLoader
	{
		public async UniTask Load(string nextScene)
		{
			AsyncOperation waitNextTime = SceneManager.LoadSceneAsync(nextScene);
			await UniTask.WaitWhile(() => waitNextTime.isDone == false);
		}
	}
}