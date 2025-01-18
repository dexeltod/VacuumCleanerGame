using System;
using Cinemachine;
using Sources.BusinessLogic.Scene;
using Sources.BusinessLogic.ServicesInterfaces;
using Sources.Infrastructure.Services;
using UnityEngine;

namespace Sources.Presentation.Factories.Scene
{
	public class CameraFactory : ICameraFactory
	{
		private readonly IAssetFactory _assetFactory;
		private readonly GameObject _player;
		private readonly ResourcesPrefabs _assetPathNameConfigProvider;

		private GameObject _characterObject;

		public CameraFactory(
			IAssetFactory assetFactory,
			GameObject playerFactory,
			ResourcesPrefabs assetPathNameConfigProvider
		)
		{
			_assetFactory
				= assetFactory ?? throw new ArgumentNullException(nameof(assetFactory));
			_player = playerFactory ? playerFactory : throw new ArgumentNullException(nameof(playerFactory));
			_assetPathNameConfigProvider = assetPathNameConfigProvider ??
			                               throw new ArgumentNullException(nameof(assetPathNameConfigProvider));
		}

		private GameObject CustomCameraConfiner => _assetPathNameConfigProvider.SceneGameObjects.CameraConfiner;

		private GameObject MainCamera => _assetPathNameConfigProvider.SceneGameObjects.MainCamera;

		private GameObject VirtualCamera => _assetPathNameConfigProvider.SceneGameObjects.VirtualCamera;

		public CinemachineVirtualCamera Create()
		{
			_characterObject = _player;
			_assetFactory.InstantiateAndGetComponent<Camera>(MainCamera);

			return GetVirtualCamera();
		}

		private CinemachineVirtualCamera GetVirtualCamera()
		{
			CinemachineVirtualCamera virtualCamera
				= _assetFactory.Instantiate(VirtualCamera).GetComponent<CinemachineVirtualCamera>();

			Collider bounds = _assetFactory.Instantiate(CustomCameraConfiner).GetComponent<Collider>();
			virtualCamera.GetComponent<CinemachineConfiner>().m_BoundingVolume = bounds;
			virtualCamera.Follow = _characterObject.transform;

			return virtualCamera;
		}
	}
}
