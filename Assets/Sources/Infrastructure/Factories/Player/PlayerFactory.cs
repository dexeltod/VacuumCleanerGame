using System;
using Joystick_Pack.Scripts.Base;
using Sources.Controllers;
using Sources.InfrastructureInterfaces.Factory;
using Sources.InfrastructureInterfaces.Providers;
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
		private readonly IPlayerStatsServiceProvider _playerStatsServiceProvider;

		private Joystick _joystick;
		private Animator _animator;
		private AnimationHasher _animationHasher;
		private AnimatorFacade _animatorFacade;

		private IPlayerStatsService PlayerStatsService => _playerStatsServiceProvider.Implementation;

		public GameObject Player { get; private set; }

		[Inject]
		public PlayerFactory(
			IAssetFactory assetFactory,
			IObjectResolver objectResolver,
			IPlayerStatsServiceProvider playerStatsServiceProvider
		)
		{
			_assetFactory = assetFactory ?? throw new ArgumentNullException(nameof(assetFactory));
			_objectResolver = objectResolver ?? throw new ArgumentNullException(nameof(objectResolver));
			_playerStatsServiceProvider = playerStatsServiceProvider ??
				throw new ArgumentNullException(nameof(playerStatsServiceProvider));
		}

		public GameObject Create(
			GameObject spawnPoint,
			Joystick joystick
		)
		{
			if (spawnPoint == null) throw new ArgumentNullException(nameof(spawnPoint));
			_joystick = joystick ? joystick : throw new ArgumentNullException(nameof(joystick));

			return Create(spawnPoint);
		}

		private GameObject Create(GameObject spawnPoint)
		{
			AnimationHasher animationHasher = new AnimationHasher();

			PlayerBody playerBodyComponent = GetPlayerBodyComponent(spawnPoint);

			_objectResolver.Inject(playerBodyComponent);

			GameObject character = playerBodyComponent.gameObject;
			Player = character;
			Rigidbody body = Player.GetComponent<Rigidbody>();
			Animator animator = Player.GetComponentInChildren<Animator>();

			PlayerTransformable playerTransformable = new(
				Player.transform,
				_joystick,
				PlayerStatsService
			);

			playerBodyComponent.Initialize(playerTransformable, body, animator, animationHasher);
			playerBodyComponent.gameObject.SetActive(true);

			return character;
		}

		private PlayerBody GetPlayerBodyComponent(GameObject spawnPoint) =>
			_assetFactory.InstantiateAndGetComponent<PlayerBody>(
				ResourcesAssetPath.Scene.Player,
				spawnPoint.transform.position
			);
	}
}