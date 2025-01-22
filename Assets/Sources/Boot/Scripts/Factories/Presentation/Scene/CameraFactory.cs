using System;
using Cinemachine;
using Sources.BusinessLogic.Interfaces.Configs;
using Sources.BusinessLogic.Scene;
using Sources.BusinessLogic.ServicesInterfaces;
using UnityEngine;

namespace Sources.Boot.Scripts.Factories.Presentation.Scene
{
	public class CameraFactory : ICameraFactory
	{
		private readonly IAssetLoader _assetLoader;
		private readonly IResourcesPrefabs _assetPathNameConfigProvider;
		private readonly GameObject _player;

		public CameraFactory(
			IAssetLoader assetLoader,
			GameObject playerFactory,
			IResourcesPrefabs assetPathNameConfigProvider
		)
		{
			_assetLoader
				= assetLoader ?? throw new ArgumentNullException(nameof(assetLoader));
			_player = playerFactory ? playerFactory : throw new ArgumentNullException(nameof(playerFactory));
			_assetPathNameConfigProvider = assetPathNameConfigProvider ??
			                               throw new ArgumentNullException(nameof(assetPathNameConfigProvider));
		}

		private GameObject CustomCameraConfiner => _assetPathNameConfigProvider.SceneGameObjects.CameraConfiner;

		private GameObject MainCamera => _assetPathNameConfigProvider.SceneGameObjects.MainCamera;

		private GameObject VirtualCamera => _assetPathNameConfigProvider.SceneGameObjects.VirtualCamera;

		public CinemachineVirtualCamera Create()
		{
			_assetLoader.InstantiateAndGetComponent<Camera>(MainCamera);

			return GetVirtualCamera();
		}

		private CinemachineVirtualCamera GetVirtualCamera()
		{
			var virtualCamera
				= _assetLoader.Instantiate(VirtualCamera).GetComponent<CinemachineVirtualCamera>();

			var bounds = _assetLoader.Instantiate(CustomCameraConfiner).GetComponent<Collider>();
			virtualCamera.GetComponent<CinemachineConfiner>().m_BoundingVolume = bounds;
			virtualCamera.Follow = _player.transform;

			return virtualCamera;
		}
	}
}