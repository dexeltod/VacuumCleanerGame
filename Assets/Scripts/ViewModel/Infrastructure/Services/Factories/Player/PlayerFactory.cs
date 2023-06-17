using System;
using Cysharp.Threading.Tasks;
using Model;
using Model.Character;
using Model.Configs;
using Model.Data;
using Model.Data.Player;
using Model.DI;
using UnityEngine;
using View.SceneEntity;

namespace ViewModel.Infrastructure.Services.Factories.Player
{
	public class PlayerFactory : IPlayerFactory
	{
		private readonly AnimationHasher _hasher;
		private readonly IAssetProvider _assetProvider;
		private readonly IPersistentProgressService _progressService;
		private readonly IInputService _inputService;

		private IPresenterFactory _presenterFactory;
		
		private Joystick _joystick;
		private Animator _animator;
		private AnimationHasher _animationHasher;
		private AnimatorFacade _animatorFacade;
		private PlayerStatesFactory _playerStatesFactory;
		private PlayerProgress _player;
		
		public GameObject Player { get; private set; }

		public event Action PlayerCreated;

		public PlayerFactory(IAssetProvider assetProvider)
		{
			_inputService = ServiceLocator.Container.GetSingle<IInputService>();
			_assetProvider = assetProvider;
		}

		public async UniTask Instantiate(GameObject initialPoint, IPresenterFactory presenterFactory,
			Joystick joystick, GameProgressModel progress)
		{
			_player = progress.PlayerProgress;
			_joystick = joystick;
			_presenterFactory = presenterFactory;
			_assetProvider.CleanUp();
			await Create(initialPoint);
			CreateDependenciesAsync();
		}

		private async UniTask Create(GameObject initialPoint)
		{
			var playerPresenter =
				await _presenterFactory.Instantiate<Vacuum>(
					ConstantNames.Player,
					initialPoint.transform.position);

			var character = playerPresenter.gameObject;
			Player = character;
			Rigidbody rigidbody = Player.GetComponent<Rigidbody>();

			VacuumModel vacuumModel = new(Player.transform, _joystick, _player);
			playerPresenter.Init(vacuumModel, rigidbody);
		}

		private void CreateDependenciesAsync()
		{
			NullifyComponents();
			GetComponents();
			CreatePlayerStateMachine();
			PlayerCreated?.Invoke();
		}

		private void GetComponents()
		{
			_animator = Player.GetComponent<Animator>();
			_animationHasher = Player.GetComponent<AnimationHasher>();
			_animatorFacade = Player.GetComponent<AnimatorFacade>();
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