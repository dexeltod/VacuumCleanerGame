using System;
using Sources.ControllersInterfaces;
using UnityEngine;

namespace Sources.Controllers.Common
{
	public abstract class Transformable : ITransformable
	{
		private readonly Rigidbody _rigidbody;

		protected Transformable(Transform transform, Rigidbody rigidbody)
		{
			_rigidbody = rigidbody;
			Transform = transform;
		}

		public virtual Transform Transform { get; }
		public Vector3 LookDirection { get; private set; }

		public event Action<Vector3> Moved;
		public event Action<Vector3> FixedMoved;
		public event Action<Vector3> Looked;
		public event Action Destroying;

		public void Destroy()
		{
			Destroying?.Invoke();
		}

		protected void LookAt(Vector3 direction)
		{
			LookDirection = direction;
			Looked?.Invoke(LookDirection);
		}

		protected void MoveTo(Vector3 position)
		{
			_rigidbody.position = position;
			FixedMoved?.Invoke(position);
		}
	}
}
