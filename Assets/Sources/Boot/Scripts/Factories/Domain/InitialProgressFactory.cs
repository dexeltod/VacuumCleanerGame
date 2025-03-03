using System;
using Sources.Boot.Scripts.Factories.Repositories;
using Sources.BusinessLogic.Interfaces.Factory;
using Sources.BusinessLogic.ServicesInterfaces;
using Sources.Domain.Progress;
using Sources.Domain.Settings;
using Sources.DomainInterfaces;
using Sources.DomainInterfaces.Implementations;
using Sources.Utils;
using Sources.Utils.AssetPaths;
using UnityEngine;
using VContainer;

namespace Sources.Boot.Scripts.Factories.Domain
{
	public class InitialProgressFactory : IInitialProgressFactory
	{
		private readonly IAssetLoader _assetLoader;

		[Inject]
		public InitialProgressFactory(
			IAssetLoader assetLoader
		) =>
			_assetLoader = assetLoader ?? throw new ArgumentNullException(nameof(assetLoader));

		public IGlobalProgress Create()
		{
			ILevelProgress levelProgress = new LevelProgressFactory(LevelSandConfig.DefaultMaxTotalResource).Create();

			return new GlobalProgress(
				new ResourcesModelFactory(new ResourceRepositoryFactory(_assetLoader, levelProgress).Create()).Create(),
				(LevelProgress)levelProgress,
				new ShopModelFactory(_assetLoader).LoadList(),
				new PlayerModelFactory(_assetLoader, new ShopModelFactory(_assetLoader).LoadList()).Create(),
				new SoundSettings(new EventFloatEntity(PlayerPrefs.GetFloat(SettingsPlayerPrefsNames.MasterVolumeName, -40f)))
			);
		}
	}
}
