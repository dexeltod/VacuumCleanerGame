using System;
using Graphic.Joystick_Pack.Scripts.Base;
using Sources.Controllers.Common;
using Sources.Domain.Temp;
using Sources.PresentationInterfaces;
using UnityEngine;

namespace Sources.Controllers
{
	public class PlayerTransformable : Transformable, IUpdatable
	{
		private const float MaxTransformHeight = 2f;

		public readonly float VacuumDistance;

		private readonly Joystick _joystick;
		private readonly IModifiableStat _speed;

		private Vector3 _offset;

		private int _currentSpeedValue;

		public PlayerTransformable(
			Transform transform,
			Joystick joystick,
			IModifiableStat entity
		) : base(transform)
		{
			_joystick = joystick ? joystick : throw new ArgumentNullException(nameof(joystick));
			_speed = entity ?? throw new ArgumentNullException(nameof(entity));
		}

		public void Update(float deltaTime) =>
			Move(deltaTime);

		private void Move(float deltaTime)
		{
			Vector3 joystickDirection = new Vector3(_joystick.Direction.x, 0, _joystick.Direction.y);
			Vector3 direction = joystickDirection * (_speed.Value * deltaTime);
			_offset = Transform.position + direction;

			if (Transform.position.y > MaxTransformHeight)
				_offset.y = MaxTransformHeight;

			MoveTo(_offset);
			LookAt(direction);
		}
	}
}