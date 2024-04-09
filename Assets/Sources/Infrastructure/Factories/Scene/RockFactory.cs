using System;
using System.Collections.Generic;
using System.Linq;
using Sources.Infrastructure.Configs.Scripts;
using Sources.Infrastructure.Configs.Scripts.Level;
using Sources.Infrastructure.Configs.Scripts.Level.LevelResouce;
using Sources.Infrastructure.Providers;
using Sources.Presentation.SceneEntity;
using Sources.ServicesInterfaces;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Sources.Infrastructure.Factories.Scene
{
	public class RockFactory
	{
		private const int RocksVariants = 4;
		private const float Offset = 0.2f;

		private readonly IAssetFactory _assetFactory;
		private readonly ILevelProgressFacade _levelProgressFacade;
		private readonly ResourcesProgressPresenterProvider _resourcesProgressPresenterProvider;
		private readonly ILevelConfigGetter _levelConfigGetter;

		public RockFactory(
			IAssetFactory assetFactory,
			ILevelProgressFacade levelProgressFacade,
			ResourcesProgressPresenterProvider resourcesProgressPresenterProvider,
			ILevelConfigGetter levelConfigGetter
		)
		{
			_assetFactory = assetFactory ?? throw new ArgumentNullException(nameof(assetFactory));
			_levelProgressFacade = levelProgressFacade ?? throw new ArgumentNullException(nameof(levelProgressFacade));
			_resourcesProgressPresenterProvider = resourcesProgressPresenterProvider ??
				throw new ArgumentNullException(nameof(resourcesProgressPresenterProvider));
			_levelConfigGetter = levelConfigGetter ?? throw new ArgumentNullException(nameof(levelConfigGetter));
		}

		private string ResourceSpawnPosition => ResourcesAssetPath.GameObjects.ResourceSpawnPosition;
		private string ResourceRock => ResourcesAssetPath.GameObjects.ResourceRock;

		public void Create()
		{
			var levelConfig = _levelConfigGetter.GetOrDefault(_levelProgressFacade.CurrentLevel);

			GameObject gameObject = new("Rocks");
			int totalScore = 0;

			int rocksCount = Mathf.CeilToInt(Mathf.Sqrt(_levelProgressFacade.MaxScoreCount));

			Dictionary<int, ISoftMinedResource> rocksVariants = InitSoftResources(levelConfig);

			GameObject resourceSpawnPosition = _assetFactory.Instantiate(ResourceSpawnPosition);
			resourceSpawnPosition.transform.SetParent(gameObject.transform);

			Instantiate(
				levelConfig.SoftMinedResource.Count,
				rocksCount,
				rocksVariants,
				totalScore,
				resourceSpawnPosition,
				levelConfig
			);
		}

		private void Instantiate(
			int softResourcesVariantsCount,
			int rocksCount,
			Dictionary<int, ISoftMinedResource> rocksVariants,
			int totalScore,
			GameObject resourceSpawnPosition,
			ILevelConfig levelConfig
		)
		{
			for (int i = 0; i < rocksCount; i++)
			{
				for (int j = 0; j < rocksCount; j++)
				{
					int randomRockIndex = Random.Range(0, softResourcesVariantsCount);
					var resourcePresentation = levelConfig.SoftMinedResource[randomRockIndex].Prefab
						.GetComponent<ResourcePresentation>();

					var config = rocksVariants.ElementAt(randomRockIndex).Value;

					if (totalScore > _levelProgressFacade.MaxScoreCount)
						return;

					float offsetPosition = randomRockIndex * Offset;

					Vector3 rockPosition = new Vector3(
						i + offsetPosition,
						resourcePresentation.transform.position.y,
						j + offsetPosition
					) + resourceSpawnPosition.transform.position;

					ResourcePresentation resourceObject = Object.Instantiate(
						resourcePresentation,
						rockPosition,
						Quaternion.identity
					).GetComponent<ResourcePresentation>();

					SetMaterial(resourceObject, config);
					resourceObject.Construct(_resourcesProgressPresenterProvider, config.Score);
					resourceObject.transform.SetParent(resourceSpawnPosition.transform);
				}
			}
		}

		private void SetMaterial(ResourcePresentation resourceObject, ISoftMinedResource config)
		{
			Material material = GetMaterial(resourceObject);

			material.color = config.Color;
		}

		private Material GetMaterial(ResourcePresentation resourceObject) =>
			resourceObject.View.GetComponent<MeshRenderer>().material;

		private Dictionary<int, ISoftMinedResource> InitSoftResources(
			ILevelConfig levelConfig
		)
		{
			var rocksVariants = new Dictionary<int, ISoftMinedResource>(RocksVariants);

			for (int i = 0; i < levelConfig.SoftMinedResource.Count; i++)
			{
				ISoftMinedResource resource = levelConfig.SoftMinedResource[i];
				rocksVariants.Add(i, resource);
			}

			return rocksVariants;
		}
	}
}