using Cysharp.Threading.Tasks;
using Sources.BuisenessLogic.ServicesInterfaces;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Sources.Infrastructure.Services
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
