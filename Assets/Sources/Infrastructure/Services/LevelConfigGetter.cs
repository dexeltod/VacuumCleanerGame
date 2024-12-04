using Sources.Infrastructure.Configs.Scripts.Level;
using Sources.InfrastructureInterfaces;
using Sources.InfrastructureInterfaces.Configs;
using Sources.ServicesInterfaces;
using Sources.Utils;

namespace Sources.Infrastructure.Services
{
	public sealed class LevelConfigGetter : ILevelConfigGetter
	{
		private readonly LevelsConfig _levelConfigs;

		public LevelConfigGetter(IAssetFactory assetFactory) =>
			_levelConfigs
				= assetFactory.LoadFromResources<LevelsConfig>(ResourcesAssetPath.Configs.LevelsConfig);

		public ILevelConfig GetOrDefault(int levelNumber) =>
			_levelConfigs.GetOrDefault(levelNumber);
	}
}