using Cysharp.Threading.Tasks;
using Sources.BusinessLogic.ServicesInterfaces;
using UnityEngine.SceneManagement;

namespace Sources.Infrastructure.Services
{
	public class SceneLoader : ISceneLoader
	{
		public async UniTask LoadAsync(string nextScene)
		{
			await SceneManager.LoadSceneAsync(nextScene);
		}

		public void Load(string nextScene)
		{
			SceneManager.LoadScene(nextScene);
		}
	}
}
