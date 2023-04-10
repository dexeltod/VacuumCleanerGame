using System.Threading.Tasks;
using UnityEngine;

namespace Model
{
	public class CameraFactory : ICameraFactory
	{
		private const string Path = "Camera";
		private readonly IAssetProvider _assetProvider;

		public Camera Camera { get; private set; }

		public CameraFactory()
		{
			_assetProvider = ServiceLocator.Container.GetSingle<IAssetProvider>();
		}

		public async Task<GameObject> CreateCamera()
		{
			GameObject cameraObject = await _assetProvider.Instantiate(Path);
			Camera = cameraObject.GetComponent<Camera>();
			return cameraObject;
		}
	}
}