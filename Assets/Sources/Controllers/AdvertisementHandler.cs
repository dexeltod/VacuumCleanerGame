using System;
using Sources.Controllers.Common;
using Sources.ServicesInterfaces.Advertisement;
using UnityEngine;

namespace Sources.Infrastructure.Providers
{
	public sealed class AdvertisementHandler : Presenter, IAdvertisementHandler
	{
		private readonly IAdvertisement _advertisement;

		public AdvertisementHandler(IAdvertisement advertisement) =>
			_advertisement = advertisement ?? throw new ArgumentNullException(nameof(advertisement));

		public override void Enable() =>
			_advertisement.Opened += OnAdOpened;

		public override void Disable() =>
			_advertisement.Opened -= OnAdOpened;

		private void OnAdOpened()
		{
			AudioListener.volume = 0;
			Time.timeScale = 0;
		}
	}
}