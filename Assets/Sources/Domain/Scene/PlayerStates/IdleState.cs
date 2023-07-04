using Application.Character;
using Infrastructure.StateMachine.GameStates;
using UnityEngine;
using View.SceneEntity;

namespace Domain.Scene.PlayerStates
{
	public sealed class IdleState : State
	{
		private readonly AnimatorFacade _animatorFacade;

		public IdleState(Animator animator,
			AnimationHasher hasher,
			AnimatorFacade animatorFacade,
			IStateTransition[] transitions) : base( animator, hasher, transitions)
		{
			_animatorFacade = animatorFacade;
		}

		protected override void OnEnter()
		{

		}
	}
}