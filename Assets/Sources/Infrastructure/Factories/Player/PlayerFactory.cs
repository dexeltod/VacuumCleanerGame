using System;
using System.Collections.Generic;
using Application.Character;
using Application.Configs;
using Application.DI;
using Cysharp.Threading.Tasks;
using Domain.Scene;
using Infrastructure.Services;
using InfrastructureInterfaces;
using UnityEngine;
using View.SceneEntity;

namespace Infrastructure.Factories.Player
{
	public class PlayerFactory : IPlayerFactory
	{
		private readonly AnimationHasher _hasher;
		private readonly IAssetProvider _assetProvider;
		private readonly IPersistentProgressService _progressService;

		private IPresenterFactory _presenterFactory;
		
		private Joystick _joystick;
		private Animator _animator;
		private AnimationHasher _animationHasher;
		private AnimatorFacade _animatorFacade;
		private PlayerStatesFactory _playerStatesFactory;
		private IPlayerStatsService _player;

		public GameObject Player { get; private set; }

		public PlayerFactory(IAssetProvider assetProvider)
		{
			_progressService = ServiceLocator.Container.GetSingle<IPersistentProgressService>();
			_assetProvider = assetProvider;
		}

		public async UniTask Instantiate(GameObject initialPoint, IPresenterFactory presenterFactory,
			Joystick joystick, IPlayerStatsService stats)
		{
			_player = stats;
			_joystick = joystick;
			_presenterFactory = presenterFactory;
			_assetProvider.CleanUp();
			await Create(initialPoint);
			CreateDependenciesAsync();
		}

		private async UniTask Create(GameObject initialPoint)
		{
			List<Tuple<string, int>> shopProgress = _progressService.GameProgress.ShopProgress.GetAll();
			_player.Initialize(shopProgress);
			
			var playerPresenter =
				await _presenterFactory.Instantiate<View.SceneEntity.Player>(
					ConstantNames.Player,
					initialPoint.transform.position);

			var character = playerPresenter.gameObject;
			Player = character;
			Rigidbody rigidbody = Player.GetComponent<Rigidbody>();

			PlayerTransformable playerTransformable = new(Player.transform, _joystick, _player);
			playerPresenter.Init(playerTransformable, rigidbody);
		}

		private void CreateDependenciesAsync()
		{
			NullifyComponents();
			GetComponents();
			CreatePlayerStateMachine();
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

			_playerStatesFactory = new PlayerStatesFactory(_animator,
				_animationHasher,
				_animatorFacade);

			_playerStatesFactory.CreateTransitions();
			_playerStatesFactory.CreateStates();
			_playerStatesFactory.CreateStateMachineAndSetState();
		}
	}
}