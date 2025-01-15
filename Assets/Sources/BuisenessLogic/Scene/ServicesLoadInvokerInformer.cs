using System;

namespace Sources.BuisenessLogic.Scene
{
	public class ServicesLoadInvokerInformer : ISceneLoadInvoker, ISceneLoadInformer
	{
		public bool IsSceneLoaded { get; private set; }

		public event Action SceneLoaded;

		public ServicesLoadInvokerInformer() =>
			IsSceneLoaded = false;

		public void Invoke()
		{
			IsSceneLoaded = true;
			SceneLoaded?.Invoke();
		}
	}
}