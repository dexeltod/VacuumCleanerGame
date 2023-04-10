using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Model
{
	public class PlayerFactory : IPlayerFactory
	{
		private const string Player = "Player";

		private readonly AnimationHasher _hasher;
		private readonly IAssetProvider _assetProvider;
		private readonly IPersistentProgressService _progressService;
		private readonly IInputService _inputService;
		private readonly GameProgress _progress;

		private Animator _animator;
		private AnimationHasher _animationHasher;
		private AnimatorFacade _animatorFacade;
		private PlayerStatesFactory _playerStatesFactory;

		public GameObject MainCharacter { get; private set; }

		public event Action MainCharacterCreated;

		public PlayerFactory(IAssetProvider assetProvider, IPersistentProgressService progressService)
		{
			_inputService = ServiceLocator.Container.GetSingle<IInputService>();
			_assetProvider = assetProvider;
			_progress = progressService.GameProgress;
		}

		public async UniTask InstantiateHero(GameObject initialPoint)
		{
			_assetProvider.CleanUp();
			await CreateHeroGameObject(initialPoint);
			CreateDependenciesAsync();
		}

		private async UniTask CreateHeroGameObject(GameObject initialPoint)
		{
			MainCharacter = null;
			MainCharacter = await _assetProvider.InstantiateNoCash(Player, initialPoint.transform.position);
		}

		private void CreateDependenciesAsync()
		{
			NullifyComponents();

			GetComponents();

			CreatePlayerStateMachine();
			MainCharacterCreated?.Invoke();
		}

		private void GetComponents()
		{
			_animator = MainCharacter.GetComponent<Animator>();
			_animationHasher = MainCharacter.GetComponent<AnimationHasher>();
			_animatorFacade = MainCharacter.GetComponent<AnimatorFacade>();
		}

		private void NullifyComponents()
		{
			_animator = null;
			_animationHasher = null;
			_animatorFacade = null;

			GC.Collect();
		}

		private void CreatePlayerStateMachine()
		{
			if (_playerStatesFactory != null)
			{
				_playerStatesFactory = null;
				GC.Collect();
			}

			_playerStatesFactory = new PlayerStatesFactory(_inputService, _animator,
				_animationHasher,
				_animatorFacade);

			_playerStatesFactory.CreateTransitions();
			_playerStatesFactory.CreateStates();
			_playerStatesFactory.CreateStateMachineAndSetState();
		}
	}
}