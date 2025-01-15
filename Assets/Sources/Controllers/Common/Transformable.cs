using System;
using UnityEngine;

namespace Sources.Controllers.Common
{
	public abstract class Transformable
	{
		private readonly Rigidbody _rigidbody;
		public virtual Transform Transform { get; private set; }
		public Vector3 LookDirection { get; private set; }

		public event Action<Vector3> Moved;
		public event Action<Vector3> Looked;
		public event Action Destroying;

		protected Transformable(Transform transform, Rigidbody rigidbody)
		{
			_rigidbody = rigidbody;
			Transform = transform;
		}

		protected void LookAt(Vector3 direction)
		{
			LookDirection = direction;
			Looked?.Invoke(LookDirection);
		}

		protected void MoveTo(Vector3 position)
		{
			_rigidbody.position = position;
			Moved?.Invoke(position);
		}

		public void Destroy()
		{
			Destroying?.Invoke();
		}
	}
}