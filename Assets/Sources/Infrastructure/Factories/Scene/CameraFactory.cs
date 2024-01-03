using Cinemachine;
using Sources.DIService;
using Sources.Infrastructure.Factories.Player;
using Sources.InfrastructureInterfaces.Scene;
using Sources.ServicesInterfaces;
using Sources.Utils.Configs;
using Sources.Utils.Configs.Scripts;
using UnityEngine;

namespace Sources.Infrastructure.Factories
{
	public class CameraFactory : ICameraFactory
	{
		private const string MainCameraPath = "MainCamera";
		private const string VirtualCameraPath = "VirtualCamera";

		private readonly IAssetProvider _assetProvider;
		private readonly IPlayerFactory _playerFactory;

		private GameObject _characterObject;
		public Camera Camera { get; private set; }

		public CameraFactory(IAssetProvider assetProvider, IPlayerFactory playerFactory)
		{
			_assetProvider = assetProvider;
			_playerFactory = playerFactory;
		}

		public CinemachineVirtualCamera CreateVirtualCamera()
		{
			_characterObject = _playerFactory.Player;
			Camera = _assetProvider.InstantiateAndGetComponent<Camera>(ResourcesAssetPath.Scene.MainCamera);

			return GetVirtualCamera();
		}

		private CinemachineVirtualCamera GetVirtualCamera()
		{
			Collider bounds = GameObject.FindWithTag(ConstantNames.Confiner).GetComponent<Collider>();

			GameObject camera = _assetProvider.Instantiate(ResourcesAssetPath.Scene.CinemachineVirtualCamera);
			CinemachineConfiner cinemachineConfiner = camera.GetComponent<CinemachineConfiner>();

			CinemachineVirtualCamera virtualCamera = SetCameraBounds(cinemachineConfiner, bounds, camera);

			virtualCamera.Follow = _characterObject.transform;
			return virtualCamera;
		}

		private CinemachineVirtualCamera SetCameraBounds(
			CinemachineConfiner cinemachineConfiner,
			Collider bounds,
			GameObject camera
		)
		{
			cinemachineConfiner.m_BoundingVolume = bounds;
			CinemachineVirtualCamera virtualCamera = camera.GetComponent<CinemachineVirtualCamera>();
			return virtualCamera;
		}
	}
}