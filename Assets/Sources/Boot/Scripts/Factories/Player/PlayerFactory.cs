using System;
using Plugins.Joystick_Pack.Scripts.Base;
using Sources.BusinessLogic.Interfaces.Factory;
using Sources.BusinessLogic.Repository;
using Sources.BusinessLogic.ServicesInterfaces;
using Sources.Controllers;
using Sources.Controllers.Common;
using Sources.ControllersInterfaces;
using Sources.Utils;
using Sources.Utils.Enums;
using UnityEngine;

namespace Sources.Boot.Scripts.Factories.Player
{
	public class PlayerFactory : IPlayerFactory
	{
		private readonly AnimationHasher _hasher;
		private readonly IInjectableAssetLoader _injectableAssetLoader;
		private readonly Joystick _joystick;
		private readonly IPlayerModelRepository _modelRepository;
		private Animator _animator;
		private Rigidbody _body;

		private GameObject _character;

		public PlayerFactory(
			IInjectableAssetLoader injectableAssetLoader,
			IPlayerModelRepository modelRepository,
			Joystick joystick
		)
		{
			_injectableAssetLoader = injectableAssetLoader ?? throw new ArgumentNullException(nameof(injectableAssetLoader));
			_modelRepository = modelRepository ?? throw new ArgumentNullException(nameof(modelRepository));
			_joystick = joystick ?? throw new ArgumentNullException(nameof(joystick));
		}

		public IMonoPresenter Create(GameObject spawnPoint)
		{
			if (spawnPoint == null)
				throw new ArgumentNullException(nameof(spawnPoint));

			PlayerBody playerBodyComponentPresenter = GetPlayerBodyComponent(spawnPoint);

			_character = playerBodyComponentPresenter.GameObject;
			_body = _character.GetComponent<Rigidbody>();
			_animator = _character.GetComponentInChildren<Animator>();

			PlayerTransformable playerTransformable = new(
				_character.transform,
				_joystick,
				_modelRepository.Get(ProgressType.Speed),
				_body
			);

			SetPresenter(
				playerBodyComponentPresenter,
				playerTransformable,
				new AnimationHasher()
			);

			return playerBodyComponentPresenter;
		}

		private PlayerBody GetPlayerBodyComponent(GameObject spawnPoint) =>
			_injectableAssetLoader.InstantiateAndGetComponent<PlayerBody>(
				ResourcesAssetPath.Scene.Player,
				spawnPoint.transform.position
			);

		private void SetPresenter(
			PlayerPresenter playerBodyComponentPresenter,
			PlayerTransformable playerTransformable,
			AnimationHasher animationHasher)
		{
			playerBodyComponentPresenter.Initialize(
				playerTransformable,
				_animator,
				animationHasher,
				playerBodyComponentPresenter.GetComponent<Rigidbody>()
			);
			playerBodyComponentPresenter.GameObject.SetActive(true);
		}
	}
}
