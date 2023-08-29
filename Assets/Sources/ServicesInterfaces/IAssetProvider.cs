using Sources.DIService;
using UnityEngine;

namespace Sources.ServicesInterfaces
{
	public interface IAssetProvider : IService
	{
		GameObject Instantiate(string path);
		T LoadComponent<T>(string path);
		T Load<T>(string path) where T: Object;
		GameObject Instantiate(string path, Vector3 position);
		T InstantiateAndGetComponent<T>(string path) where T: Behaviour;
	}
}