using Cysharp.Threading.Tasks;
using UnityEngine;

namespace InfrastructureInterfaces
{
	public interface IUpgradeWindowFactory : IUpgradeWindowGetter
	{
		UniTask<GameObject> Create();
	}
}