using System;

namespace Sources.InfrastructureInterfaces.Scene
{
	public class SceneLoadInformer : ISceneLoad
	{
		public event Action SceneLoaded;

		public void InvokeSceneLoaded() =>
			SceneLoaded?.Invoke();
	}
}