
using UnityEngine;

namespace Sources.ServicesInterfaces
{
	public interface IAssetProvider 
	{
		GameObject Instantiate(string path);
		T LoadComponent<T>(string path);
		T LoadFromResources<T>(string path) where T : Object;
		GameObject Instantiate(string path, Vector3 position);
		T InstantiateAndGetComponent<T>(string path) where T : Behaviour;
		T InstantiateAndGetComponent<T>(string path, Vector3 position) where T : Behaviour;
	}
}