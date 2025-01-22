using System;
using Sources.ControllersInterfaces;
using UnityEngine;

namespace Sources.Controllers.Mesh
{
	public class MeshDeformationPresenter : IMeshDeformationController
	{
		private readonly MeshCollider _meshCollider;
		private readonly UnityEngine.Mesh _meshModifiable;

		private readonly float _meshModificatorRadiusDeformation;

		private readonly IResourcesProgressPresenter _resourceMaxScore;

		public MeshDeformationPresenter(
			UnityEngine.Mesh meshModifiable,
			IResourcesProgressPresenter resourcePresenter,
			float meshModificatorRadiusDeformation,
			MeshCollider meshCollider
		)
		{
			if (meshModificatorRadiusDeformation < 0)
				throw new ArgumentOutOfRangeException(nameof(meshModificatorRadiusDeformation));

			_meshModifiable = meshModifiable ? meshModifiable : throw new ArgumentNullException(nameof(meshModifiable));
			_resourceMaxScore = resourcePresenter ?? throw new ArgumentNullException(nameof(resourcePresenter));
			_meshModificatorRadiusDeformation = meshModificatorRadiusDeformation;
			_meshCollider = meshCollider ? meshCollider : throw new ArgumentNullException(nameof(meshCollider));
		}

		public void OnCollisionHappen(int scoreCount, Transform transform, Collision collision)
		{
			if (_resourceMaxScore.IsMaxScoreReached == false)
				return;

			var isDeforming = false;
			Vector3[] vertices = _meshModifiable.vertices;

			for (var i = 0; i < _meshModifiable.vertexCount; i++)
				foreach (ContactPoint contact in collision.contacts)
				{
					Vector3 point = transform.InverseTransformPoint(contact.point);
					float distance = Vector3.Distance(point, vertices[i]);

					if (!(distance < _meshModificatorRadiusDeformation)) continue;

					Vector3 newVertex = Calculate(distance);
					vertices[i] += newVertex;
					isDeforming = true;
				}

			if (!isDeforming)
				return;

			Recalculate(vertices);

			_resourceMaxScore.TryAddSand(scoreCount);
		}

		private Vector3 Calculate(float distance) =>
			(_meshModificatorRadiusDeformation - distance) * Vector3.down;

		private void Recalculate(Vector3[] vertices)
		{
			_meshModifiable.vertices = vertices;
			_meshModifiable.RecalculateNormals();
			_meshModifiable.RecalculateBounds();

			_meshCollider.sharedMesh = _meshModifiable;
		}
	}
}