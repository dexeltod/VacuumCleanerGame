using System;
using Cysharp.Threading.Tasks;
using Model.DI;
using Presenter.SceneEntity;
using UnityEngine;

namespace Model.Infrastructure.Services.Factories
{
	public class UpgradeWindowFactory : IUpgradeWindowFactory
	{
		private GameObject _upgradeWindowGameObject;
		private readonly IAssetProvider _assetProvider;

		public UpgradeWindowFactory()
		{
			_assetProvider = ServiceLocator.Container.GetSingle<IAssetProvider>();
		}

		public UpgradeWindow GetUpgradeWindow()
		{
			if (_upgradeWindowGameObject != null)
				return _upgradeWindowGameObject.GetComponent<UpgradeWindow>();
			
			throw new InvalidOperationException();
		}

		public async UniTask<GameObject> Create()
		{
			_upgradeWindowGameObject = await _assetProvider.Instantiate(ConstantNames.UIElementNames.UpgradeWindow);
			return _upgradeWindowGameObject;
		}
	}
}