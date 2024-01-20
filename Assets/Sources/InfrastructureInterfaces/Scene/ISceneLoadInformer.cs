using System;


namespace Sources.InfrastructureInterfaces.Scene
{
	public interface ISceneLoadInformer 
	{
		event Action SceneLoaded;
		bool IsSceneLoaded { get; }
	}
}