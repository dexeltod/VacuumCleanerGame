using Sources.DIService;
using UnityEngine;

namespace Sources.InfrastructureInterfaces
{
	public interface IPresenterFactory: IService
	{
		T Instantiate<T> (string name, Vector3 position);
		T Instantiate<T>(string name);
	}
}