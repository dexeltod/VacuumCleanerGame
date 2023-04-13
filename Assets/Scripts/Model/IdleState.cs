using Model.Character;
using Model.Infrastructure;
using Model.Infrastructure.StateMachine.GameStates;
using UnityEngine;

namespace Model
{
	public sealed class IdleState : State
	{
		private readonly AnimatorFacade _animatorFacade;

		public IdleState(IInputService inputService, Animator animator,
			AnimationHasher hasher,
			AnimatorFacade animatorFacade,
			IStateTransition[] transitions) : base(inputService, animator, hasher, transitions)
		{
			_animatorFacade = animatorFacade;
		}

		protected override void OnEnter()
		{

		}
	}
}