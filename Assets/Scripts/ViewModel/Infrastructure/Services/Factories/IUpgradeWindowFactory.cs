using Cysharp.Threading.Tasks;
using UnityEngine;

namespace ViewModel.Infrastructure.Services.Factories
{
	public interface IUpgradeWindowFactory : IUpgradeWindowGetter
	{
		UniTask<GameObject> Create();
	}
}