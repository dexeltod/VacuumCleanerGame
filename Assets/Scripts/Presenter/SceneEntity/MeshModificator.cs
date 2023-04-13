using System;
using UnityEngine;

namespace Presenter.SceneEntity
{
	public class MeshModificator : MonoBehaviour
	{
		private Mesh _mesh;

		private void Start()
		{
			_mesh = GetComponent<MeshFilter>().mesh;
		}

		private void OnCollisionEnter(Collision collision)
		{
			// if (collision.collider.TryGetComponent(out VacuumTool _))
			// {
			// 	Vector3[] vertexes = _mesh.vertices;
			//
			// 	for (int i = 0; i < _mesh.vertexCount; i++)
			// 	{
			// 		for (int j = 0; j < collision.contacts.Length; j++)
			// 		{
			// 			Vector3 point = transform.InverseTransformPoint(collision.contacts[j].point);
			// 			Vector3
			// 			
			// 			vertexes[i] += 
			// 		}
			// 	}
			// }
		}
	}
}