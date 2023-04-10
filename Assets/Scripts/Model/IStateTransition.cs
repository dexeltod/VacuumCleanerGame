using System;

namespace Model
{
	public interface IStateTransition
	{
		event Action<IState> StateChanged;
		void OnEnable();
		void OnDisable();
	}
}