using Cysharp.Threading.Tasks;
using Sources.Core.DI;
using Sources.Infrastructure.InfrastructureInterfaces;
using Sources.Infrastructure.Services.Interfaces;
using UnityEngine;

namespace Sources.Infrastructure.Factories
{
	public class PresenterFactory : IPresenterFactory
	{
		private readonly IAssetProvider _assetProvider;

		public PresenterFactory()
		{
			_assetProvider = ServiceLocator.Container.GetSingle<IAssetProvider>();
		}

		public async UniTask<T> Instantiate<T>(string name, Vector3 position)
		{
			var gameObject = await _assetProvider.Instantiate(name, position);
			return gameObject.GetComponent<T>();
		}

		public async UniTask<T> Instantiate<T>(string name)
		{
			var gameObject = await _assetProvider.Instantiate(name);
			return gameObject.GetComponent<T>();
		}
	}
}