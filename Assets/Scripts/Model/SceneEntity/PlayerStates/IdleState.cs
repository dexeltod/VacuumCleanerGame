using Model.Character;
using UnityEngine;
using View.SceneEntity;
using ViewModel.Infrastructure;
using ViewModel.Infrastructure.StateMachine.GameStates;

namespace Model.SceneEntity.PlayerStates
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