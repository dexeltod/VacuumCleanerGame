using System;
using Plugins.Joystick_Pack.Scripts.Base;
using Sources.Controllers;
using Sources.InfrastructureInterfaces.Factory;
using Sources.InfrastructureInterfaces.Providers;
using Sources.InfrastructureInterfaces.Repository;
using Sources.ServicesInterfaces;
using Sources.Utils;
using Sources.Utils.Enums;
using UnityEngine;
using VContainer;

namespace Sources.Infrastructure.Factories.Player
{
	public class PlayerFactory : IPlayerFactory
	{
		private readonly AnimationHasher _hasher;
		private readonly IAssetFactory _assetFactory;
		private readonly IObjectResolver _objectResolver;
		private readonly IGameplayInterfacePresenterProvider _interfaceProvider;
		private readonly IPlayerModelRepositoryProvider _modelRepository;
		private GameObject _character;
		private Rigidbody _body;
		private Animator _animator;

		[Inject]
		public PlayerFactory(
			IAssetFactory assetFactory,
			IObjectResolver objectResolver,
			IGameplayInterfacePresenterProvider interfaceProvider,
			IPlayerModelRepositoryProvider modelRepository
		)
		{
			_assetFactory = assetFactory ??
			                throw new ArgumentNullException(
				                nameof(assetFactory)
			                );
			_objectResolver = objectResolver ??
			                  throw new ArgumentNullException(
				                  nameof(objectResolver)
			                  );

			_interfaceProvider = interfaceProvider ??
			                     throw new ArgumentNullException(
				                     nameof(interfaceProvider)
			                     );
			_modelRepository = modelRepository ??
			                   throw new ArgumentNullException(
				                   nameof(modelRepository)
			                   );
		}

		private IPlayerModelRepository PlayerModelRepository => _modelRepository.Self;

		private Joystick ImplementationJoystick => _interfaceProvider.Self.Joystick;

		public GameObject Create(GameObject spawnPoint)
		{
			if (spawnPoint == null)
				throw new ArgumentNullException(
					nameof(spawnPoint)
				);

			var joystick = ImplementationJoystick
				? ImplementationJoystick
				: throw new ArgumentNullException(
					nameof(ImplementationJoystick)
				);

			AnimationHasher animationHasher = new AnimationHasher();

			PlayerBody playerBodyComponentPresenter = GetPlayerBodyComponent(
				spawnPoint
			);

			_objectResolver.Inject(
				playerBodyComponentPresenter
			);

			_character = playerBodyComponentPresenter.gameObject;
			_body = _character.GetComponent<Rigidbody>();
			_animator = _character.GetComponentInChildren<Animator>();

			PlayerTransformable playerTransformable = new(
				_character.transform,
				joystick,
				PlayerModelRepository.Get(
					(int)ProgressType.Speed
				),
				_body
			);

			SetPresenter(
				playerBodyComponentPresenter,
				playerTransformable,
				animationHasher
			);

			return _character;
		}

		private void SetPresenter(PlayerBody playerBodyComponentPresenter, PlayerTransformable playerTransformable,
			AnimationHasher animationHasher)
		{
			playerBodyComponentPresenter.Initialize(
				playerTransformable,
				_animator,
				animationHasher
			);
			playerBodyComponentPresenter.gameObject.SetActive(
				true
			);
		}

		private PlayerBody GetPlayerBodyComponent(GameObject spawnPoint) =>
			_assetFactory.InstantiateAndGetComponent<PlayerBody>(
				ResourcesAssetPath.Scene.Player,
				spawnPoint.transform.position
			);
	}
}
