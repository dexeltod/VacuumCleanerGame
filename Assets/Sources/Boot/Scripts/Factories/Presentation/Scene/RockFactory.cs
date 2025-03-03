using System;
using System.Collections.Generic;
using System.Linq;
using Sources.BusinessLogic.Interfaces;
using Sources.BusinessLogic.Repository;
using Sources.BusinessLogic.ServicesInterfaces;
using Sources.Domain.Entities;
using Sources.InfrastructureInterfaces.Configs.Scripts.Level;
using Sources.Presentation.SceneEntity;
using Sources.PresentationInterfaces.Common;
using Sources.Utils;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Sources.Boot.Scripts.Factories.Presentation.Scene
{
	public class RockFactory
	{
		private const float Offset = 0.2f;

		private readonly IAssetLoader _assetLoader;
		private readonly ILevelConfigGetter _levelConfigGetter;
		private readonly ILevelProgressFacade _levelProgressFacade;
		private readonly ICollection<IResourcePresentation> _minedResources = new List<IResourcePresentation>();
		private readonly SceneResourcesRepository _sceneResourcesRepository;

		private int _currentResourceId;
		private ParticleSystem _hardEffect;

		public RockFactory(
			IAssetLoader assetLoader,
			ILevelProgressFacade levelProgressFacade,
			ILevelConfigGetter levelConfigGetter,
			SceneResourcesRepository newSceneResourcesRepository)
		{
			_assetLoader
				= assetLoader ?? throw new ArgumentNullException(nameof(assetLoader));
			_levelProgressFacade = levelProgressFacade ?? throw new ArgumentNullException(nameof(levelProgressFacade));
			_levelConfigGetter = levelConfigGetter ?? throw new ArgumentNullException(nameof(levelConfigGetter));
			_sceneResourcesRepository = newSceneResourcesRepository
			                            ?? throw new ArgumentNullException(nameof(newSceneResourcesRepository));
		}

		private string ResourceSpawnPosition => ResourcesAssetPath.GameObjects.ResourceSpawnPosition;

		public Dictionary<int, IResourcePresentation> Create()
		{
			Debug.Log("rocks" + _levelProgressFacade.MaxTotalResourceCount);

			_currentResourceId = 0;

			LevelConfig levelConfig = _levelConfigGetter.GetOrDefault(_levelProgressFacade.CurrentLevel);
			_hardEffect = _levelConfigGetter.GetOrDefault().HardEffectParticle;

			var resourceSceneObject = new GameObject("resource - ");

			// Dictionary<int, ResourcesConfig> hardVariants = InitHardResource(levelConfig);

			GameObject resourceSpawnPosition = _assetLoader.Instantiate(ResourceSpawnPosition);
			resourceSpawnPosition.transform.SetParent(resourceSceneObject.transform);

			InstantiateResources(
				levelConfig.PrefabsCount,
				Mathf.CeilToInt(Mathf.Sqrt(_levelProgressFacade.MaxTotalResourceCount)),
				InitSoftResources(levelConfig),
				resourceSpawnPosition,
				levelConfig,
				Mathf.CeilToInt(_levelProgressFacade.MaxTotalResourceCount / 100f),
				Random.Range(0, _levelProgressFacade.MaxTotalResourceCount)
			);

			return _minedResources.ToDictionary(elem => elem.ID, elem => elem);
		}

		private void AddHardResourceParticle(ResourcePresentation hardResource)
		{
			ParticleSystem particleSystem = Object.Instantiate(_hardEffect, hardResource.transform.position, Quaternion.identity);

			var activationExtension = hardResource.gameObject.AddComponent<ParticleSystemActivationExtension>();
			activationExtension.Construct(particleSystem, hardResource);

			particleSystem.Play();
		}

		private float GetRandomAngle() => Random.Range(0, 360);

		private GameObject GetResourcePresentation(LevelConfig levelConfig, int resource) =>
			levelConfig.ResourcesConfig.Prefabs[resource]
			?? throw new ArgumentNullException("ResourcePresentation is null");

		private IReadOnlyCollection<ResourcesConfig> InitHardResource(LevelConfig levelConfig)
		{
			var hardVariants = new List<GameObject>();

			for (var i = 0; i < levelConfig.HardResourceCount; i++)
				hardVariants.Add(levelConfig.ResourcesConfig.Prefabs[Random.Range(0, levelConfig.PrefabsCount)]);

			return hardVariants;
		}

		private Dictionary<int, ResourcesConfig> InitSoftResources(LevelConfig levelConfig)
		{
			var resourcesConfigs = new Dictionary<int, ResourcesConfig>(levelConfig.ResourcesConfig.Prefabs.Count);

			for (var i = 0; i < levelConfig.ResourcesConfig.Prefabs.Count; i++)
			{
				GameObject resources = levelConfig.ResourcesConfig.Prefabs[i];
				resourcesConfigs.Add(i, resources);
			}

			return resourcesConfigs;
		}

		private ResourcePresentation InstantiateAndSetPosition(
			GameObject resourcePresentation,
			Vector3 rockPosition
		) =>
			_assetLoader.InstantiateAndGetComponent<ResourcePresentation>(
				resourcePresentation,
				rockPosition,
				new Quaternion(GetRandomAngle(), GetRandomAngle(), GetRandomAngle(), 1)
			);

		private void InstantiateResources(
			int softResourcesVariantsCount,
			int areaSize,
			Dictionary<int, ResourcesConfig> softVariants,
			GameObject resourceSpawnPosition,
			LevelConfig levelConfig,
			int hardResourceCount,
			int hardResourceSpawnIndex
		)
		{
			for (var i = 0; i < areaSize; i++)
				for (var j = 0; j < areaSize; j++)
				{
					if (_currentResourceId > _levelProgressFacade.MaxTotalResourceCount)
						break;

					if (_currentResourceId >= hardResourceSpawnIndex && hardResourceCount > 0)
					{
						AddHardResourceParticle();

						hardResourceCount = SpawnHardResource(
							hardVariants,
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
							resourceSpawnPosition,
							levelConfig,
							i,
							j
						);
					}
				}
		}

		private void SetMaterial(ResourcePresentation resourceObject, ResourcesConfig config)
		{
			var renderer = resourceObject.View.GetComponent<MeshRenderer>();

			renderer.material = new Material(config.Material) { color = config.Color };
		}

		private Vector3 SetTransformPosition(
			GameObject resourceSpawnPosition,
			int i,
			int j,
			float offsetPosition,
			GameObject resourcePresentation) =>
			new Vector3(i + offsetPosition, resourcePresentation.transform.position.y, j + offsetPosition)
			+ resourceSpawnPosition.transform.position;

		private void SetViewAndData(
			GameObject resourceSpawnPosition,
			ResourcePresentation resource,
			ResourcesConfig resourcesConfig
		)
		{
			SetMaterial(resource, resourcesConfig);

			_sceneResourcesRepository.Add(new SceneResourceEntity(_currentResourceId, resourcesConfig.Score));
			resource.Construct(_currentResourceId, resourcesConfig.Material, resourcesConfig.Material.color);
			resource.transform.SetParent(resourceSpawnPosition.transform);
		}

		private void SpawnSoftCurrency(
			int softResourcesVariantsCount,
			IReadOnlyCollection<ResourcesConfig> softVariants,
			GameObject resourceSpawnPosition,
			LevelConfig levelConfig,
			int i,
			int j
		)
		{
			int randomRockIndex = Random.Range(0, softResourcesVariantsCount);

			GameObject resourcePresentation = GetResourcePresentation(levelConfig, randomRockIndex);

			_currentResourceId++;

			ResourcePresentation resourceObject = InstantiateAndSetPosition(
				resourcePresentation,
				SetTransformPosition(resourceSpawnPosition, i, j, randomRockIndex * Offset, resourcePresentation)
			);

			SetViewAndData(resourceSpawnPosition, resourceObject, softVariants.ElementAt(randomRockIndex));

			_minedResources.Add(resourceObject);
		}

		// private int SpawnHardResource(

		// 	Dictionary<int, IHardMinedResource> hardVariants,

		// 	GameObject resourceSpawnPosition,

		// 	LevelConfig levelConfig,

		// 	int hardResourceCount,

		// 	int x,

		// 	int z

		// )

		// {

		// 	int index = Random.Range(0, hardVariants.Count);

		// 	IHardMinedResource hardConfig = hardVariants[index];

		//

		// 	var resourcePresentation = levelConfig

		// 		.HardMinedResource[index]

		// 		.Prefab

		// 		.GetComponent<ResourcePresentation>();

		//

		// 	Vector3 position

		// 		= new Vector3(x + Offset, resourcePresentation.transform.position.y, z + Offset)

		// 		  + resourceSpawnPosition.transform.position;

		//

		// 	ResourcePresentation hardResource = InstantiateAndSetPosition(resourcePresentation, position);

		//

		// 	hardResourceCount--;

		// 	_totalResource++;

		//

		// 	SetViewAndData(resourceSpawnPosition, hardResource, hardConfig);

		// 	AddHardResourceParticle(hardConfig, hardResource);

		//

		// 	_minedResources.Add(hardResource);

		//

		// 	return hardResourceCount;

		// }
	}
}
