using System;
using System.Threading.Tasks;
using UnityEngine;

namespace Model
{
	public class UIFactory : IUIFactory
	{
		private const string UserInterface = "UI";
		private readonly IAssetProvider _assetProvider;

		public event Action UICreated;

		public UIFactory()
		{
			_assetProvider = ServiceLocator.Container.GetSingle<IAssetProvider>();
		}
		
		public async Task<GameObject> CreateUI()
		{
			var instance = await _assetProvider.Instantiate(UserInterface);
			UICreated?.Invoke();
			return instance;
		}
	}
}