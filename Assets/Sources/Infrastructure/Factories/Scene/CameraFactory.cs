using System;
using Cinemachine;
using Sources.Infrastructure.Providers;
using Sources.InfrastructureInterfaces.Scene;
using Sources.ServicesInterfaces;
using UnityEngine;

namespace Sources.Infrastructure.Factories.Scene
{
	public class CameraFactory : ICameraFactory
	{
		private readonly IAssetFactory _assetFactory;
		private readonly GameObject _player;
		private readonly ResourcePathNameConfigProvider _assetPathNameConfigProvider;

		private GameObject _characterObject;

		public CameraFactory(
			IAssetFactory assetFactory,
			GameObject playerFactory,
			ResourcePathNameConfigProvider assetPathNameConfigProvider
		)
		{
			_assetFactory = assetFactory ?? throw new ArgumentNullException(nameof(assetFactory));
			_player = playerFactory ? playerFactory : throw new ArgumentNullException(nameof(playerFactory));
			_assetPathNameConfigProvider = assetPathNameConfigProvider ??
				throw new ArgumentNullException(nameof(assetPathNameConfigProvider));
		}

		private GameObject CustomCameraConfiner => _assetPathNameConfigProvider.Self.SceneGameObjects.CameraConfiner;

		private GameObject MainCamera => _assetPathNameConfigProvider.Self.SceneGameObjects.MainCamera;

		private GameObject VirtualCamera => _assetPathNameConfigProvider.Self.SceneGameObjects.VirtualCamera;

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