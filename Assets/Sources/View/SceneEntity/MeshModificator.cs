using System.Collections.Generic;
using Application.DI;
using InfrastructureInterfaces;
using UnityEngine;

namespace View.SceneEntity
{
	public class MeshModificator : MonoBehaviour
	{
		[SerializeField] private float _radiusDeformate;

		private const int SendScoreCount = 1;

		private Mesh _mesh;
		private List<Vector3> _newVertices;
		private IResourcesProgressViewModel _resourcesProgressViewModel;

		private void Start()
		{
			_mesh = GetComponent<MeshFilter>().mesh;
			_resourcesProgressViewModel = ServiceLocator.Container.GetSingle<IResourcesProgressViewModel>();
		}

		private void OnCollisionEnter(Collision collision)
		{
			if (_resourcesProgressViewModel.CheckMaxScore() == false)
				return;

			if (collision.collider.TryGetComponent(out VacuumTool _))
			{
				bool isDeforming = false;
				Vector3[] vertices = _mesh.vertices;

				for (int i = 0; i < _mesh.vertexCount; i++)
				{
					for (int j = 0; j < collision.contacts.Length; j++)
					{
						Vector3 point = transform.InverseTransformPoint(collision.contacts[j].point);
						float distance = Vector3.Distance(point, vertices[i]);

						if (distance < _radiusDeformate)
						{
							Vector3 newVertex = (_radiusDeformate - distance) * Vector3.down;
							vertices[i] += newVertex;
							isDeforming = true;
						}
					}
				}

				if (isDeforming)
				{
					_mesh.vertices = vertices;
					_mesh.RecalculateNormals();
					_mesh.RecalculateBounds();
					GetComponent<MeshCollider>().sharedMesh = _mesh;
					_resourcesProgressViewModel.AddSand(SendScoreCount);
				}
			}
		}
	}
}