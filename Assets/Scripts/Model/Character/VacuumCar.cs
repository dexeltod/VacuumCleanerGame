using Model.SceneEntity;
using UnityEngine;

namespace Model.Character
{
	public class VacuumCar : Transformable, IUpdatable
	{
		private readonly Joystick _joystick;
		private readonly float _speed;
		private Vector3 _offset;

		public VacuumCar(Vector3 position, float rotation, Joystick joystick, float speed) : base(position, rotation)
		{
			_joystick = joystick;
			_speed = speed;
		}

		public void Update(float deltaTime)
		{
			Vector3 joystickDirection = new Vector3(_joystick.Direction.x, 0, _joystick.Direction.y);

			Vector3 direction = joystickDirection * _speed;
			_offset = Position + direction;
			MoveTo(_offset);
			
			if (joystickDirection != Vector3.zero)
				LookAt(direction);
		}
	}
}