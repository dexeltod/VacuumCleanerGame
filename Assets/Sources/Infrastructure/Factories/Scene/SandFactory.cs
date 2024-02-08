using System;
using Sources.Presentation.SceneEntity;
using Sources.PresentationInterfaces;
using Sources.ServicesInterfaces;
using Sources.Utils.Configs.Scripts;

namespace Sources.Infrastructure.Factories.Scene
{
	public class SandFactory
	{
		private readonly IAssetFactory _assetFactory;

		public SandFactory(IAssetFactory assetFactory) =>
			_assetFactory = assetFactory ?? throw new ArgumentNullException(nameof(assetFactory));

		public IMeshModifiable Create()
		{
			var meshModificator = _assetFactory.InstantiateAndGetComponent<MeshModificator>
			(
				ResourcesAssetPath
					.GameObjects
					.ModifiableMesh
			);

			return meshModificator;
		}
	}
}