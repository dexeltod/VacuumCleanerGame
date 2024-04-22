using System;
using Graphic.Joystick_Pack.Scripts.Base;
using Sources.Controllers;
using Sources.Domain.Temp;
using Sources.Infrastructure.Configs.Scripts;
using Sources.InfrastructureInterfaces.Factory;
using Sources.InfrastructureInterfaces.Providers;
using Sources.ServicesInterfaces;
using Sources.Utils;
using UnityEngine;
using VContainer;

namespace Sources.Infrastructure.Factories.Player
{
	public class PlayerFactory : IPlayerFactory
	{
		private readonly AnimationHasher _hasher;
		private readonly IAssetFactory _assetFactory;
		private readonly IObjectResolver _objectResolver;
		private readonly IGameplayInterfacePresenterProvider _gameplayInterfaceProvider;
		private readonly IModifiableStatsRepositoryProvider _modifiableStatsRepositoryProvider;

		[Inject]
		public PlayerFactory(
			IAssetFactory assetFactory,
			IObjectResolver objectResolver,
			IGameplayInterfacePresenterProvider gameplayInterfaceProvider,
			IModifiableStatsRepositoryProvider modifiableStatsRepositoryProvider
		)
		{
			_assetFactory = assetFactory ?? throw new ArgumentNullException(nameof(assetFactory));
			_objectResolver = objectResolver ?? throw new ArgumentNullException(nameof(objectResolver));

			_gameplayInterfaceProvider = gameplayInterfaceProvider ??
				throw new ArgumentNullException(nameof(gameplayInterfaceProvider));
			_modifiableStatsRepositoryProvider = modifiableStatsRepositoryProvider ??
				throw new ArgumentNullException(nameof(modifiableStatsRepositoryProvider));
		}

		private Joystick ImplementationJoystick => _gameplayInterfaceProvider.Implementation.Joystick;

		public GameObject Player { get; private set; }

		public GameObject Create(GameObject spawnPoint)
		{
			if (spawnPoint == null) throw new ArgumentNullException(nameof(spawnPoint));

			var joystick =
				ImplementationJoystick
					? ImplementationJoystick
					: throw new ArgumentNullException(nameof(ImplementationJoystick));

			AnimationHasher animationHasher = new AnimationHasher();

			PlayerBody playerBodyComponentPresenter = GetPlayerBodyComponent(spawnPoint);

			_objectResolver.Inject(playerBodyComponentPresenter);

			GameObject character = playerBodyComponentPresenter.gameObject;
			Player = character;
			Rigidbody body = Player.GetComponent<Rigidbody>();

			Animator animator = Player.GetComponentInChildren<Animator>();

			PlayerTransformable playerTransformable = new(
				Player.transform,
				joystick,
				_modifiableStatsRepositoryProvider.Implementation.Get((int)ProgressType.Speed)
			);

			playerBodyComponentPresenter.Initialize(playerTransformable, body, animator, animationHasher);
			playerBodyComponentPresenter.gameObject.SetActive(true);

			return character;
		}

		private PlayerBody GetPlayerBodyComponent(GameObject spawnPoint) =>
			_assetFactory.InstantiateAndGetComponent<PlayerBody>(
				ResourcesAssetPath.Scene.Player,
				spawnPoint.transform.position
			);
	}
}