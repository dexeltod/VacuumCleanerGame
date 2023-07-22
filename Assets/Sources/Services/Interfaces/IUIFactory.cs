using System.Threading.Tasks;
using UnityEngine;

namespace Sources.View.Services.UI
{
	public interface IUIFactory : IUIGetter
	{
		Task<GameObject> CreateUI();
	}
}