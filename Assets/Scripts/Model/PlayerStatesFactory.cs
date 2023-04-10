using UnityEngine;

namespace Model
{
	public class PlayerStatesFactory
	{
		private readonly StateService _stateService;
		private readonly MusicSetter _musicSetter;
		
		private readonly IInputService _inputService;
		private readonly Animator _animator;
		private readonly AnimatorFacade _animatorFacade;
		private readonly AnimationHasher _animationHasher;

		private IStateTransition _anyToAttackTransition;
		private IStateTransition _anyToRunTransition;
		private IStateTransition _anyToDeadTransition;
		private IStateTransition _anyToIdleTransition;
		private IStateTransition _anyToJumpTransition;
		private IStateTransition _anyToFallTransition;
		private IStateTransition _anyToWallSlideTransition;

		private IStateTransition _attackToRunTransition;
		private IStateTransition _attackToIdleTransition;
		private IStateTransition _attackToFallTransition;

		public PlayerStatesFactory(IInputService inputService, Animator animator,
			AnimationHasher hasher, AnimatorFacade animatorFacade)
		{
			_inputService = inputService;
			_animator = animator;
			_animationHasher = hasher;
			_animatorFacade = animatorFacade;

			_stateService = new StateService();
		}

		public void CreateStateMachineAndSetState() =>
			new StateMachine(_stateService.Get<IdleState>());

		public void CreateTransitions()
		{
		}

		public void CreateStates()
		{
		}
	}
}