using System;
using Sources.Controllers.Common;
using Sources.PresentationInterfaces;
using Sources.ServicesInterfaces.DTO;

namespace Sources.Controllers
{
	public class UpgradeElementPresenter : Presenter, IUpgradeElementPresenter
	{
		private readonly IPlayerProgressSetterFacade _playerProgressSetterFacade;
		private readonly IColorChangeable _button;

		public UpgradeElementPresenter(
			IPlayerProgressSetterFacade playerProgressSetterFacade,
			IColorChangeable button
		)
		{
			_playerProgressSetterFacade = playerProgressSetterFacade ??
				throw new ArgumentNullException(nameof(playerProgressSetterFacade));
			_button = button ?? throw new ArgumentNullException(nameof(button));
		}

		public void Upgrade(string progressIdName)
		{
			_playerProgressSetterFacade.TryAddOneProgressPoint(progressIdName);
			_button.AddProgressPointColor();
		}
	}
}