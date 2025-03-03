using System;
using Sources.BusinessLogic.Interfaces;
using Sources.BusinessLogic.ServicesInterfaces;
using Sources.DomainInterfaces;
using Sources.InfrastructureInterfaces.Configs.Scripts.Level;
using Sources.Utils;
using VContainer;

namespace Sources.Infrastructure.Services
{
	public sealed class LevelConfigGetter : ILevelConfigGetter
	{
		private readonly IAssetLoader _assetLoader;
		private LevelsConfig _levelConfigs;

		[Inject]
		public LevelConfigGetter(IAssetLoader assetLoader) =>
			_assetLoader = assetLoader ?? throw new ArgumentNullException(nameof(assetLoader));

		public LevelConfig GetOrDefault(int levelNumber)
		{
			if (_levelConfigs == null)
				_levelConfigs = _assetLoader.LoadFromResources<LevelsConfig>(ResourcesAssetPath.Configs.LevelsConfig);

			return _levelConfigs.GetOrDefault(levelNumber);
		}
	}
}
