using System.Threading.Tasks;
using UnityEngine;

namespace InfrastructureInterfaces
{
	public interface IUIFactory : IUIGetter
	{
		Task<GameObject> CreateUI();
	}
}