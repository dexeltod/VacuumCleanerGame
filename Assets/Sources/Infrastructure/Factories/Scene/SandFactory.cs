using System;
using Sources.Presentation.SceneEntity;
using Sources.PresentationInterfaces;
using Sources.ServicesInterfaces;
using Sources.Utils.Configs.Scripts;

namespace Sources.Infrastructure.Factories.Scene
{
	public class SandFactory
	{
		private readonly IAssetResolver _assetResolver;

		public SandFactory(IAssetResolver assetResolver) =>
			_assetResolver = assetResolver ?? throw new ArgumentNullException(nameof(assetResolver));

		public IMeshModifiable Create()
		{
			var meshModificator = _assetResolver.InstantiateAndGetComponent<MeshModificator>
			(
				ResourcesAssetPath
					.GameObjects
					.ModifiableMesh
			);

			return meshModificator;
		}
	}
}