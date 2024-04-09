using Graphic.Joystick_Pack.Scripts.Base;
using Sources.Controllers.Common;
using Sources.DomainInterfaces;
using Sources.PresentationInterfaces;
using Sources.ServicesInterfaces;
using Sources.Utils;
using UnityEngine;

namespace Sources.Controllers
{
	public class PlayerTransformable : Transformable, IUpdatable
	{
		private const float MaxTransformHeight = 2f;

		public readonly float VacuumDistance;

		private readonly Joystick _joystick;
		private readonly IPlayerStat _speedStat;

		private Vector3 _offset;

		private float Speed => _speedStat.Value;

		public PlayerTransformable(
			Transform transform,
			Joystick joystick,
			IPlayerStatsService stats
		) : base(transform)
		{
			_speedStat = stats.Get(PlayerStatNames.Speed);

			_joystick = joystick;
		}

		public void Update(float deltaTime) =>
			Move(deltaTime);

		private void Move(float deltaTime)
		{
			Vector3 joystickDirection = new Vector3(_joystick.Direction.x, 0, _joystick.Direction.y);
			Vector3 direction = joystickDirection * (Speed * deltaTime);
			_offset = Transform.position + direction;

			if (Transform.position.y > MaxTransformHeight)
				_offset.y = MaxTransformHeight;

			MoveTo(_offset);
			LookAt(direction);
		}
	}
}