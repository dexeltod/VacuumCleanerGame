using System;
using Sources.ServicesInterfaces;

namespace Sources.Services
{
	public class MeshPresenter : IDisposable
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