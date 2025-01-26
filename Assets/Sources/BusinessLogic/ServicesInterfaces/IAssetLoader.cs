using UnityEngine;

namespace Sources.BusinessLogic.ServicesInterfaces
{
	public interface IAssetLoader
	{
		GameObject Instantiate(string path);
		GameObject Instantiate(string path, Vector3 position);
		GameObject Instantiate(GameObject instanceObject, Vector3 position);
		GameObject Instantiate(GameObject gameObject);
		GameObject Instantiate(GameObject instanceObject, Transform transform);
		T InstantiateAndGetComponent<T>(string path) where T : Behaviour;
		T InstantiateAndGetComponent<T>(string path, Vector3 position) where T : Behaviour;
		T InstantiateAndGetComponent<T>(string path, Transform transform) where T : Behaviour;
		T InstantiateAndGetComponent<T>(GameObject gameObject) where T : Behaviour;
		T InstantiateAndGetComponent<T>(GameObject gameObject, Transform transform) where T : Behaviour;
		T InstantiateAndGetComponent<T>(GameObject gameObject, Vector3 position) where T : Behaviour;
		T LoadComponent<T>(string path);
		T LoadFromResources<T>(string path) where T : Object;
	}
}