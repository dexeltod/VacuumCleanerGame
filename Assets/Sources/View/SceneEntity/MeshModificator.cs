using System;
using System.Collections.Generic;
using Sources.DIService;
using Sources.PresentationInterfaces;
using Sources.ServicesInterfaces;
using UnityEngine;

namespace Sources.View.SceneEntity
{
	public class MeshModificator : MonoBehaviour, IMeshModifiable
	{
		[SerializeField] private float _radiusDeformation = 2;
		[SerializeField] private int _pointPerOneSand = 1;

		private List<Vector3> _newVertices;
		private IResourcesProgressPresenter _resourcesProgressPresenter;

		public MeshModificator(Collision collision)
		{
			Collision = collision;
		}

		public float RadiusDeformation => _radiusDeformation;

		public int PointPerOneSand => _pointPerOneSand;

		public Mesh Mesh { get; private set; }
		public event Action<int, Transform> CollisionHappen;

		public Collision Collision { get; private set; }

		public MeshCollider GetMeshCollider() =>
			GetComponent<MeshCollider>();

		private void Start()
		{
			Mesh = GetComponent<MeshFilter>().mesh;
			_resourcesProgressPresenter = ServiceLocator.Container.Get<IResourcesProgressPresenter>();
		}

		private void OnCollisionEnter(Collision collision)
		{
			if (_resourcesProgressPresenter.CheckMaxScore() == false)
				return;

			if (collision.collider.TryGetComponent(out VacuumTool _))
			{
				Collision = collision;
				CollisionHappen.Invoke(_pointPerOneSand, transform);
			}
		}
	}
}