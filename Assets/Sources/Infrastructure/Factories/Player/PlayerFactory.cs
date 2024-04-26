using System;
using Graphic.Joystick_Pack.Scripts.Base;
using Sources.Controllers;
using Sources.Domain.Temp;
using Sources.DomainInterfaces.DomainServicesInterfaces;
using Sources.DomainInterfaces.Models;
using Sources.Infrastructure.Configs.Scripts;
using Sources.InfrastructureInterfaces.Factory;
using Sources.InfrastructureInterfaces.Providers;
using Sources.InfrastructureInterfaces.Repository;
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
		private readonly IPlayerModelRepositoryProvider _playerModelRepository;

		[Inject]
		public PlayerFactory(
			IAssetFactory assetFactory,
			IObjectResolver objectResolver,
			IGameplayInterfacePresenterProvider gameplayInterfaceProvider,
			IPlayerModelRepositoryProvider playerModelRepository
		)
		{
			_assetFactory = assetFactory ?? throw new ArgumentNullException(nameof(assetFactory));
			_objectResolver = objectResolver ?? throw new ArgumentNullException(nameof(objectResolver));

			_gameplayInterfaceProvider = gameplayInterfaceProvider ??
				throw new ArgumentNullException(nameof(gameplayInterfaceProvider));
			_playerModelRepository = playerModelRepository ??
				throw new ArgumentNullException(nameof(playerModelRepository));
		}

		private IPlayerModelRepository PlayerModelRepository => _playerModelRepository.Self;

		private Joystick ImplementationJoystick => _gameplayInterfaceProvider.Self.Joystick;

		public GameObject Create(GameObject spawnPoint)
		{
			if (spawnPoint == null) throw new ArgumentNullException(nameof(spawnPoint));

			var joystick = ImplementationJoystick
				? ImplementationJoystick
				: throw new ArgumentNullException(nameof(ImplementationJoystick));

			AnimationHasher animationHasher = new AnimationHasher();

			PlayerBody playerBodyComponentPresenter = GetPlayerBodyComponent(spawnPoint);

			_objectResolver.Inject(playerBodyComponentPresenter);

			GameObject character = playerBodyComponentPresenter.gameObject;
			Rigidbody body = character.GetComponent<Rigidbody>();

			Animator animator = character.GetComponentInChildren<Animator>();

			PlayerTransformable playerTransformable = new(
				character.transform,
				joystick,
				PlayerModelRepository.Get((int)ProgressType.Speed)
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