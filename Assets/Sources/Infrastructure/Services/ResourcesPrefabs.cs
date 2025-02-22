using System;
using Sources.BusinessLogic.Interfaces.Configs;
using UnityEngine;

namespace Sources.Infrastructure.Services
{
	[CreateAssetMenu(fileName = "ResourcesPrefabs", menuName = "ViewConfigs/ResourcesPrefabs")]
	public class ResourcesPrefabs : ScriptableObject, IResourcesPrefabs
	{
		[SerializeField] private SceneGameObjects _sceneGameObjects;
		[SerializeField] private Triggers _triggers;

		public ISceneGameObjects SceneGameObjects => _sceneGameObjects;
		public ITriggers Triggers => _triggers;
	}

	[Serializable]
	public class Triggers : ITriggers
	{
		[SerializeField] private GameObject _sellTrigger;

		public GameObject SellTrigger => _sellTrigger;
	}

	[Serializable]
	public class SceneGameObjects : ISceneGameObjects
	{
		[SerializeField] private GameObject _spawnPoint;
		[SerializeField] private GameObject _sandContainer;
		[SerializeField] private GameObject _virtualCamera;
		[SerializeField] private GameObject _mainCamera;
		[SerializeField] private GameObject _upgradeTrigger;
		[SerializeField] private GameObject _sandGround;
		[SerializeField] private GameObject _cameraConfiner;

		public GameObject SpawnPoint => _spawnPoint;
		public GameObject SandContainer => _sandContainer;
		public GameObject VirtualCamera => _virtualCamera;
		public GameObject MainCamera => _mainCamera;
		public GameObject UpgradeTrigger => _upgradeTrigger;
		public GameObject SandGround => _sandGround;
		public GameObject CameraConfiner => _cameraConfiner;
	}
}