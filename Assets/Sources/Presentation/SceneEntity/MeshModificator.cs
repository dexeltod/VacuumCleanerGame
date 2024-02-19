using System;
using System.Collections.Generic;
using Sources.ControllersInterfaces;
using Sources.InfrastructureInterfaces.Providers;
using Sources.PresentationInterfaces;
using UnityEngine;
using VContainer;

namespace Sources.Presentation.SceneEntity
{
	public class MeshModificator : MonoBehaviour, IMeshModifiable
	{
		[SerializeField] private float _radiusDeformation = 2;
		[SerializeField] private int _pointPerOneSand = 1;

		private IResourcesProgressPresenterProvider _resourcesProgressPresenterProvider;
		private List<Vector3> _newVertices;

		private IResourcesProgressPresenter ResourcesProgressPresenter => _resourcesProgressPresenterProvider.Implementation;

		public MeshModificator(Collision collision) =>
			Collision = collision ?? throw new ArgumentNullException(nameof(collision));

		public Mesh Mesh { get; private set; }
		public float RadiusDeformation => _radiusDeformation;

		public int PointPerOneSand => _pointPerOneSand;

		public Collision Collision { get; private set; }
		public event Action<int, Transform> CollisionHappen;

		[Inject]
		public void Construct(IResourcesProgressPresenterProvider resourcesProgressPresenterProvider) =>
			_resourcesProgressPresenterProvider = resourcesProgressPresenterProvider;

		public MeshCollider GetMeshCollider() =>
			GetComponent<MeshCollider>();

		private void Start() =>
			Mesh = GetComponent<MeshFilter>().mesh;

		private void OnCollisionEnter(Collision collision)
		{
			if (ResourcesProgressPresenter.IsMaxScoreReached == false)
				return;
		
			if (collision.collider.TryGetComponent(out VacuumTool _))
			{
				Collision = collision;
				CollisionHappen!.Invoke(_pointPerOneSand, transform);
			}
		}
	}
}