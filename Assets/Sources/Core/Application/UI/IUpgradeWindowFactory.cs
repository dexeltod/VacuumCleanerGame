using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Sources.View.Services.UI
{
	public interface IUpgradeWindowFactory : IUpgradeWindowGetter
	{
		UniTask<GameObject> Create();
	}
}