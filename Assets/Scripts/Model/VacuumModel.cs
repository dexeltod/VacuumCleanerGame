using Model.SceneEntity;
using Presenter.SceneEntity;
using UnityEngine;

namespace Model.Character
{
	public class VacuumModel : Transformable, IUpdatable
	{
		private readonly Joystick _joystick;
		private readonly float _speed;
		private Vector3 _offset;
		private Terrain _terrain;

		public float VacuumDistance { get; private set; }

		public VacuumModel(Transform transform, Joystick joystick, float speed, float vacuumDistance) : base(
			transform)
		{
			_speed = speed;
			_joystick = joystick;
			VacuumDistance = vacuumDistance;
		}

		public void Update(float deltaTime)
		{
			Move(deltaTime);

			Physics.Raycast(Transform.position, Transform.forward, out RaycastHit hitInfo, VacuumDistance);

			Debug.DrawLine(Transform.position, Transform.position + Transform.forward * VacuumDistance, Color.red);

			if (hitInfo.collider == null)
				return;

			if (hitInfo.collider.TryGetComponent(out Sand sand))
			{
				if (_terrain == null)
					_terrain = sand.GetComponent<Terrain>();

				var mesh = sand.GetComponent<MeshRenderer>();

				Vector3 hitPoint = hitInfo.point;
			}
		}

		private void Move(float deltaTime)
		{
			Vector3 joystickDirection = new Vector3(_joystick.Direction.x, 0, _joystick.Direction.y);
			Vector3 direction = joystickDirection * (_speed * deltaTime);
			_offset = Transform.position + direction;
			MoveTo(_offset);

			if (joystickDirection != Vector3.zero)
				LookAt(direction);
		}
	}
}