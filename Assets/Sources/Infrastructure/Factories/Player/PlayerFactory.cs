using System;
using Joystick_Pack.Scripts.Base;
using Sources.DIService;
using Sources.DomainInterfaces;
using Sources.Infrastructure.Scene;
using Sources.Services;
using Sources.Services.Character;
using Sources.ServicesInterfaces;
using Sources.Utils.Configs;
using UnityEngine;

namespace Sources.Infrastructure.Factories.Player
{
	public class PlayerFactory : IPlayerFactory
	{
		private readonly AnimationHasher _hasher;
		private readonly IAssetProvider _assetProvider;
		private readonly IPersistentProgressService _progressService;

		private Joystick _joystick;
		private Animator _animator;
		private AnimationHasher _animationHasher;
		private AnimatorFacade _animatorFacade;
		private PlayerStatesFactory _playerStatesFactory;
		private IPlayerStatsService _playerStats;

		public GameObject Player { get; private set; }

		public PlayerFactory(IAssetProvider assetProvider)
		{
			_progressService = ServiceLocator.Container.Get<IPersistentProgressService>();
			_assetProvider = assetProvider;
		}

		public GameObject Create
		(
			GameObject initialPoint,
			Joystick joystick,
			IPlayerStatsService stats,
			Action onErrorCallback = null
		)
		{
			try
			{
				_playerStats = stats;
				_joystick = joystick;
				return Create(initialPoint);
			}
			catch (Exception e)
			{
				onErrorCallback?.Invoke();
			}

			throw new NullReferenceException("GameObject is null");
		}

		private GameObject Create(GameObject initialPoint)
		{
			Player playerPresenter = _assetProvider.InstantiateAndGetComponent<Player>
			(
				ResourcesAssetPath.Scene.Player,
				initialPoint.transform.position
			);

			GameObject character = playerPresenter.gameObject;
			Player = character;
			Rigidbody rigidbody = Player.GetComponent<Rigidbody>();

			PlayerTransformable playerTransformable = new(Player.transform, _joystick, _playerStats);
			playerPresenter.Init(playerTransformable, rigidbody);

			return character;
		}
	}
}