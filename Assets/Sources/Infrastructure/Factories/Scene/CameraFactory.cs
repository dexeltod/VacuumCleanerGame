using System;
using Cinemachine;
using Sources.Infrastructure.Factories.Player;
using Sources.InfrastructureInterfaces.Factory;
using Sources.InfrastructureInterfaces.Scene;
using Sources.ServicesInterfaces;
using Sources.Utils.Configs.Scripts;
using Sources.Utils.ConstantNames;
using UnityEngine;

namespace Sources.Infrastructure.Factories.Scene
{
	public class CameraConfinerFactory
	{
		private readonly IAssetFactory _assetFactory;
	}

	public class CameraFactory : ICameraFactory
	{
		private const string MainCameraPath = "MainCamera";
		private const string VirtualCameraPath = "VirtualCamera";

		private readonly IAssetFactory _assetFactory;
		private readonly IPlayerFactory _playerFactory;
		private readonly ResourcePathConfigProvider _assetPathConfigProvider;

		private GameObject _characterObject;
		public Camera Camera { get; private set; }

		private GameObject CustomCameraConfiner =>
			_assetPathConfigProvider.Implementation.SceneGameObjects.CameraConfiner;

		private GameObject MainCamera => _assetPathConfigProvider.Implementation.SceneGameObjects.MainCamera;
		private GameObject VirtualCamera => _assetPathConfigProvider.Implementation.SceneGameObjects.VirtualCamera;

		public CameraFactory(
			IAssetFactory assetFactory,
			IPlayerFactory playerFactory,
			ResourcePathConfigProvider assetPathConfigProvider
		)
		{
			_assetFactory = assetFactory ?? throw new ArgumentNullException(nameof(assetFactory));
			_playerFactory = playerFactory ?? throw new ArgumentNullException(nameof(playerFactory));
			_assetPathConfigProvider = assetPathConfigProvider ??
				throw new ArgumentNullException(nameof(assetPathConfigProvider));
		}

		public CinemachineVirtualCamera CreateVirtualCamera()
		{
			_characterObject = _playerFactory.Player;
			Camera = _assetFactory.InstantiateAndGetComponent<Camera>(MainCamera);

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