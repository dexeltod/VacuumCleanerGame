using System;
using Cysharp.Threading.Tasks;
using Sources.ServicesInterfaces.Advertisement;
using UnityEngine;

namespace Sources.Application.UnityApplicationServices
{
	public sealed class EditorAdvertisement : IAdvertisement
	{
		public event Action Opened;

		public UniTask ShowAd(Action onRewardsCallback, Action onCloseCallback)
		{
			Debug.Log("ShowAd");
			
			onRewardsCallback.Invoke();
			return UniTask.CompletedTask;
		}
	}
}