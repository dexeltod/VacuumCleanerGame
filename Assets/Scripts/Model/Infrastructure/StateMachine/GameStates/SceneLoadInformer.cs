using System;
using Model.Infrastructure.Services;

namespace Model.Infrastructure.StateMachine.GameStates
{
	public class SceneLoadInformer : ISceneLoad
	{
		public event Action SceneLoaded;

		public void InvokeSceneLoaded() =>
			SceneLoaded?.Invoke();
	}
}