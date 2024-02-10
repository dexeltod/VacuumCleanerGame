using System;
using Joystick_Pack.Scripts.Base;
using Sources.Controllers;
using Sources.InfrastructureInterfaces.Factory;
using Sources.Services.Facade;
using Sources.ServicesInterfaces;
using Sources.Utils;
using Sources.Utils.Configs.Scripts;
using UnityEngine;
using VContainer;

namespace Sources.Infrastructure.Factories.Player
{
	public class PlayerFactory : IPlayerFactory
	{
		private readonly AnimationHasher _hasher;
		private readonly IAssetFactory _assetFactory;
		private readonly IObjectResolver _objectResolver;

		private Joystick _joystick;
		private Animator _animator;
		private AnimationHasher _animationHasher;
		private AnimatorFacade _animatorFacade;
		private IPlayerStatsService _playerStats;

		public GameObject Player { get; private set; }

		[Inject]
		public PlayerFactory(IAssetFactory assetFactory, IObjectResolver objectResolver)
		{
			_assetFactory = assetFactory ?? throw new ArgumentNullException(nameof(assetFactory));
			_objectResolver = objectResolver ?? throw new ArgumentNullException(nameof(objectResolver));
		}

		public GameObject Create(
			GameObject spawnPoint,
			Joystick joystick,
			IPlayerStatsService stats
		)
		{
			if (spawnPoint == null) throw new ArgumentNullException(nameof(spawnPoint));
			_joystick = joystick ? joystick : throw new ArgumentNullException(nameof(joystick));
			_playerStats = stats ?? throw new ArgumentNullException(nameof(stats));

			return Create(spawnPoint);
		}

		private GameObject Create(GameObject spawnPoint)
		{
			AnimationHasher animationHasher = new AnimationHasher();

			PlayerBody playerBodyPresenter = _assetFactory.InstantiateAndGetComponent<PlayerBody>(
				ResourcesAssetPath.Scene.Player,
				spawnPoint.transform.position
			);

			_objectResolver.Inject(playerBodyPresenter);

			GameObject character = playerBodyPresenter.gameObject;
			Player = character;
			Rigidbody rigidbody = Player.GetComponent<Rigidbody>();
			Animator animator = Player.GetComponentInChildren<Animator>();

			PlayerTransformable playerTransformable = new(Player.transform, _joystick, _playerStats);
			playerBodyPresenter.Initialize(playerTransformable, rigidbody, animator, animationHasher);
			playerBodyPresenter.gameObject.SetActive(true);

			return character;
		}
	}
}