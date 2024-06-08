using UnityEngine;

namespace Sources.ServicesInterfaces
{
	public interface IInjectabeAssetFactory
	{
		GameObject Instantiate(string path);
		T LoadComponent<T>(string path);
		T LoadFromResources<T>(string path) where T : Object;
		GameObject Instantiate(string path, Vector3 position);
		GameObject Instantiate(GameObject instanceObject, Vector3 position);
		T InstantiateAndGetComponent<T>(string path) where T : Behaviour;
		T InstantiateAndGetComponent<T>(string path, Vector3 position) where T : Behaviour;
		GameObject Instantiate(GameObject gameObject);
		T InstantiateAndGetComponent<T>(GameObject gameObject) where T : Behaviour;
	}
}