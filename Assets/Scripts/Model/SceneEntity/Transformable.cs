using System;
using UnityEngine;

namespace Model.SceneEntity
{
	public abstract class Transformable
	{
		public virtual Transform Transform { get; private set; }
		public Vector3 LookDirection { get; private set; }

		public event Action<Vector3> Moved;
		public event Action<Vector3> Looked;
		public event Action Destroying;

		public Transformable(Transform transform)
		{
			Transform = transform;
		}

		public void LookAt(Vector3 direction)
		{
			LookDirection = direction;
			Looked?.Invoke(LookDirection);
		}

		public void MoveTo(Vector3 position)
		{
			Transform.position = position;
			Moved?.Invoke(position);
		}

		public void Destroy()
		{
			Destroying?.Invoke();
		}
	}
}