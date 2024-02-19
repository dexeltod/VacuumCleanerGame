using System;
using Sources.Controllers.Common;
using Sources.DomainInterfaces.DomainServicesInterfaces;
using Sources.InfrastructureInterfaces.Providers;
using Sources.PresentationInterfaces;
using Sources.ServicesInterfaces;
using Sources.ServicesInterfaces.Upgrade;

namespace Sources.Controllers
{
	public class UpgradeElementPresenter : Presenter, IUpgradeElementPresenter
	{
		private readonly IProgressSetterFacade _progressSetterFacade;
		private readonly IUpgradeElementChangeable _button;
		private readonly IUpgradeItemData _upgradeItemData;
		private readonly IPersistentProgressServiceProvider _persistentProgressServiceProvider;
		private readonly ISaveLoader _saveLoader;

		public UpgradeElementPresenter(
			IProgressSetterFacade progressSetterFacade,
			IUpgradeElementChangeable button,
			IUpgradeItemData upgradeItemData,
			IPersistentProgressServiceProvider persistentProgressServiceProvider
		)
		{
			_progressSetterFacade = progressSetterFacade ??
				throw new ArgumentNullException(nameof(progressSetterFacade));
			_button = button ?? throw new ArgumentNullException(nameof(button));
			_upgradeItemData = upgradeItemData ?? throw new ArgumentNullException(nameof(upgradeItemData));
			_persistentProgressServiceProvider = persistentProgressServiceProvider ??
				throw new ArgumentNullException(nameof(persistentProgressServiceProvider));
		}

		public void Upgrade()
		{
			if (_progressSetterFacade.TryAddOneProgressPoint(_upgradeItemData.IdName, _upgradeItemData) ==
				false)
				throw new InvalidOperationException();

			_button.AddProgressPointColor();
			_button.SetPriceText(_upgradeItemData.Price);
		}
	}
}