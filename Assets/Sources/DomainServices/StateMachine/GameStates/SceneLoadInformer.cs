using System;
using InfrastructureInterfaces;

namespace Infrastructure.StateMachine.GameStates
{
	public class SceneLoadInformer : ISceneLoad
	{
		public event Action SceneLoaded;

		public void InvokeSceneLoaded() =>
			SceneLoaded?.Invoke();
	}
}