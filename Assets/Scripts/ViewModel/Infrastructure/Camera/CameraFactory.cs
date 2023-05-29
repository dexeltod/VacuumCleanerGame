using System.Threading.Tasks;
using Cinemachine;
using Model;
using Model.Configs;
using Model.DI;
using UnityEngine;
using ViewModel.Infrastructure.Services;
using ViewModel.Infrastructure.Services.Factories;
using ViewModel.Infrastructure.Services.Factories.Player;

namespace ViewModel.Infrastructure.Camera
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
			_characterObject = _playerFactory.MainCharacter;
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