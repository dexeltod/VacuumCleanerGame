using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Model.Infrastructure.Services.Factories
{
	public interface IUpgradeWindowFactory : IUpgradeWindowGetter
	{
		UniTask<GameObject> Create();
	}
}