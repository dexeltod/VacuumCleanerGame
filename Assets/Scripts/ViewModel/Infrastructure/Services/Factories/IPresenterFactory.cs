using Cysharp.Threading.Tasks;
using UnityEngine;

namespace ViewModel.Infrastructure.Services.Factories
{
	public interface IPresenterFactory: IService
	{
		UniTask<T> Instantiate<T> (string name, Vector3 position);
		UniTask<T> Instantiate<T>(string name);
	}
}