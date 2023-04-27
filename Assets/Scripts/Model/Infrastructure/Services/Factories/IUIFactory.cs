using System.Threading.Tasks;
using UnityEngine;

namespace Model.Infrastructure.Services.Factories
{
	public interface IUIFactory : IUIGetter
	{
		Task<GameObject> CreateUI();
	}
}