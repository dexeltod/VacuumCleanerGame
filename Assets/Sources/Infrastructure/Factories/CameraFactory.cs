using System.Threading.Tasks;
using Application.Configs;
using Application.DI;
using Cinemachine;
using Infrastructure.Factories.Player;
using InfrastructureInterfaces;
using UnityEngine;

namespace Infrastructure.Factories
{
	public class CameraFactory : ICameraFactory
	{
		private const string MainCameraPath = "MainCamera";
		private const string VirtualCameraPath = "VirtualCamera";
		private readonly IAssetProvider _assetProvider;
		private IPlayerFactory _playerFactory;

		private GameObject _characterObject;
		public UnityEngine.Camera Camera { get; private set; }

		public CameraFactory()
		{
			_assetProvider = ServiceLocator.Container.GetSingle<IAssetProvider>();
		}

		public async Task<CinemachineVirtualCamera> CreateVirtualCamera()
		{
			_playerFactory = ServiceLocator.Container.GetSingle<IPlayerFactory>();
			_characterObject = _playerFactory.Player;
			GameObject camera = await _assetProvider.Instantiate(MainCameraPath);
			Camera = camera.GetComponent<UnityEngine.Camera>();

			return await GetVirtualCamera();
		}

		private async Task<CinemachineVirtualCamera> GetVirtualCamera()
		{
			Collider bounds = GameObject.FindWithTag(ConstantNames.Confiner).GetComponent<Collider>();

			GameObject camera = await _assetProvider.Instantiate(VirtualCameraPath);
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