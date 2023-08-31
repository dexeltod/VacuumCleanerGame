using Joystick_Pack.Scripts.Base;
using Sources.DomainInterfaces;
using Sources.ServicesInterfaces;
using UnityEngine;

namespace Sources.Infrastructure.Scene
{
	public class PlayerTransformable : Transformable, IUpdatable
	{
		private const float MaxTransformHeight = 2f;
		private const string SpeedName = "Speed";

		public readonly float VacuumDistance;
		
		private float _speed;
		private readonly Joystick _joystick;
		private readonly IPlayerStat _speedStat;

		private Vector3 _offset;

		public PlayerTransformable(Transform transform, Joystick joystick, IPlayerStatsService stats) : base(transform)
		{
			_speedStat = stats.GetPlayerStat(SpeedName);
			_speedStat.ValueChanged += OnPlayerStatChanged;

			_speed = _speedStat.Value;
			_joystick = joystick;
		}

		private void OnPlayerStatChanged()
		{
			_speed = _speedStat.Value;
		}

		public void Update(float deltaTime) =>
			Move(deltaTime);

		private void Move(float deltaTime)
		{
			Vector3 joystickDirection = new Vector3(_joystick.Direction.x, 0, _joystick.Direction.y);
			Vector3 direction = joystickDirection * (_speed * deltaTime);

			_offset = Transform.position + direction;

			if (Transform.position.y > MaxTransformHeight)
				_offset.y = MaxTransformHeight;

			MoveTo(_offset);

			if (joystickDirection != Vector3.zero)
				LookAt(direction);
		}
	}
}