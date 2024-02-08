using System;
using Sources.InfrastructureInterfaces.Common.Providers;
using UnityEngine;

namespace Sources.Infrastructure.Common.Provider
{
	public abstract class Provider<T> : IDisposable, IProvider<T> where T : class
	{
		private T _instance;

		public T Instance
		{
			get
			{
				if (_instance == null)
					throw new NullReferenceException($"{nameof(_instance)} {typeof(T)} {nameof(T)}  is null");

				return _instance;
			}
			protected set => _instance = value;
		}

		public virtual void Register(T instance) =>
			_instance = instance;

		public virtual void Unregister() =>
			Instance = null;

		public void Dispose()
		{
			Debug.Log($"{nameof(T)} {typeof(T)} is disposed");
		}
	}
}