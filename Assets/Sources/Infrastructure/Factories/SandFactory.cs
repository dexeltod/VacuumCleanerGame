using Sources.PresentationInterfaces;
using Sources.ServicesInterfaces;
using Sources.Utils.Configs;
using Sources.View.SceneEntity;

namespace Sources.Infrastructure.Factories
{
	public class SandFactory
	{
		private readonly IAssetProvider _assetProvider;

		public SandFactory(IAssetProvider assetProvider) => 
			_assetProvider = assetProvider;

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