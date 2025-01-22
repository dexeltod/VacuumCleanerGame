using UnityEngine;

namespace Sources.BusinessLogic.ServicesInterfaces
{
	public interface IInjectableAssetLoader : IAssetLoader
	{
		T InstantiateAndGetComponent<T>(string path, Transform position) where T : Behaviour;
	}
}