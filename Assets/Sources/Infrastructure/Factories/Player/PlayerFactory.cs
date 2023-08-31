using Joystick_Pack.Scripts.Base;
using Sources.DIService;
using Sources.DomainInterfaces;
using Sources.Infrastructure.Scene;
using Sources.InfrastructureInterfaces;
using Sources.InfrastructureInterfaces.Factory;
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

		private IPresenterFactory _presenterFactory;

		private Joystick _joystick;
		private Animator _animator;
		private AnimationHasher _animationHasher;
		private AnimatorFacade _animatorFacade;
		private PlayerStatesFactory _playerStatesFactory;
		private IPlayerStatsService _playerStats;

		public GameObject Player { get; private set; }

		public PlayerFactory(IAssetProvider assetProvider)
		{
			_progressService = GameServices.Container.Get<IPersistentProgressService>();
			_assetProvider = assetProvider;
		}

		public void Instantiate(GameObject initialPoint, IPresenterFactory presenterFactory,
			Joystick joystick, IPlayerStatsService stats)
		{
			_playerStats = stats;
			_joystick = joystick;
			_presenterFactory = presenterFactory;
			Create(initialPoint);
		}

		private void Create(GameObject initialPoint)
		{
			Player playerPresenter = _presenterFactory.Instantiate<Player>(
				ResourcesAssetPath.Scene.Player,
				initialPoint.transform.position);

			var character = playerPresenter.gameObject;
			Player = character;
			Rigidbody rigidbody = Player.GetComponent<Rigidbody>();

			PlayerTransformable playerTransformable = new(Player.transform, _joystick, _playerStats);
			playerPresenter.Init(playerTransformable, rigidbody);
		}
	}
}