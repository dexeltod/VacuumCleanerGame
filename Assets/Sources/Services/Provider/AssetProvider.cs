using System;
using Sources.ServicesInterfaces;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Sources.Services
{
	public class AssetProvider : IAssetProvider
	{
		public GameObject Instantiate(string path)
		{
			GameObject @object = Resources.Load<GameObject>(path) ??
				throw new ArgumentNullException(path);
			CheckPathException(path, @object);
			return Object.Instantiate(@object);
		}

		public T LoadComponent<T>(string path)
		{
			GameObject @object = Resources.Load<GameObject>(path) ?? throw new ArgumentNullException(path);
			CheckPathException(path, @object);
			return @object.GetComponent<T>();
		}

		public T InstantiateAndGetComponent<T>(string path) where T : Behaviour
		{
			T resource = Resources.Load<T>(path) ?? throw new ArgumentNullException(path);
			T @object = Object.Instantiate(resource);
			CheckPathException(path, @object);
			return @object;
		}

		public T InstantiateAndGetComponent<T>(string path, Vector3 position) where T : Behaviour
		{
			T @object = Object.Instantiate(
				Resources.Load<T>(path),
				position,
				Quaternion.identity
			) ?? throw new ArgumentNullException(path);

			CheckPathException(path, @object);
			return @object;
		}

		public T LoadFromResources<T>(string path) where T : Object
		{
			T @object = Resources.Load<T>(path) ?? throw new ArgumentNullException(path);
			CheckPathException(path, @object);
			return @object;
		}

		public GameObject Instantiate(string path, Vector3 position)
		{
			GameObject @object = Resources.Load<GameObject>(path) ?? throw new ArgumentNullException(path);

			CheckPathException(path, @object);
			return Object.Instantiate(
				@object,
				position,
				Quaternion.identity
			);
		}

		private void CheckPathException(string path, object @object)
		{
			if (string.IsNullOrEmpty(path))
				throw new ArgumentNullException(nameof(path));

			if (@object == null)
				throw new ArgumentNullException(nameof(@object) + path);
		}
	}
}