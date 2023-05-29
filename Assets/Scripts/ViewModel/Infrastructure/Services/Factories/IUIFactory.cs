using System.Threading.Tasks;
using UnityEngine;

namespace ViewModel.Infrastructure.Services.Factories
{
	public interface IUIFactory : IUIGetter
	{
		Task<GameObject> CreateUI();
	}
}