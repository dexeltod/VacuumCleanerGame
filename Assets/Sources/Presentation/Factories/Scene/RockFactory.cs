using System;
using System.Collections.Generic;
using Sources.BuisenessLogic.Interfaces;
using Sources.BuisenessLogic.ServicesInterfaces;
using Sources.DomainInterfaces;
using Sources.Infrastructure.Services;
using Sources.Presentation.SceneEntity;
using Sources.Presentation.Services;
using Sources.Utils;
using Sources.Utils.ParticleColorChanger.Scripts;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Sources.Presentation.Factories.Scene
{
	public class RockFactory
	{
		private const float Offset = 0.2f;

		private readonly IAssetFactory _assetFactory;
		private readonly ILevelProgressFacade _levelProgressFacade;
		private readonly ResourcesPrefabs _resourcesProgressPresenterProvider;
		private readonly ILevelConfigGetter _levelConfigGetter;

		public RockFactory(
			IAssetFactory assetFactory,
			ILevelProgressFacade levelProgressFacade,
			ResourcesPrefabs resourcesProgressPresenterProvider,
			ILevelConfigGetter levelConfigGetter
		)
		{
			_assetFactory
				= assetFactory ?? throw new ArgumentNullException(nameof(assetFactory));
			_levelProgressFacade = levelProgressFacade ?? throw new ArgumentNullException(nameof(levelProgressFacade));
			_resourcesProgressPresenterProvider = resourcesProgressPresenterProvider ??
			                                      throw new ArgumentNullException(nameof(resourcesProgressPresenterProvider));
			_levelConfigGetter = levelConfigGetter ?? throw new ArgumentNullException(nameof(levelConfigGetter));
		}

		private string ResourceSpawnPosition => ResourcesAssetPath.GameObjects.ResourceSpawnPosition;

		public void Create()
		{
			int totalResource = 0;

			ILevelConfig levelConfig = _levelConfigGetter.GetOrDefault(_levelProgressFacade.CurrentLevel);

			Debug.Log("rocks" + _levelProgressFacade.MaxTotalResourceCount);

			GameObject rocksObject = new GameObject("RockResource");

			int hardCurrencySpawnIndex = Random.Range(0, _levelProgressFacade.MaxTotalResourceCount);
			int areaSize = Mathf.CeilToInt(Mathf.Sqrt(_levelProgressFacade.MaxTotalResourceCount));

			int hardResourceCount = Mathf.CeilToInt(_levelProgressFacade.MaxTotalResourceCount / 100f);

			var softResources = InitSoftResources(levelConfig);
			var hardVariants = InitHardResources(levelConfig);

			GameObject resourceSpawnPosition = _assetFactory.Instantiate(ResourceSpawnPosition);
			resourceSpawnPosition.transform.SetParent(rocksObject.transform);

			InstantiateResources(
				levelConfig.SoftMinedResource.Count,
				areaSize,
				softResources,
				hardVariants,
				totalResource,
				resourceSpawnPosition,
				levelConfig,
				hardResourceCount,
				hardCurrencySpawnIndex
			);
		}

		private void InstantiateResources(
			int softResourcesVariantsCount,
			int areaSize,
			Dictionary<int, ISoftMinedResource> softVariants,
			Dictionary<int, IHardMinedResource> hardVariants,
			int totalResource,
			GameObject resourceSpawnPosition,
			ILevelConfig levelConfig,
			int hardResourceCount,
			int hardResourceSpawnIndex
		)
		{
			for (int i = 0; i < areaSize; i++)
			{
				for (int j = 0; j < areaSize; j++)
				{
					if (totalResource > _levelProgressFacade.MaxTotalResourceCount)
						return;

					if (totalResource >= hardResourceSpawnIndex && hardResourceCount > 0)
					{
						hardResourceCount = SpawnHardResource(
							hardVariants,
							ref totalResource,
							resourceSpawnPosition,
							levelConfig,
							hardResourceCount,
							i,
							j
						);
					}
					else
					{
						SpawnSoftCurrency(
							softResourcesVariantsCount,
							softVariants,
							ref totalResource,
							resourceSpawnPosition,
							levelConfig,
							i,
							j
						);
					}
				}
			}
		}

		private void SpawnSoftCurrency(
			int softResourcesVariantsCount,
			IReadOnlyDictionary<int, ISoftMinedResource> softVariants,
			ref int totalResource,
			GameObject resourceSpawnPosition,
			ILevelConfig levelConfig,
			int i,
			int j
		)
		{
			int randomRockIndex = Random.Range(0, softResourcesVariantsCount);

			ResourcePresentation resourcePresentation = GetResourcePresentation(levelConfig, randomRockIndex);

			IMinedResource config = softVariants[randomRockIndex];

			totalResource++;

			float offsetPosition = randomRockIndex * Offset;

			Vector3 position = SetTransformPosition(resourceSpawnPosition, i, j, offsetPosition, resourcePresentation);

			ResourcePresentation resourceObject = InstantiateAndSetPosition(resourcePresentation, position);

			SetViewAndData(resourceSpawnPosition, resourceObject, config);
		}

		private ResourcePresentation GetResourcePresentation(ILevelConfig levelConfig, int randomRockIndex) =>
			levelConfig.SoftMinedResource[randomRockIndex].Prefab
				.GetComponent<ResourcePresentation>() ??
			throw new ArgumentNullException($"ResourcePresentation is null");

		private Vector3 SetTransformPosition(
			GameObject resourceSpawnPosition,
			int i,
			int j,
			float offsetPosition,
			ResourcePresentation resourcePresentation
		) =>
			new Vector3(
				i + offsetPosition,
				resourcePresentation.transform.position.y,
				j + offsetPosition
			) +
			resourceSpawnPosition.transform.position;

		private int SpawnHardResource(
			Dictionary<int, IHardMinedResource> hardVariants,
			ref int totalResource,
			GameObject resourceSpawnPosition,
			ILevelConfig levelConfig,
			int hardResourceCount,
			int x,
			int z
		)
		{
			int index = Random.Range(0, hardVariants.Count);
			IHardMinedResource hardConfig = hardVariants[index];

			ResourcePresentation resourcePresentation = levelConfig.HardMinedResource[index].Prefab
				.GetComponent<ResourcePresentation>();

			Vector3 position
				= new Vector3(
					  x + Offset,
					  resourcePresentation.transform.position.y,
					  z + Offset
				  ) +
				  resourceSpawnPosition.transform.position;

			ResourcePresentation hardResource = InstantiateAndSetPosition(resourcePresentation, position);

			hardResourceCount--;
			totalResource++;

			SetViewAndData(resourceSpawnPosition, hardResource, hardConfig);
			AddHardResourceParticle(hardConfig, hardResource);

			return hardResourceCount;
		}

		private void AddHardResourceParticle(IHardMinedResource hardConfig, ResourcePresentation hardResource)
		{
			ParticleSystem particleSystem = Object.Instantiate(
				hardConfig.HardResourceEffect,
				hardResource.transform.position,
				Quaternion.identity
			);

			var activationExtension
				= hardResource.gameObject.AddComponent<ParticleSystemActivationExtension>();

			activationExtension.Construct(particleSystem, hardResource);
			particleSystem.Play();
		}

		private void SetViewAndData(
			GameObject resourceSpawnPosition,
			ResourcePresentation hardResource,
			IMinedResource hardConfig
		)
		{
			SetMaterial(hardResource, hardConfig);
			SetParticleColor(hardResource, hardConfig, hardConfig.Material);
			hardResource.Construct(_resourcesProgressPresenterProvider, hardConfig.Score);
			hardResource.transform.SetParent(resourceSpawnPosition.transform);
		}

		private ResourcePresentation InstantiateAndSetPosition(
			ResourcePresentation resourcePresentation,
			Vector3 rockPosition
		) =>
			Object.Instantiate(
				resourcePresentation,
				rockPosition,
				new Quaternion(GetRandomAngle(), GetRandomAngle(), GetRandomAngle(), 1)
			).GetComponent<ResourcePresentation>();

		private float GetRandomAngle() =>
			Random.Range(0, 360);

		private void SetParticleColor(
			ResourcePresentation resourcePresentation,
			IMinedResource sceneObject,
			Material material
		)
		{
			PS_ColorChanger colorChanger = resourcePresentation.Particle.GetComponent<PS_ColorChanger>();

			ParticleSystem particleSystem = resourcePresentation.Particle.GetComponent<ParticleSystem>();
			ParticleSystemRenderer renderer = particleSystem.GetComponent<ParticleSystemRenderer>();

			renderer.material = new Material(material) { color = sceneObject.Color };

			colorChanger.newColor = sceneObject.Color;
			colorChanger.ChangeColor();
		}

		private void SetMaterial(ResourcePresentation resourceObject, IMinedResource config)
		{
			var renderer = resourceObject.View.GetComponent<MeshRenderer>();

			renderer.material = new Material(config.Material) { color = config.Color };
		}

		private Dictionary<int, ISoftMinedResource> InitSoftResources(ILevelConfig levelConfig)
		{
			var resources = new Dictionary<int, ISoftMinedResource>(levelConfig.SoftMinedResource.Count);

			for (int i = 0; i < levelConfig.SoftMinedResource.Count; i++)
			{
				ISoftMinedResource resource = levelConfig.SoftMinedResource[i];
				resources.Add(i, resource);
			}

			return resources;
		}

		private Dictionary<int, IHardMinedResource> InitHardResources(ILevelConfig levelConfig)
		{
			var resources = new Dictionary<int, IHardMinedResource>(levelConfig.HardMinedResource.Count);

			for (int i = 0; i < levelConfig.HardMinedResource.Count; i++)
			{
				IHardMinedResource resource = levelConfig.HardMinedResource[i];
				resources.Add(i, resource);
			}

			return resources;
		}
	}
}