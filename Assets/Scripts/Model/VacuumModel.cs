using Model.SceneEntity;
using UnityEngine;

namespace Model
{
	public class VacuumModel : Transformable, IUpdatable
	{
		private const float MaxHeight = 2f;

		private readonly Joystick _joystick;
		private readonly float _speed;
		private Vector3 _offset;
		private Terrain _terrain;

		public float VacuumDistance { get; private set; }

		public VacuumModel(Transform transform, Joystick joystick, float speed,
			float vacuumDistance) : base(transform)
		{
			_speed = speed;
			_joystick = joystick;
			VacuumDistance = vacuumDistance;
		}

		public void Update(float deltaTime) =>
			Move(deltaTime);

		private void Move(float deltaTime)
		{
			Vector3 joystickDirection = new Vector3(_joystick.Direction.x, 0, _joystick.Direction.y);
			Vector3 direction = joystickDirection * (_speed * deltaTime);

			_offset = Transform.position + direction;

			if (Transform.position.y > MaxHeight)
				_offset.y = MaxHeight;

			MoveTo(_offset);

			if (joystickDirection != Vector3.zero)
				LookAt(direction);
		}
	}
}