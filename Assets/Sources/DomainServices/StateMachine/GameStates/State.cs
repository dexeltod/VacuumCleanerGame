using System;
using Application.Character;
using UnityEngine;

namespace Infrastructure.StateMachine.GameStates
{
	public abstract class State : IState
	{
		protected readonly Animator Animator;
		protected readonly AnimationHasher AnimationHasher;

		private readonly IStateTransition[] _transitions;
		private int _currentAnimationHash;
		public event Action<IState> StateChanged;

		protected State(Animator animator, AnimationHasher hasher,
			IStateTransition[] transitions)
		{
			Animator = animator;
			AnimationHasher = hasher;
			_transitions = transitions;
		}

		public void Enter()
		{
			OnEnter();

			foreach (IStateTransition transition in _transitions)
				transition.StateChanged += OnStateChanging;

			foreach (var transition in _transitions)
				transition.OnEnable();
		}

		public void Exit()
		{
			foreach (IStateTransition transition in _transitions)
				transition.StateChanged -= OnStateChanging;

			foreach (var transition in _transitions)
				transition.OnDisable();

			OnExit();
		}

		protected virtual void OnEnter()
		{
		}

		protected virtual void OnExit()
		{
		}

		private void OnStateChanging(IState state) =>
			StateChanged?.Invoke(state);

		public void Dispose()
		{
			GC.Collect();
		}
	}
}