using System;
using Sources.Presentation.SceneEntity;
using Sources.PresentationInterfaces;
using Sources.ServicesInterfaces;
using Sources.Utils.Configs.Scripts;

namespace Sources.Infrastructure.Factories
{
	public class SandFactory
	{
		private readonly IAssetProvider _assetProvider;

		public SandFactory(IAssetProvider assetProvider) =>
			_assetProvider = assetProvider ?? throw new ArgumentNullException(nameof(assetProvider));

		public IMeshModifiable Create()
		{
			return _assetProvider.InstantiateAndGetComponent<MeshModificator>
			(
				ResourcesAssetPath
					.GameObjects
					.ModifiableMesh
			);
		}
	}
}