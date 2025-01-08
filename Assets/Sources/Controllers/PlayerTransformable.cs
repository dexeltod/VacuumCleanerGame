using System;
using JetBrains.Annotations;
using Plugins.Joystick_Pack.Scripts.Base;
using Sources.Controllers.Common;
using Sources.DomainInterfaces.Entities;
using Sources.PresentationInterfaces;
using UnityEngine;

namespace Sources.Controllers
{
	public class PlayerTransformable : Transformable, IUpdatable
	{
		private const float MaxTransformHeight = 2.5f;

		public readonly float VacuumDistance;

		private readonly Joystick _joystick;
		private readonly IStatReadOnly _speedProgressValue;
		private readonly Rigidbody _rigidbody;

		private Vector3 _offset;

		private int _currentSpeedValue;

		public PlayerTransformable(
			Transform transform,
			Joystick joystick,
			IStatReadOnly speedProgressValue,
			Rigidbody rigidbody
		) : base(
			transform,
			rigidbody
		)
		{
			_joystick = joystick
				? joystick
				: throw new ArgumentNullException(
					nameof(joystick)
				);
			_speedProgressValue = speedProgressValue ??
			                      throw new ArgumentNullException(
				                      nameof(speedProgressValue)
			                      );
			_rigidbody = rigidbody ??
			             throw new ArgumentNullException(
				             nameof(rigidbody)
			             );
		}

		public void Update(float deltaTime) =>
			Move(
				deltaTime
			);

		private void Move(float deltaTime)
		{
			Vector3 joystickDirection = new Vector3(
				_joystick.Direction.x,
				0,
				_joystick.Direction.y
			);

			Vector3 direction = joystickDirection * (_speedProgressValue.Value * deltaTime);

			_offset = Transform.position + direction;

			if (Transform.position.y > MaxTransformHeight)
				_offset.y = MaxTransformHeight;

			MoveTo(
				_offset
			);
			LookAt(
				direction
			);
		}
	}
}
