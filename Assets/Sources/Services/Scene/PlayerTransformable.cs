using Sources.Domain.Progress;
using Sources.DomainInterfaces;
using Sources.Services.Interfaces;
using Sources.ServicesInterfaces;
using UnityEngine;

namespace Sources.Services.Scene
{
	public class PlayerTransformable : Transformable, IUpdatable
	{
		private const float MaxMoveHeight = 2f;

		public readonly float VacuumDistance;

		private readonly Joystick _joystick;
		private readonly float _speed;
		private Vector3 _offset;

		public PlayerTransformable(Transform transform, Joystick joystick, IPlayerStatsService stats) : base(transform)
		{
			int speed = stats.GetConvertedProgressValue("Speed");
			//TODO: Need to implement vacuum distance
			_speed = speed;
			_joystick = joystick;
		}

		public void Update(float deltaTime) =>
			Move(deltaTime);

		private void Move(float deltaTime)
		{
			Vector3 joystickDirection = new Vector3(_joystick.Direction.x, 0, _joystick.Direction.y);
			Vector3 direction = joystickDirection * (_speed * deltaTime);

			_offset = Transform.position + direction;

			if (Transform.position.y > MaxMoveHeight)
				_offset.y = MaxMoveHeight;

			MoveTo(_offset);

			if (joystickDirection != Vector3.zero)
				LookAt(direction);
		}
	}
}