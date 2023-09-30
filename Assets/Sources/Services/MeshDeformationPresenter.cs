using System;
using Sources.PresentationInterfaces;
using Sources.ServicesInterfaces;
using UnityEngine;

namespace Sources.Services
{
	public class MeshDeformationPresenter : IMeshDeformationPresenter, IDisposable
	{
		private readonly IMeshModifiable _meshModifiable;
		private readonly IResourceMaxScore _resourceMaxScore;

		public event Action<int> MeshDeformed;

		public MeshDeformationPresenter(IMeshModifiable meshModifiable, IResourceMaxScore resourceMaxScore)
		{
			_meshModifiable = meshModifiable;
			_resourceMaxScore = resourceMaxScore;
			_meshModifiable.CollisionHappen += OnCollisionHappen;
		}

		public void Dispose() => 
			_meshModifiable.CollisionHappen -= OnCollisionHappen;

		private void OnCollisionHappen(int scoreCount, Transform transform)
		{
			if (_resourceMaxScore.CheckMaxScore() == false)
				return;
			
			bool isDeforming = false;
			Vector3[] vertices = _meshModifiable.Mesh.vertices;

			for (int i = 0; i < _meshModifiable.Mesh.vertexCount; i++)
			{
				for (int j = 0; j < _meshModifiable.Collision.contacts.Length; j++)
				{
					Vector3 point = transform.InverseTransformPoint(_meshModifiable.Collision.contacts[j].point);
					float distance = Vector3.Distance(point, vertices[i]);

					if (distance < _meshModifiable.RadiusDeformation)
					{
						Vector3 newVertex = Calculate(distance);
						vertices[i] += newVertex;
						isDeforming = true;
					}
				}
			}

			if (isDeforming)
			{
				Recalculate(vertices);
				MeshDeformed.Invoke(scoreCount);
			}
		}

		private Vector3 Calculate(float distance) => 
			(_meshModifiable.RadiusDeformation - distance) * Vector3.down;

		private void Recalculate(Vector3[] vertices)
		{
			_meshModifiable.Mesh.vertices = vertices;
			_meshModifiable.Mesh.RecalculateNormals();
			_meshModifiable.Mesh.RecalculateBounds();
			_meshModifiable.GetMeshCollider().sharedMesh = _meshModifiable.Mesh;
		}
	}
}