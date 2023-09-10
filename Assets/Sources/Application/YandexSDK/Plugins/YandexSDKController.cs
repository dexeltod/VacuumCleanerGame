using System.Collections;
using System.Runtime.InteropServices;
using Agava.YandexGames;
using Cysharp.Threading.Tasks;
using Sources.ApplicationServicesInterfaces;
using UnityEngine;

namespace Sources.Application.YandexSDK
{
	// public class YandexSDKController : IYandexSDKController
	// {
	// 	private bool _initialized = false;
	// 	private const string DLLName = "__Internal";
	//
	// 	[DllImport("__Internal")]
	// 	private static extern void YandexGamesSdkInitialize();
	//
	// 	public IEnumerator Initialize() => 
	// 		YandexGamesSdk.Initialize();
	//
	// 	public void Start() => InitializeSDK();
	//
	// 	private async void InitializeSDK()
	// 	{
	// 		while (_initialized == false)
	// 		{
	// 			YandexGamesSdkInitialize();
	// 			await UniTask.Yield();
	// 		}
	//
	// 		Debug.Log($"Yandex Initialized: {_initialized}");
	// 	}
	// }
}