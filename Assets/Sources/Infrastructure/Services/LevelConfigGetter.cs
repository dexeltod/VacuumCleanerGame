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
		private readonly IAssetFactory _assetFactory;
		private LevelsConfig _levelConfigs;

		[Inject]
		public LevelConfigGetter(IAssetFactory assetFactory)
		{
			_assetFactory = assetFactory ?? throw new ArgumentNullException(nameof(assetFactory));
		}

		public ILevelConfig GetOrDefault(int levelNumber)
		{
			if (_levelConfigs == null)
				_levelConfigs = _assetFactory.LoadFromResources<LevelsConfig>(ResourcesAssetPath.Configs.LevelsConfig);

			return _levelConfigs.GetOrDefault(levelNumber);
		}
	}
}
