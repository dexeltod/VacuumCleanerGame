using UnityEngine;

namespace Sources.Services.Interfaces
{
	public interface IUIFactory : IUIGetter
	{
		GameObject CreateUI();
	}
}