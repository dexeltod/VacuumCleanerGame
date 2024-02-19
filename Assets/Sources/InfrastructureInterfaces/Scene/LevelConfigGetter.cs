using Sources.ServicesInterfaces;
using Sources.Utils.Configs.Scripts;

namespace Sources.InfrastructureInterfaces.Scene
{
	public sealed class LevelConfigGetter : ILevelConfigGetter
	{
		private readonly LevelsConfig _levelConfigs;

		public LevelConfigGetter(IAssetFactory assetFactory) =>
			_levelConfigs = assetFactory.LoadFromResources<LevelsConfig>(ResourcesAssetPath.Configs.LevelsConfig);

		public LevelConfig Get(int levelNumber) =>
			_levelConfigs.Get(levelNumber);
	}
}