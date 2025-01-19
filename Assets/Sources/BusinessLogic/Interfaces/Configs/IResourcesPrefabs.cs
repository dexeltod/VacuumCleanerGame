using UnityEngine;

namespace Sources.BusinessLogic.Interfaces.Configs
{
	public interface IResourcesPrefabs
	{
		ISceneGameObjects SceneGameObjects { get; }
		ITriggers Triggers { get; }
	}

	public interface ITriggers
	{
		GameObject SellTrigger { get; }
	}
}
