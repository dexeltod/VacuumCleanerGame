using System;
using Joystick_Pack.Scripts.Base;
using Sources.Infrastructure.Presenters;
using Sources.Services;
using Sources.ServicesInterfaces;
using Sources.Utils.Configs.Scripts;
using UnityEngine;

namespace Sources.Infrastructure.Factories.Player
{
	public class PlayerFactory : IPlayerFactory
	{
		private readonly AnimationHasher _hasher;
		private readonly IAssetProvider _assetProvider;

		private Joystick _joystick;
		private Animator _animator;
		private AnimationHasher _animationHasher;
		private AnimatorFacade _animatorFacade;
		private IPlayerStatsService _playerStats;

		public GameObject Player { get; private set; }

		public PlayerFactory(IAssetProvider assetProvider) =>
			_assetProvider = assetProvider ?? throw new ArgumentNullException(nameof(assetProvider));

		public GameObject Create(
			GameObject initialPoint,
			Joystick joystick,
			IPlayerStatsService stats,
			Action onErrorCallback = null
		)
		{
			if (initialPoint == null) throw new ArgumentNullException(nameof(initialPoint));
			if (joystick == null) throw new ArgumentNullException(nameof(joystick));
			if (stats == null) throw new ArgumentNullException(nameof(stats));

			try
			{
				_playerStats = stats;
				_joystick = joystick;
				return Create(initialPoint);
			}
			catch (Exception)
			{
				onErrorCallback?.Invoke();
			}

			throw new NullReferenceException("GameObject is null");
		}

		private GameObject Create(GameObject initialPoint)
		{
			AnimationHasher animationHasher = new AnimationHasher();

			PlayerBody playerBodyPresenter = _assetProvider.InstantiateAndGetComponent<PlayerBody>(
				ResourcesAssetPath.Scene.Player,
				initialPoint.transform.position
			);

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