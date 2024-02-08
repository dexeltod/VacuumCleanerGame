using System;
using Sources.ServicesInterfaces;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using Object = UnityEngine.Object;

namespace Sources.Services.Providers
{
	public class AssetFactory : IAssetFactory
	{
		[Inject] private IObjectResolver _objectResolver;

		public GameObject Instantiate(string path)
		{
			GameObject @object = Resources.Load<GameObject>(path) ?? throw new ArgumentNullException(path);

			CheckPathException(path, @object);

			GameObject gameObject = Object.Instantiate(@object);

			_objectResolver.Inject(gameObject);

			return gameObject;
		}

		public T LoadComponent<T>(string path)
		{
			GameObject @object = Resources.Load<GameObject>(path) ?? throw new ArgumentNullException(path);
			CheckPathException(path, @object);
			_objectResolver.Inject(@object);
			return @object.GetComponent<T>();
		}

		public T InstantiateAndGetComponent<T>(string path) where T : Behaviour
		{
			T resource = Resources.Load<T>(path) ?? throw new ArgumentNullException(path);
			T @object = Object.Instantiate(resource);

			_objectResolver.InjectGameObject(@object.gameObject);
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

			_objectResolver.InjectGameObject(@object.gameObject);

			CheckPathException(path, @object);
			return @object;
		}

		public T LoadFromResources<T>(string path) where T : Object
		{
			T @object = Resources.Load<T>(path) ?? throw new ArgumentNullException(path);
			CheckPathException(path, @object);
			_objectResolver.Inject(@object);
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

			_objectResolver.Inject(@object);

			return gameObject;
		}

		public GameObject Instantiate(GameObject instanceObject, Vector3 position)
		{
			GameObject gameObject = Object.Instantiate(
				instanceObject,
				position,
				Quaternion.identity
			);

			_objectResolver.Inject(gameObject);

			return gameObject;
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