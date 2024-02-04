using System;
using Sources.InfrastructureInterfaces.Common.Providers;

namespace Sources.Infrastructure.Common
{
	public abstract class Provider<T> : IProvider<T>
	{
		private T _instance;

		public T Instance
		{
			get
			{
				if (_instance == null)
					throw new NullReferenceException($"{nameof(_instance)} is null");

				return _instance;
			}
			protected set => _instance = value;
		}

		public abstract void Register(T instance);
	}
}