using System;
using Sources.BusinessLogic.ServicesInterfaces;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using Object = UnityEngine.Object;

namespace Sources.Boot.Scripts.Factories
{
	public sealed class InjectableAssetLoader : IInjectableAssetLoader
	{
		[Inject] private IObjectResolver _objectResolver;

		public GameObject Instantiate(string path)
		{
			ValidatePath(path);

			GameObject @object = Resources.Load<GameObject>(path) ?? throw new ArgumentNullException(path);

			ValidateObject(@object);

			GameObject gameObject = Object.Instantiate(@object);

			_objectResolver.Inject(gameObject);

			return gameObject;
		}

		public GameObject Instantiate(GameObject gameObject)
		{
			GameObject instantiated = Object.Instantiate(gameObject);

			_objectResolver.InjectGameObject(instantiated);

			return instantiated;
		}

		public T LoadComponent<T>(string path)
		{
			ValidatePath(path);

			GameObject @object = Resources.Load<GameObject>(path) ?? throw new ArgumentNullException(path);
			ValidateObject(@object);
			_objectResolver.Inject(@object);
			return @object.GetComponent<T>();
		}

		public T InstantiateAndGetComponent<T>(string path) where T : Behaviour
		{
			ValidatePath(path);

			T resource = Resources.Load<T>(path) ?? throw new ArgumentNullException(path);
			T @object = Object.Instantiate(resource);

			_objectResolver.InjectGameObject(@object.gameObject);
			ValidateObject(@object);
			return @object;
		}

		public T InstantiateAndGetComponent<T>(GameObject gameObject) where T : Behaviour
		{
			var component = Object.Instantiate(gameObject).GetComponent<T>();

			_objectResolver.InjectGameObject(component.gameObject);
			return component;
		}

		public GameObject Instantiate(GameObject instanceObject, Transform transform)
		{
			GameObject gameObject = Object.Instantiate(
				instanceObject,
				transform
			);

			_objectResolver.Inject(gameObject);

			return gameObject;
		}

		public T InstantiateAndGetComponent<T>(string path, Vector3 position) where T : Behaviour
		{
			ValidatePath(path);

			T @object = Object.Instantiate(Resources.Load<T>(path), position, Quaternion.identity)
			            ?? throw new ArgumentNullException(path);
			ValidateObject(@object);
			_objectResolver.InjectGameObject(@object.gameObject);

			return @object;
		}

		public T InstantiateAndGetComponent<T>(string path, Transform position) where T : Behaviour
		{
			ValidatePath(path);

			T loaded = Resources.Load<T>(path) ?? throw new ArgumentNullException(path);

			T @object = Object.Instantiate(loaded, position) ?? throw new ArgumentNullException(path);

			ValidateObject(@object);

			_objectResolver.InjectGameObject(@object.gameObject);

			return @object;
		}

		public T LoadFromResources<T>(string path) where T : Object
		{
			ValidatePath(path);

			T @object = Resources.Load<T>(path) ?? throw new ArgumentNullException(path);

			ValidateObject(@object);
			_objectResolver.Inject(@object);
			return @object;
		}

		public T InstantiateAndGetComponent<T>(GameObject gameObject, Vector3 position, Quaternion quaternion)
		{
			throw new NotImplementedException();
        }

		public T InstantiateAndGetComponent<T>(GameObject gameObject, Transform transform) where T : Behaviour =>
			throw new NotImplementedException();

		public T InstantiateAndGetComponent<T>(GameObject gameObject, Vector3 position) where T : Behaviour =>
			throw new NotImplementedException();

		public GameObject Instantiate(string path, Vector3 position)
		{
			GameObject @object = Resources.Load<GameObject>(path) ?? throw new ArgumentNullException(path);

			ValidateObject(@object);

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

		private void ValidateObject(object @object)
		{
			if (@object == null)
				throw new ArgumentNullException($"{nameof(@object)} is null.");
		}

		private void ValidatePath(string path)
		{
			if (string.IsNullOrEmpty(path))
				throw new ArgumentNullException("File from resource not found:" + nameof(path));
		}
	}
}
