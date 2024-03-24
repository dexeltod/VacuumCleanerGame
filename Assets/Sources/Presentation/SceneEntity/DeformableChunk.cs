using System;
using Sources.Controllers;
using Sources.Controllers.Mesh;
using Sources.ControllersInterfaces;
using Sources.InfrastructureInterfaces.Providers;
using UnityEngine;

namespace Sources.Presentation.SceneEntity
{
	public class DeformableChunk : MonoBehaviour
	{
		private const string VacuumColliderBottom = "VacuumColliderBottom";
		private const string VacuumColliderTop = "VacuumColliderTop";

		private float _radiusDeformation = 2;
		private int _pointPerOneSand = 1;

		private IResourcesProgressPresenterProvider _resourcesProgressPresenterProvider;
		private MeshDeformationPresenter _meshDeformationPresenter;
		private int _id;

		private IResourcesProgressPresenter ResourcesProgressPresenter =>
			_resourcesProgressPresenterProvider.Implementation;

		public float RadiusDeformation => _radiusDeformation;

		public int PointPerOneSand => _pointPerOneSand;

		public void Construct(
			float radiusDeformation,
			int pointPerOneSand,
			IResourcesProgressPresenterProvider resourcesProgressPresenterProvider,
			MeshDeformationPresenter meshDeformationPresenter,
			int id
		)
		{
			if (radiusDeformation < 0) throw new ArgumentOutOfRangeException(nameof(radiusDeformation));
			if (pointPerOneSand < 0) throw new ArgumentOutOfRangeException(nameof(pointPerOneSand));
			if (id < 0) throw new ArgumentOutOfRangeException(nameof(id));
			_id = id;

			_radiusDeformation = radiusDeformation;
			_pointPerOneSand = pointPerOneSand;

			_meshDeformationPresenter = meshDeformationPresenter ??
				throw new ArgumentNullException(nameof(meshDeformationPresenter));
			_resourcesProgressPresenterProvider = resourcesProgressPresenterProvider ??
				throw new ArgumentNullException(nameof(resourcesProgressPresenterProvider));
		}

		private void OnCollisionEnter(Collision collision)
		{
			if (ResourcesProgressPresenter.IsMaxScoreReached == false)
				return;

			if (collision.collider.name is VacuumColliderBottom or VacuumColliderTop)
				_meshDeformationPresenter.OnCollisionHappen(_pointPerOneSand, transform, collision, _id);
		}
	}
}