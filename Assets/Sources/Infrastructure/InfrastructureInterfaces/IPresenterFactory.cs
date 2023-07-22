using Cysharp.Threading.Tasks;
using Sources.Core;
using UnityEngine;

namespace Sources.Infrastructure.InfrastructureInterfaces
{
	public interface IPresenterFactory: IService
	{
		UniTask<T> Instantiate<T> (string name, Vector3 position);
		UniTask<T> Instantiate<T>(string name);
	}
}