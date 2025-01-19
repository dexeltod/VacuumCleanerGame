using System;
using Plugins.Joystick_Pack.Scripts.Base;
using Sources.BusinessLogic.Interfaces.Factory;
using Sources.BusinessLogic.Repository;
using Sources.BusinessLogic.ServicesInterfaces;
using Sources.ControllersInterfaces;
using Sources.Presentation;
using Sources.PresentationInterfaces;
using Sources.Utils;
using Sources.Utils.Enums;
using UnityEngine;
using VContainer;

namespace Sources.Controllers.Factories
{
	public class PlayerFactory : IPlayerFactory
	{
		private readonly AnimationHasher _hasher;
		private readonly IAssetFactory _assetFactory;
		private readonly IObjectResolver _objectResolver;
		private readonly IGameplayInterfacePresenter _interfaceProvider;
		private readonly IPlayerModelRepository _modelRepository;

		private GameObject _character;
		private Rigidbody _body;
		private Animator _animator;

		public PlayerFactory(
			IAssetFactory assetFactory,
			IObjectResolver objectResolver,
			IGameplayInterfacePresenter interfaceProvider,
			IPlayerModelRepository modelRepository
		)
		{
			_assetFactory = assetFactory ?? throw new ArgumentNullException(nameof(assetFactory));
			_objectResolver = objectResolver ?? throw new ArgumentNullException(nameof(objectResolver));

			_interfaceProvider = interfaceProvider ?? throw new ArgumentNullException(nameof(interfaceProvider));
			_modelRepository = modelRepository ?? throw new ArgumentNullException(nameof(modelRepository));
		}

		private Joystick ImplementationJoystick => _interfaceProvider.Joystick;

		public GameObject Create(GameObject spawnPoint)
		{
			if (spawnPoint == null)
				throw new ArgumentNullException(
					nameof(spawnPoint)
				);

			Joystick joystick = ImplementationJoystick
				? ImplementationJoystick
				: throw new ArgumentNullException(nameof(ImplementationJoystick));

			IPlayer playerBodyComponentPresenter = GetPlayerBodyComponent(spawnPoint);

			_objectResolver.Inject(playerBodyComponentPresenter);

			_character = playerBodyComponentPresenter.GameObject;
			_body = _character.GetComponent<Rigidbody>();
			_animator = _character.GetComponentInChildren<Animator>();

			PlayerTransformable playerTransformable = new(
				_character.transform,
				joystick,
				_modelRepository.Get(
					(int)ProgressType.Speed
				),
				_body
			);

			SetPresenter(
				playerBodyComponentPresenter,
				playerTransformable,
				new AnimationHasher()
			);

			return _character;
		}

		private void SetPresenter(PlayerPresenter playerBodyComponentPresenter,
			PlayerTransformable playerTransformable,
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

		private IPlayer GetPlayerBodyComponent(GameObject spawnPoint) =>
			_assetFactory.InstantiateAndGetComponent <
			(PlayerBody)IPlayer >
			(
				ResourcesAssetPath.Scene.Player,
				spawnPoint.transform.position
			);
	}
}
