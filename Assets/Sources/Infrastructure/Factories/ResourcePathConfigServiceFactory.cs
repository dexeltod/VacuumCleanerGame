using System;
using Sources.BusinessLogic.ServicesInterfaces;
using Sources.Infrastructure.Services;

namespace Sources.Infrastructure.Factories
{
	public sealed class ResourcePathConfigServiceFactory
	{
		private const string ResourcePathConfigServicePath = "Config/ScriptableObjects/ResourcesPrefabs";

		private readonly IAssetFactory _assetFactory;

		public ResourcePathConfigServiceFactory(IAssetFactory assetFactory) => _assetFactory
			= assetFactory ?? throw new ArgumentNullException(nameof(assetFactory));

		public ResourcesPrefabs Create() =>
			_assetFactory.LoadFromResources<ResourcesPrefabs>(ResourcePathConfigServicePath);
	}
}
