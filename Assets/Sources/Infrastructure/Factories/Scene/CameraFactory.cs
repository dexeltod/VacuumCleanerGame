using Cinemachine;
using Sources.Infrastructure.Factories.Player;
using Sources.InfrastructureInterfaces.Scene;
using Sources.ServicesInterfaces;
using Sources.Utils.Configs.Scripts;
using Sources.Utils.ConstantNames;
using UnityEngine;

namespace Sources.Infrastructure.Factories.Scene
{
	public class CameraFactory : ICameraFactory
	{
		private const string MainCameraPath = "MainCamera";
		private const string VirtualCameraPath = "VirtualCamera";

		private readonly IAssetResolver _assetResolver;
		private readonly IPlayerFactory _playerFactory;

		private GameObject _characterObject;
		public Camera Camera { get; private set; }

		public CameraFactory(IAssetResolver assetResolver, IPlayerFactory playerFactory)
		{
			_assetResolver = assetResolver;
			_playerFactory = playerFactory;
		}

		public CinemachineVirtualCamera CreateVirtualCamera()
		{
			_characterObject = _playerFactory.Player;
			Camera = _assetResolver.InstantiateAndGetComponent<Camera>(ResourcesAssetPath.Scene.MainCamera);

			return GetVirtualCamera();
		}

		private CinemachineVirtualCamera GetVirtualCamera()
		{
			Collider bounds = GameObject.FindWithTag(ConstantNames.Confiner).GetComponent<Collider>();

			GameObject camera = _assetResolver.Instantiate(ResourcesAssetPath.Scene.CinemachineVirtualCamera);
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