using System;
using Cysharp.Threading.Tasks;
using Model.Character;
using Model.DI;
using Model.Infrastructure.Data;
using Presenter.SceneEntity;
using UnityEngine;

namespace Model.Infrastructure.Services.Factories
{
	public class PlayerFactory : IPlayerFactory
	{
		private readonly AnimationHasher _hasher;
		private readonly IAssetProvider _assetProvider;
		private readonly IPersistentProgressService _progressService;
		private readonly IInputService _inputService;
		private readonly GameProgress _progress;

		private Joystick _joystick;
		private Animator _animator;
		private AnimationHasher _animationHasher;
		private AnimatorFacade _animatorFacade;
		private PlayerStatesFactory _playerStatesFactory;
		private IPresenterFactory _presenterFactory;

		public GameObject MainCharacter { get; private set; }

		public event Action MainCharacterCreated;

		public PlayerFactory(IAssetProvider assetProvider, IPersistentProgressService progressService)
		{
			_inputService = ServiceLocator.Container.GetSingle<IInputService>();
			_assetProvider = assetProvider;
			_progress = progressService.GameProgress;
		}

		public async UniTask Instantiate(GameObject initialPoint, IPresenterFactory presenterFactory,
			Joystick joystick)
		{
			_joystick = joystick;
			_presenterFactory = presenterFactory;
			_assetProvider.CleanUp();
			await CreateHeroGameObject(initialPoint);
			CreateDependenciesAsync();
		}

		private async UniTask CreateHeroGameObject(GameObject initialPoint)
		{
			var playerPresenter = await _presenterFactory.Instantiate<PlayerPresenter>(
				ConstantNamesConfig.Player,
				initialPoint.transform.position);

			var gameObject = playerPresenter.gameObject;
			MainCharacter = gameObject;

			//TODO Need vacuum car config;
			VacuumCar vacuumCar = new(gameObject.transform.position, gameObject.transform.rotation.z, _joystick, 0.05f);
			playerPresenter.Init(vacuumCar);
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