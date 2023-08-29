using Cinemachine;
using Sources.Application.Utils.Configs;
using Sources.DIService;
using Sources.Infrastructure.Factories.Player;
using Sources.InfrastructureInterfaces.Scene;
using Sources.ServicesInterfaces;
using UnityEngine;

namespace Sources.Infrastructure.Factories
{
	public class CameraFactory : ICameraFactory
	{
		private const string MainCameraPath = "MainCamera";
		private const string VirtualCameraPath = "VirtualCamera";
		private readonly IAssetProvider _assetProvider;
		private IPlayerFactory _playerFactory;

		private GameObject _characterObject;
		public Camera Camera { get; private set; }

		public CameraFactory()
		{
			_assetProvider = GameServices.Container.Get<IAssetProvider>();
		}

		public CinemachineVirtualCamera CreateVirtualCamera()
		{
			_playerFactory = GameServices.Container.Get<IPlayerFactory>();
			_characterObject = _playerFactory.Player;
			Camera = _assetProvider.InstantiateAndGetComponent<Camera>(ResourcesAssetPath.Scene.MainCamera);

			return GetVirtualCamera();
		}

		private CinemachineVirtualCamera GetVirtualCamera()
		{
			Collider bounds = GameObject.FindWithTag(ConstantNames.Confiner).GetComponent<Collider>();

			GameObject camera = _assetProvider.Instantiate(ResourcesAssetPath.Scene.CinemachineVirtualCamera);
			var cinemachineConfiner = camera.GetComponent<CinemachineConfiner>();
			
			var virtualCamera = SetCameraBounds(cinemachineConfiner, bounds, camera);

			virtualCamera.Follow = _characterObject.transform;
			return virtualCamera;
		}

		private CinemachineVirtualCamera SetCameraBounds(CinemachineConfiner cinemachineConfiner, Collider bounds,
			GameObject camera)
		{
			cinemachineConfiner.m_BoundingVolume = bounds;
			var virtualCamera = camera.GetComponent<CinemachineVirtualCamera>();
			return virtualCamera;
		}
	}
}