using System;
using Sources.DomainInterfaces.DomainServicesInterfaces;
using Sources.InfrastructureInterfaces.Providers;
using Sources.ServicesInterfaces;

namespace Sources.Controllers
{
	public class ShopPurchaseController
	{
		private readonly IProgressService _progressService;
		private readonly IPersistentProgressServiceProvider _persistentProgressServiceProvider;
		private readonly ISaveLoader _saveLoader;

		public ShopPurchaseController(
			IProgressService progressService,
			IPersistentProgressServiceProvider persistentProgressServiceProvider,
			ISaveLoader saveLoader
		)
		{
			_progressService = progressService ?? throw new ArgumentNullException(nameof(progressService));
			_persistentProgressServiceProvider = persistentProgressServiceProvider ??
				throw new ArgumentNullException(nameof(persistentProgressServiceProvider));
			_saveLoader = saveLoader ?? throw new ArgumentNullException(nameof(saveLoader));
		}

		private int Money =>
			_persistentProgressServiceProvider.Self.GlobalProgress.ResourceModelReadOnly
				.SoftCurrency.Value;

		public bool TryAddOneProgressPoint(int id)
		{
			var price = _progressService.GetPrice(id);

			if (Money - price < 0)
				throw new InvalidOperationException($"Not enough money. Need {price} but have {Money}");

			_progressService.AddProgressPoint(id);
			return true;
		}
	}
}