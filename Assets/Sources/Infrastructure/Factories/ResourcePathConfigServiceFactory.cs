using System;
using Sources.Infrastructure.Common.Factory;
using Sources.Infrastructure.Services;
using Sources.ServicesInterfaces;

namespace Sources.Infrastructure.Factories
{
	public sealed class ResourcePathConfigServiceFactory : Factory<ResourcesPrefabs>
	{
		private const string ResourcePathConfigServicePath = "Config/ScriptableObjects/ResourcesPrefabs";

		private readonly IAssetFactory _assetFactory;

		public ResourcePathConfigServiceFactory(
			IAssetFactory assetFactory
		) =>
			_assetFactory
				= assetFactory ?? throw new ArgumentNullException(nameof(assetFactory));

		public override ResourcesPrefabs Create() =>
			_assetFactory.LoadFromResources<ResourcesPrefabs>(ResourcePathConfigServicePath);
	}
}