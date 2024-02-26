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
		private readonly IGameplayInterfacePresenterProvider _gameplayInterfaceProvider;

		[Inject]
		public PlayerFactory(
			IAssetFactory assetFactory,
			IObjectResolver objectResolver,
			IPlayerStatsServiceProvider playerStatsServiceProvider,
			IGameplayInterfacePresenterProvider gameplayInterfaceProvider
		)
		{
			_assetFactory = assetFactory ?? throw new ArgumentNullException(nameof(assetFactory));
			_objectResolver = objectResolver ?? throw new ArgumentNullException(nameof(objectResolver));
			_playerStatsServiceProvider = playerStatsServiceProvider ??
				throw new ArgumentNullException(nameof(playerStatsServiceProvider));
			_gameplayInterfaceProvider = gameplayInterfaceProvider ??
				throw new ArgumentNullException(nameof(gameplayInterfaceProvider));
		}

		private Joystick ImplementationJoystick => _gameplayInterfaceProvider.Implementation.Joystick;

		private IPlayerStatsService PlayerStatsService => _playerStatsServiceProvider.Implementation;

		public GameObject Player { get; private set; }

		public GameObject Create(GameObject spawnPoint)
		{
			if (spawnPoint == null) throw new ArgumentNullException(nameof(spawnPoint));

			var joystick =
				ImplementationJoystick
					? ImplementationJoystick
					: throw new ArgumentNullException(nameof(ImplementationJoystick));

			AnimationHasher animationHasher = new AnimationHasher();

			PlayerBody playerBodyComponent = GetPlayerBodyComponent(spawnPoint);

			_objectResolver.Inject(playerBodyComponent);

			GameObject character = playerBodyComponent.gameObject;
			Player = character;
			Rigidbody body = Player.GetComponent<Rigidbody>();
			
			Animator animator = Player.GetComponentInChildren<Animator>();

			PlayerTransformable playerTransformable = new(
				Player.transform,
				joystick,
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