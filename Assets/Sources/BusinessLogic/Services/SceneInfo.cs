using UnityEngine.SceneManagement;

namespace Sources.BusinessLogic.Services
{
	public class SceneInfo
	{
		public UnityEngine.SceneManagement.Scene Get() => SceneManager.GetActiveScene();
	}
}