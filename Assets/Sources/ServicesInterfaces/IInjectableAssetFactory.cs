using UnityEngine;

namespace Sources.ServicesInterfaces
{
	public interface IInjectableAssetFactory : IAssetFactory
	{
		T InstantiateAndGetComponent<T>(string path, Transform position) where T : Behaviour;
	}
}