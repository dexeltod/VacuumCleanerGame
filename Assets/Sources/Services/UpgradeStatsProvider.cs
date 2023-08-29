using Sources.Application.Utils.Configs;
using Sources.Domain;
using Sources.ServicesInterfaces;
using UnityEngine;

namespace Sources.Services
{
	public class UpgradeStatsProvider : IUpgradeStatsProvider
	{
		private readonly IAssetProvider _provider;
		private StatsConfig _statsConfig;

		public UpgradeStatsProvider(IAssetProvider provider) =>
			_provider = provider;

		public StatsConfig LoadConfig()
		{
			if (_statsConfig != null)
				return _statsConfig;

			string json = _provider.Load<TextAsset>(ResourcesAssetPath.Configs.ProgressItems).text;
			_statsConfig = JsonUtility.FromJson<StatsConfig>(json);
			
			return _statsConfig;
		}
	}
}