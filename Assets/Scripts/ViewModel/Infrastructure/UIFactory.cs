using System;
using System.Threading.Tasks;
using Model.DI;
using Model.Infrastructure.Services;
using Model.Infrastructure.Services.Factories;
using UnityEngine;

namespace ViewModel.Infrastructure
{
	public class UIFactory : IUIFactory
	{
		private const string UserInterface = "UI";
		private readonly IAssetProvider _assetProvider;
		public Joystick Joystick { get; private set; }

		public event Action UICreated;

		public UIFactory()
		{
			_assetProvider = ServiceLocator.Container.GetSingle<IAssetProvider>();
		}
		
		public async Task<GameObject> CreateUI()
		{
			GameObject instance = await _assetProvider.Instantiate(UserInterface);
			
			Joystick = instance.GetComponentInChildren<Joystick>();
			UICreated?.Invoke();
			return instance;
		}
	}
}