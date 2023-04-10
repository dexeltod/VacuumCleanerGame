using System;

namespace Model
{
	public class SceneLoadInformer : ISceneLoad
	{
		public event Action SceneLoaded;

		public void InvokeSceneLoaded() =>
			SceneLoaded?.Invoke();
	}
}