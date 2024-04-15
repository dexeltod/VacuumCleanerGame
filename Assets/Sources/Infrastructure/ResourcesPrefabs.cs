using System;
using UnityEngine;

namespace Sources.Infrastructure
{
	[CreateAssetMenu(fileName = "ResourcesPrefabs", menuName = "ViewConfigs/ResourcesPrefabs")]
	public class ResourcesPrefabs : ScriptableObject
	{
		[SerializeField] private SceneGameObjects _sceneGameObjects;
		[SerializeField] private ServicesGameObjects _servicesGameObjects;
		[SerializeField] private Triggers _triggers;

		public SceneGameObjects SceneGameObjects => _sceneGameObjects;
		public ServicesGameObjects ServicesGameObjects => _servicesGameObjects;
		public Triggers Triggers => _triggers;
	}

	[Serializable] public class Triggers
	{
		[SerializeField] private GameObject _sellTrigger;

		public GameObject SellTrigger => _sellTrigger;
	}

	[Serializable] public class SceneGameObjects
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

	[Serializable] public class ServicesGameObjects
	{
		[SerializeField] private GameObject _coroutineRunner;

		[SerializeField] private GameObject _leanLocalization;

		[SerializeField] public GameObject CoroutineRunner => _coroutineRunner;
		[SerializeField] public GameObject LeanLocalization => _leanLocalization;
	}
}