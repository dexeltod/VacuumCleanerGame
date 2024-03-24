using System;
using System.Collections.Generic;
using Sources.ControllersInterfaces;
using Sources.InfrastructureInterfaces.Providers;
using UnityEngine;

namespace Sources.Controllers.Mesh
{
	public class MeshDeformationPresenter : IMeshDeformationController
	{
		private readonly float _meshModificatorRadiusDeformation;
		private readonly Dictionary<int, MeshCollider> _meshes;

		private readonly IResourcesProgressPresenterProvider _resourcesProgressPresenterProvider;

		public MeshDeformationPresenter(
			IResourcesProgressPresenterProvider resourcePresenter,
			float meshModificatorRadiusDeformation,
			Dictionary<int, MeshCollider> meshes
		)
		{
			if (meshModificatorRadiusDeformation < 0)
				throw new ArgumentOutOfRangeException(nameof(meshModificatorRadiusDeformation));

			_resourcesProgressPresenterProvider
				= resourcePresenter ?? throw new ArgumentNullException(nameof(resourcePresenter));
			_meshModificatorRadiusDeformation = meshModificatorRadiusDeformation;
			_meshes = meshes ?? throw new ArgumentNullException(nameof(meshes));
		}

		private IResourcesProgressPresenter ResourcesProgressPresenter =>
			_resourcesProgressPresenterProvider.Implementation;

		public void OnCollisionHappen(int scoreCount, Transform transform, Collision collision, int id)
		{
			if (ResourcesProgressPresenter.IsMaxScoreReached == false)
				return;

			bool isDeforming = false;

			UnityEngine.Mesh mesh = _meshes[id].sharedMesh;
			Vector3[] vertices = mesh.vertices;

			for (int i = 0; i < mesh.vertexCount; i++)
			{
				foreach (ContactPoint contact in collision.contacts)
				{
					Vector3 point = transform.InverseTransformPoint(contact.point);
					float distance = Vector3.Distance(point, vertices[i]);

					if (!(distance < _meshModificatorRadiusDeformation)) continue;

					Vector3 newVertex = Calculate(distance);
					vertices[i] += newVertex;
					isDeforming = true;
				}
			}

			if (!isDeforming)
				return;

			Recalculate(vertices, mesh, id);

			ResourcesProgressPresenter.TryAddSand(scoreCount);
		}

		private Vector3 Calculate(float distance) =>
			(_meshModificatorRadiusDeformation - distance) * Vector3.down;

		private void Recalculate(Vector3[] vertices, UnityEngine.Mesh mesh, int id)
		{
			mesh.vertices = vertices;
			mesh.RecalculateNormals();
			mesh.RecalculateBounds();

			_meshes[id].sharedMesh = mesh;
		}
	}
}