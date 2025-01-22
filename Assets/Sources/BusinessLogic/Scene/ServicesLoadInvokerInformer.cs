using System;

namespace Sources.BusinessLogic.Scene
{
	public class ServicesLoadInvokerInformer : ISceneLoadInvoker, ISceneLoadInformer
	{
		public ServicesLoadInvokerInformer() =>
			IsSceneLoaded = false;

		public bool IsSceneLoaded { get; private set; }

		public event Action SceneLoaded;

		public void Invoke()
		{
			IsSceneLoaded = true;
			SceneLoaded?.Invoke();
		}
	}
}