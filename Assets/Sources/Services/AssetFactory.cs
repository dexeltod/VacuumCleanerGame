using System;
using Sources.ServicesInterfaces;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Sources.Services
{
	public sealed class AssetFactory : IAssetFactory
	{
		public GameObject Instantiate(string path)
		{
			GameObject @object = Resources.Load<GameObject>(path) ?? throw new ArgumentNullException(path);

			CheckPathException(path, @object);

			GameObject gameObject = Object.Instantiate(@object);

			return gameObject;
		}

		public T InstantiateAndGetComponent<T>(string path, Transform transform) where T : Behaviour
		{
			T resource = Resources.Load<T>(path) ?? throw new ArgumentNullException(path);
			T @object = Object.Instantiate(resource, transform);

			CheckPathException(path, @object);
			return @object;
		}

		public GameObject Instantiate(GameObject gameObject)
		{
			GameObject instantiated = Object.Instantiate(gameObject);

			return instantiated;
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

		public T InstantiateAndGetComponent<T>(GameObject gameObject) where T : Behaviour
		{
			T component = Object.Instantiate(gameObject).GetComponent<T>();

			return component;
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

			GameObject gameObject = Object.Instantiate(
				@object,
				position,
				Quaternion.identity
			);

			return gameObject;
		}

		public GameObject Instantiate(GameObject instanceObject, Vector3 position)
		{
			GameObject gameObject = Object.Instantiate(
				instanceObject,
				position,
				Quaternion.identity
			);

			return gameObject;
		}

		public GameObject Instantiate(GameObject instanceObject, Transform transform)
		{
			GameObject gameObject = Object.Instantiate(instanceObject, transform);

			return gameObject;
		}

		private void CheckPathException(string path, object @object)
		{
			if (string.IsNullOrEmpty(path))
				throw new ArgumentNullException($"File from resource not found:" + nameof(path));

			if (@object == null)
				throw new ArgumentNullException(nameof(@object) + path);
		}
	}
}