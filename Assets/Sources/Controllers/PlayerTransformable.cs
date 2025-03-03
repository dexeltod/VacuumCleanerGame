using System;
using Plugins.Joystick_Pack.Scripts.Base;
using Sources.Controllers.Common;
using Sources.DomainInterfaces.Entities;
using Sources.PresentationInterfaces;
using Sources.Utils;
using UnityEngine;

namespace Sources.Controllers
{
	public class PlayerTransformable : Transformable, IUpdatable
	{
		private const float DistanceToMove = 1f;
		private const float MaxTransformHeight = 2.5f;

		private readonly Rigidbody _body;
		private readonly Vector3 _boxCastHalfExtents; // Размер BoxCast
		private readonly LayerMask _collisionMask; // Маска для слоев столкновений

		private readonly Joystick _joystick;
		private readonly IStatReadOnly _speedProgress;

		private int _currentSpeedValue;
		private Vector3 _movementDirection;

		private Vector3 _offset;

		public PlayerTransformable(
			Transform transform,
			Joystick joystick,
			IStatReadOnly speedProgressValue,
			Rigidbody body,
			Collider playerCollider
		) : base(
			transform,
			body
		)
		{
			_joystick = joystick ?? throw new ArgumentNullException(nameof(joystick));
			_speedProgress = speedProgressValue ?? throw new ArgumentNullException(nameof(speedProgressValue));
			_body = body ?? throw new ArgumentNullException(nameof(body));
			Collider collider = playerCollider ?? throw new ArgumentNullException(nameof(playerCollider));

			switch (collider)
			{
				case BoxCollider boxCollider:
					_boxCastHalfExtents = boxCollider.size * 0.5f;
					break;
				default:
					Debug.LogWarning("Unsupported Collider type! Using default BoxCast size.");
					_boxCastHalfExtents = Vector3.one * 0.5f;
					break;
			}

			_collisionMask = LayerMask.GetMask(LayerService.GetNameByType(LayerType.Bound));
		}

		public void FixedUpdate() => Move();

		private bool IsClose(Vector3 direction, float distance)
		{
			Vector3 center = _body.position;

#if UNITY_EDITOR
			Debug.DrawRay(center, direction * distance, Color.red);
#endif

			return Physics.SphereCast(center, _boxCastHalfExtents.magnitude, direction, out _, distance, _collisionMask);
		}

		private void Move()
		{
			var joystickDirection = new Vector3(
				_joystick.Direction.x,
				0,
				_joystick.Direction.y
			);

			_movementDirection = joystickDirection * (_speedProgress.Value * Time.fixedDeltaTime);

			if (_body.position.y > MaxTransformHeight)
				_offset.y = MaxTransformHeight;

			var steps = 10;
			Vector3 step = _movementDirection / steps;

			for (var i = 0; i < steps; i++)
				if (!IsClose(step.normalized, step.magnitude))
				{
					_offset = _body.position + step;
					MoveTo(_offset);
				}
				else
				{
					break;
				}

			LookAt(joystickDirection * _speedProgress.Value);
		}
	}
}
