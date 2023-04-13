using System;
using UnityEngine;

namespace Model.SceneEntity
{
	public abstract class Transformable
	{
		public virtual Vector3 Position { get; private set; }
		public float Rotation { get; private set; }
		public Vector3 LookDirection { get; private set; }

		public event Action<Vector3> Moved;
		public event Action<Vector3> Looked;
		public event Action Destroying;

		public Transformable(Vector3 position, float rotation)
		{
			Position = position;
			Rotation = rotation;
		}

		public void LookAt(Vector3 direction)
		{
			LookDirection = direction;
			Looked?.Invoke(LookDirection);
		}

		public void MoveTo(Vector3 position)
		{
			Position = position;
			Moved?.Invoke(position);
		}

		public void Destroy()
		{
			Destroying?.Invoke();
		}
	}
}