using System;
using Sources.ControllersInterfaces;
using Sources.PresentationInterfaces;
using Sources.ServicesInterfaces;
using UnityEngine;

namespace Sources.Controllers.Mesh
{
	public class MeshDeformationController : IMeshDeformationPresenter, IDisposable
	{
		private readonly IMeshModifiable _meshModifiable;
		private readonly IResourcesProgressPresenter _resourceMaxScore;

		public MeshDeformationController(IMeshModifiable meshModifiable, IResourcesProgressPresenter resourceMaxScore)
		{
			_meshModifiable = meshModifiable ?? throw new ArgumentNullException(nameof(meshModifiable));
			_resourceMaxScore = resourceMaxScore ?? throw new ArgumentNullException(nameof(resourceMaxScore));
			_meshModifiable.CollisionHappen += OnCollisionHappen;
		}

		public void Dispose() =>
			_meshModifiable.CollisionHappen -= OnCollisionHappen;

		private void OnCollisionHappen(int scoreCount, Transform transform)
		{
			if (_resourceMaxScore.IsMaxScoreReached == false)
				return;

			bool isDeforming = false;
			Vector3[] vertices = _meshModifiable.Mesh.vertices;

			for (int i = 0; i < _meshModifiable.Mesh.vertexCount; i++)
			{
				foreach (ContactPoint contact in _meshModifiable.Collision.contacts)
				{
					Vector3 point = transform.InverseTransformPoint(contact.point);
					float distance = Vector3.Distance(point, vertices[i]);

					if (!(distance < _meshModifiable.RadiusDeformation)) continue;

					Vector3 newVertex = Calculate(distance);
					vertices[i] += newVertex;
					isDeforming = true;
				}
			}

			if (!isDeforming)
				return;

			Recalculate(vertices);

			_resourceMaxScore.TryAddSand(scoreCount);
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