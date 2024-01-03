using Sources.ServicesInterfaces;
using Sources.Utils.Configs;
using Sources.Utils.Configs.Scripts;

namespace Sources.InfrastructureInterfaces.Scene
{
	public class LevelConfigGetter : ILevelConfigGetter
	{
		private readonly LevelsConfig _levelConfigs;

		public LevelConfigGetter(IAssetProvider assetProvider) =>
			_levelConfigs = assetProvider.LoadFromResources<LevelsConfig>(ResourcesAssetPath.Configs.LevelsConfig);

		public LevelConfig Get(int levelNumber) =>
			_levelConfigs.Get(levelNumber);
	}
}