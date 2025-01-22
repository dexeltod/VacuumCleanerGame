using System;
using UnityEngine;

namespace Sources.ControllersInterfaces
{
	public interface ITransformable
	{
		Transform Transform { get; }
		Vector3 LookDirection { get; }
		void Destroy();
		event Action<Vector3> Moved;
		event Action<Vector3> Looked;
		event Action Destroying;
	}
}