namespace Sources.Infrastructure
{
	public class ResourcePathConfigService
	{
		private readonly ResourcesPrefabs _resourcesPrefabs;

		public ResourcesPrefabs ResourcesPrefabs => _resourcesPrefabs;

		public ResourcePathConfigService(ResourcesPrefabs resourcesPrefabs) =>
			_resourcesPrefabs = resourcesPrefabs;
	}
}