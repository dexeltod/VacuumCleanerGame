using System;
using System.Collections.Generic;
using System.Linq;
using Sources.BusinessLogic.Interfaces;
using Sources.BusinessLogic.Repository;
using Sources.BusinessLogic.ServicesInterfaces;
using Sources.Domain.Entities;
using Sources.DomainInterfaces;
using Sources.Presentation.SceneEntity;
using Sources.PresentationInterfaces;
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
		private int _totalResource;

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
			_totalResource = 0;

			ILevelConfig levelConfig = _levelConfigGetter.GetOrDefault(_levelProgressFacade.CurrentLevel);

			Debug.Log("rocks" + _levelProgressFacade.MaxTotalResourceCount);

			var rocksObject = new GameObject("RockResource");

			int hardCurrencySpawnIndex = Random.Range(0, _levelProgressFacade.MaxTotalResourceCount);
			int areaSize = Mathf.CeilToInt(Mathf.Sqrt(_levelProgressFacade.MaxTotalResourceCount));

			int hardResourceCount = Mathf.CeilToInt(_levelProgressFacade.MaxTotalResourceCount / 100f);

			Dictionary<int, ISoftMinedResource> softResources = InitSoftResources(levelConfig);
			Dictionary<int, IHardMinedResource> hardVariants = InitHardResources(levelConfig);

			GameObject resourceSpawnPosition = _assetLoader.Instantiate(ResourceSpawnPosition);
			resourceSpawnPosition.transform.SetParent(rocksObject.transform);

			InstantiateResources(
				levelConfig.SoftMinedResource.Count,
				areaSize,
				softResources,
				hardVariants,
				resourceSpawnPosition,
				levelConfig,
				hardResourceCount,
				hardCurrencySpawnIndex
			);

			return _minedResources.ToDictionary(elem => elem.ID, elem => elem);
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

		private float GetRandomAngle() => Random.Range(0, 360);

		private ResourcePresentation GetResourcePresentation(ILevelConfig levelConfig, int randomRockIndex) =>
			levelConfig
				.SoftMinedResource[randomRockIndex]
				.Prefab.GetComponent<ResourcePresentation>()
			?? throw new ArgumentNullException("ResourcePresentation is null");

		private Dictionary<int, IHardMinedResource> InitHardResources(ILevelConfig levelConfig)
		{
			var resources = new Dictionary<int, IHardMinedResource>(levelConfig.HardMinedResource.Count);

			for (var i = 0; i < levelConfig.HardMinedResource.Count; i++)
			{
				IHardMinedResource resource = levelConfig.HardMinedResource[i];
				resources.Add(i, resource);
			}

			return resources;
		}

		private Dictionary<int, ISoftMinedResource> InitSoftResources(ILevelConfig levelConfig)
		{
			var resources = new Dictionary<int, ISoftMinedResource>(levelConfig.SoftMinedResource.Count);

			for (var i = 0; i < levelConfig.SoftMinedResource.Count; i++)
			{
				ISoftMinedResource resource = levelConfig.SoftMinedResource[i];
				resources.Add(i, resource);
			}

			return resources;
		}

		private ResourcePresentation InstantiateAndSetPosition(
			ResourcePresentation resourcePresentation,
			Vector3 rockPosition
		) =>
			Object
				.Instantiate(
					resourcePresentation,
					rockPosition,
					new Quaternion(GetRandomAngle(), GetRandomAngle(), GetRandomAngle(), 1)
				)
				.GetComponent<ResourcePresentation>();

		private void InstantiateResources(
			int softResourcesVariantsCount,
			int areaSize,
			Dictionary<int, ISoftMinedResource> softVariants,
			Dictionary<int, IHardMinedResource> hardVariants,
			GameObject resourceSpawnPosition,
			ILevelConfig levelConfig,
			int hardResourceCount,
			int hardResourceSpawnIndex
		)
		{
			for (var i = 0; i < areaSize; i++)
				for (var j = 0; j < areaSize; j++)
				{
					if (_totalResource > _levelProgressFacade.MaxTotalResourceCount)
						break;

					if (_totalResource >= hardResourceSpawnIndex && hardResourceCount > 0)
						hardResourceCount = SpawnHardResource(
							hardVariants,
							resourceSpawnPosition,
							levelConfig,
							hardResourceCount,
							i,
							j
						);
					else
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

		private void SetMaterial(ResourcePresentation resourceObject, IMinedResource config)
		{
			var renderer = resourceObject.View.GetComponent<MeshRenderer>();

			renderer.material = new Material(config.Material) { color = config.Color };
		}

		private Vector3 SetTransformPosition(
			GameObject resourceSpawnPosition,
			int i,
			int j,
			float offsetPosition,
			ResourcePresentation resourcePresentation) =>
			new Vector3(i + offsetPosition, resourcePresentation.transform.position.y, j + offsetPosition)
			+ resourceSpawnPosition.transform.position;

		private void SetViewAndData(
			GameObject resourceSpawnPosition,
			ResourcePresentation resource,
			IMinedResource resourceConfig
		)
		{
			SetMaterial(resource, resourceConfig);

			_sceneResourcesRepository.Add(new SceneResourceEntity(_totalResource, resourceConfig.Score));
			resource.Construct(_totalResource, resourceConfig.Material, resourceConfig.Material.color);
			resource.transform.SetParent(resourceSpawnPosition.transform);
		}

		private int SpawnHardResource(
			Dictionary<int, IHardMinedResource> hardVariants,
			GameObject resourceSpawnPosition,
			ILevelConfig levelConfig,
			int hardResourceCount,
			int x,
			int z
		)
		{
			int index = Random.Range(0, hardVariants.Count);
			IHardMinedResource hardConfig = hardVariants[index];

			var resourcePresentation = levelConfig
				.HardMinedResource[index]
				.Prefab
				.GetComponent<ResourcePresentation>();

			Vector3 position
				= new Vector3(x + Offset, resourcePresentation.transform.position.y, z + Offset)
				  + resourceSpawnPosition.transform.position;

			ResourcePresentation hardResource = InstantiateAndSetPosition(resourcePresentation, position);

			hardResourceCount--;
			_totalResource++;

			SetViewAndData(resourceSpawnPosition, hardResource, hardConfig);
			AddHardResourceParticle(hardConfig, hardResource);

			_minedResources.Add(hardResource);

			return hardResourceCount;
		}

		private void SpawnSoftCurrency(
			int softResourcesVariantsCount,
			IReadOnlyDictionary<int, ISoftMinedResource> softVariants,
			GameObject resourceSpawnPosition,
			ILevelConfig levelConfig,
			int i,
			int j
		)
		{
			int randomRockIndex = Random.Range(0, softResourcesVariantsCount);

			ResourcePresentation resourcePresentation = GetResourcePresentation(levelConfig, randomRockIndex);

			IMinedResource config = softVariants[randomRockIndex];

			_totalResource++;

			float offsetPosition = randomRockIndex * Offset;

			Vector3 position = SetTransformPosition(resourceSpawnPosition, i, j, offsetPosition, resourcePresentation);

			ResourcePresentation resourceObject = InstantiateAndSetPosition(resourcePresentation, position);

			SetViewAndData(resourceSpawnPosition, resourceObject, config);

			_minedResources.Add(resourceObject);
		}
	}

	public class ParticleSystemActivationExtension : MonoBehaviour
	{
		private ICollideable _collideable;
		private ParticleSystem _particleSystem;

		private void OnDisable() => _collideable.Collided -= OnCollided;

		private void OnDestroy() => _collideable.Collided -= OnCollided;

		public void Construct(ParticleSystem particles, ICollideable collideable)
		{
			_collideable = collideable ?? throw new ArgumentNullException(nameof(collideable));
			_particleSystem = particles ? particles : throw new ArgumentNullException(nameof(particles));
			_collideable.Collided += OnCollided;
		}

		private void OnCollided(int value) => _particleSystem.Stop();
	}
}