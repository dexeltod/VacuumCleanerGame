using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Sources.Services.Interfaces
{
	public interface IUIFactory : IUIGetter
	{
		UniTask<GameObject> CreateUI();
	}
}