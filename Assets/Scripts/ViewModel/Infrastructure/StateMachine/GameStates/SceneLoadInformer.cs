using System;
using ViewModel.Infrastructure.Services;

namespace ViewModel.Infrastructure.StateMachine.GameStates
{
	public class SceneLoadInformer : ISceneLoad
	{
		public event Action SceneLoaded;

		public void InvokeSceneLoaded() =>
			SceneLoaded?.Invoke();
	}
}