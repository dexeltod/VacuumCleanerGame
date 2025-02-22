using System;
using Sources.BusinessLogic.ServicesInterfaces.Advertisement;
using Sources.Controllers.Common;
using Sources.ControllersInterfaces;
using UnityEngine;

namespace Sources.Controllers
{
	public class AdvertisementPresenter : Presenter, IAdvertisementPresenter
	{
		private readonly IAdvertisement _advertisement;

		public AdvertisementPresenter(IAdvertisement advertisement) =>
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

		private void OnAdOpened()
		{
			Debug.Log("OnAdOpened");
			AudioListener.volume = 0;
			Time.timeScale = 0;
		}

		private void OnClosed()
		{
			Debug.Log("ad closed/rewarded");
			AudioListener.volume = 1;
			Time.timeScale = 1;
		}

		private void OnRewarded() => OnClosed();
	}
}