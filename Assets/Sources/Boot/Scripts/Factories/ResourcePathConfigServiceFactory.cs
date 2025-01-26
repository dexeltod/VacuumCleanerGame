using System;
using Sources.BusinessLogic.ServicesInterfaces;
using Sources.Infrastructure.Services;

namespace Sources.Boot.Scripts.Factories
{
	public sealed class ResourcePathConfigServiceFactory
	{
		private const string ResourcePathConfigServicePath = "Config/ScriptableObjects/ResourcesPrefabs";

		private readonly IAssetLoader _assetLoader;

		public ResourcePathConfigServiceFactory(IAssetLoader assetLoader) =>
			_assetLoader
				= assetLoader ?? throw new ArgumentNullException(nameof(assetLoader));

		public ResourcesPrefabs Create() => _assetLoader.LoadFromResources<ResourcesPrefabs>(ResourcePathConfigServicePath);
	}
}