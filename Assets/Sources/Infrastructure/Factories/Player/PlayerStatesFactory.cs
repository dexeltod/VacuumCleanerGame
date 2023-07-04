using Application.Character;
using Domain.Scene.PlayerStates;
using Infrastructure.Services;
using Infrastructure.StateMachine.GameStates;
using UnityEngine;
using View.SceneEntity;

namespace Infrastructure.Factories.Player
{
	public class PlayerStatesFactory
	{
		private readonly StateService _stateService;
		private readonly MusicSetter _musicSetter;
		
		private readonly Animator _animator;
		private readonly AnimatorFacade _animatorFacade;
		private readonly AnimationHasher _animationHasher;

		private IStateTransition _anyToRunTransition;
		private IStateTransition _anyToIdleTransition;

		public PlayerStatesFactory(  Animator animator,
			AnimationHasher hasher, AnimatorFacade animatorFacade)
		{
			_animator = animator;
			_animationHasher = hasher;
			_animatorFacade = animatorFacade;

			_stateService = new StateService();
		}

		public void CreateStateMachineAndSetState() =>
			new StateMachine.GameStates.StateMachine(_stateService.Get<IdleState>());

		public void CreateTransitions()
		{
		}

		public void CreateStates()
		{
			_stateService.Register(new IdleState(_animator, _animationHasher, _animatorFacade, new IStateTransition[]{}));
		}
	}
}