using Sources.ServicesInterfaces;
using UnityEngine;

namespace Sources.Services
{
	public class AssetProvider : IAssetProvider
	{
		public GameObject Instantiate(string path)
		{
			GameObject @object = Resources.Load<GameObject>(path);
			return Object.Instantiate(@object);
		}

		public T LoadComponent<T>(string path)
		{
			GameObject @object = Resources.Load<GameObject>(path);
			return @object.GetComponent<T>();
		}

		public T InstantiateAndGetComponent<T>(string path) where T : Behaviour =>
			Object.Instantiate(Resources.Load<T>(path));

		public T InstantiateAndGetComponent<T>(string path, Vector3 position) where T : Behaviour =>
			Object.Instantiate(Resources.Load<T>(path), position, Quaternion.identity);

		public T Load<T>(string path) where T : Object =>
			Resources.Load<T>(path);

		public GameObject Instantiate(string path, Vector3 position)
		{
			GameObject @object = Resources.Load<GameObject>(path);
			return Object.Instantiate(@object, position, Quaternion.identity);
		}
	}
}