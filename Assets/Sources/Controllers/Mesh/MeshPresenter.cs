using System;
using Sources.Controllers.Common;
using Sources.ControllersInterfaces;
using Sources.InfrastructureInterfaces.Presenters;

namespace Sources.Controllers.Mesh
{
	public class MeshPresenter : Presenter, IDisposable
	{
		private readonly IMeshDeformationPresenter _meshModifiable;
		private readonly IResourcesProgressPresenter _resourceService;

		public MeshPresenter(
			IMeshDeformationPresenter meshModifiable,
			IResourcesProgressPresenter resourceService
		)
		{
			_resourceService = resourceService ?? throw new ArgumentNullException(nameof(resourceService));
			_meshModifiable = meshModifiable ?? throw new ArgumentNullException(nameof(meshModifiable));
			_meshModifiable.MeshDeformed += OnCollisionHappen;
		}

		private void OnCollisionHappen(int pointCount) =>
			_resourceService.TryAddSand(pointCount);

		public void Dispose() =>
			_meshModifiable.MeshDeformed -= OnCollisionHappen;
	}
}