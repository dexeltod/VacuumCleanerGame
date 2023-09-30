using System;
using UnityEngine;

namespace Sources.PresentationInterfaces
{
	public interface IMeshModifiable
	{
		event Action<int, Transform> CollisionHappen;
		Mesh Mesh{ get;}
		float RadiusDeformation { get; }
		int PointPerOneSand { get; }
		MeshCollider GetMeshCollider();
		Collision Collision { get; }
	}
}