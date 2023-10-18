using Sources.DIService;
using Sources.ServicesInterfaces;
using Sources.Utils.Configs;

namespace Sources.InfrastructureInterfaces.Scene
{
	public class LevelConfigGetter : ILevelConfigGetter
	{
		private readonly IAssetProvider          _assetProvider;
		private readonly ILevelProgressPresenter _levelProgressPresenter;
		private          string                  _sceneName;
		private          LevelsConfig            _levelConfigs;

		public LevelConfigGetter(IAssetProvider assetProvider, ILevelProgressPresenter levelProgressPresenter)
		{
			_assetProvider          = assetProvider;
			_levelProgressPresenter = levelProgressPresenter;
			_assetProvider.Load<LevelsConfig>(ResourcesAssetPath.Configs.LevelsConfig);
		}

		public LevelConfig GetCurrentLevel() =>
			_levelConfigs.Get(_levelProgressPresenter.CurrentLevelNumber);

		public LevelConfig Get(int levelNumber) =>
			_levelConfigs.Get(levelNumber);
	}
}