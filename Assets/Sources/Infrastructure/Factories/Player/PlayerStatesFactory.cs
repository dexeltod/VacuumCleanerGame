using Sources.Services;
using UnityEngine;

namespace Sources.Infrastructure.Factories.Player
{
	public class PlayerStatesFactory
	{
		private readonly StateService _stateService;

		private readonly Animator _animator;
		private readonly AnimatorFacade _animatorFacade;
		private readonly AnimationHasher _animationHasher;

		private IStateTransition _anyToRunTransition;
		private IStateTransition _anyToIdleTransition;

		public PlayerStatesFactory(
			Animator animator,
			AnimationHasher hasher,
			AnimatorFacade animatorFacade
		)
		{
			_animator = animator;
			_animationHasher = hasher;
			_animatorFacade = animatorFacade;

			_stateService = new StateService();
		}

		public void CreateStateMachineAndSetState() =>
			new StateMachine(new StartState());

		public void CreateTransitions() { }

		public void CreateStates() { }
	}
}