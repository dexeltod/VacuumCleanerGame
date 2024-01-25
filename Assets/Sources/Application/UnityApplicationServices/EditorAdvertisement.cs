using System;
using Cysharp.Threading.Tasks;
using Sources.Application.YandexSDK;
using UnityEngine;

namespace Sources.Application.UnityApplicationServices
{
	public class EditorAdvertisement : IAdvertisement
	{
		public UniTask ShowAd(Action onOpenCallback, Action onRewardsCallback, Action onCloseCallback)
		{
			Debug.Log("ShowAd");
			return UniTask.CompletedTask;
		}
	}
}