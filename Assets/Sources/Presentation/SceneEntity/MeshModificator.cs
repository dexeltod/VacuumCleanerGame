using System;
using System.Collections.Generic;
using Sources.Controllers.Mesh;
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

		private Vector3[] _initialVertices;

		private IResourcesProgressPresenterProvider _resourcesProgressPresenterProvider;
		private MeshDeformationPresenter _meshDeformationPresenter;

		private IResourcesProgressPresenter ResourcesProgressPresenter =>
			_resourcesProgressPresenterProvider.Implementation;

		public float RadiusDeformation => _radiusDeformation;

		public int PointPerOneSand => _pointPerOneSand;

		public void Construct(
			IResourcesProgressPresenterProvider resourcesProgressPresenterProvider,
			MeshDeformationPresenter meshDeformationPresenter
		)
		{
			_meshDeformationPresenter = meshDeformationPresenter;
			_resourcesProgressPresenterProvider = resourcesProgressPresenterProvider;
		}

		private void OnCollisionEnter(Collision collision)
		{
			if (ResourcesProgressPresenter.IsMaxScoreReached == false)
				return;

			if (collision.collider.name is "VacuumColliderBottom" or "VacuumColliderTop")
				_meshDeformationPresenter.OnCollisionHappen(_pointPerOneSand, transform, collision);
		}
	}
}