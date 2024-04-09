using Sources.Infrastructure.Configs.Scripts;
using Sources.Infrastructure.Configs.Scripts.Level;
using Sources.ServicesInterfaces;

namespace Sources.InfrastructureInterfaces.Scene
{
	public sealed class LevelConfigGetter : ILevelConfigGetter
	{
		private readonly LevelsConfig _levelConfigs;

		public LevelConfigGetter(IAssetFactory assetFactory) =>
			_levelConfigs = assetFactory.LoadFromResources<LevelsConfig>(ResourcesAssetPath.Configs.LevelsConfig);

		public ILevelConfig GetOrDefault(int levelNumber) =>
			_levelConfigs.GetOrDefault(levelNumber);
	}
}