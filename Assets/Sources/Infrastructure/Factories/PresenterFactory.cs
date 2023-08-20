using Sources.DIService;
using Sources.InfrastructureInterfaces;
using Sources.InfrastructureInterfaces.Factory;
using Sources.ServicesInterfaces;
using UnityEngine;

namespace Sources.Infrastructure.Factories
{
	public class PresenterFactory : IPresenterFactory
	{
		private readonly IResourceProvider _assetProvider;

		public PresenterFactory()
		{
			_assetProvider = GameServices.Container.Get<IResourceProvider>();
		}

		public T Instantiate<T>(string name, Vector3 position)
		{
			var gameObject = _assetProvider.Instantiate(name, position);
			return gameObject.GetComponent<T>();
		}

		public T Instantiate<T>(string name)
		{
			var gameObject = _assetProvider.Instantiate(name);
			return gameObject.GetComponent<T>();
		}
	}
}