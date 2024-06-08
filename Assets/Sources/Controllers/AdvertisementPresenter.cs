using System;
using Sources.Controllers.Common;
using Sources.ControllersInterfaces;
using Sources.ServicesInterfaces.Advertisement;
using UnityEngine;

namespace Sources.Controllers
{
	public sealed class AdvertisementHandler : Presenter, IAdvertisementHandler
	{
		private readonly IAdvertisement _advertisement;

		public AdvertisementHandler(IAdvertisement advertisement) =>
			_advertisement = advertisement ?? throw new ArgumentNullException(nameof(advertisement));

		public override void Enable()
		{
			_advertisement.Closed += OnClosed;
			_advertisement.Rewarded += OnRewarded;
			_advertisement.Opened += OnAdOpened;
		}

		public override void Disable()
		{
			_advertisement.Closed -= OnClosed;
			_advertisement.Rewarded -= OnRewarded;
			_advertisement.Opened -= OnAdOpened;
		}

		private void OnRewarded() =>
			OnClosed();

		private void OnClosed()
		{
			AudioListener.volume = 1;
			Time.timeScale = 1;
		}

		private void OnAdOpened()
		{
			AudioListener.volume = 0;
			Time.timeScale = 0;
		}
	}
}