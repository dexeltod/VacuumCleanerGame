using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.CloudSave;
using Unity.Services.Core;
using UnityEngine;

namespace Sources.Application.UnityApplicationServices
{
	public class UnityServicesController
	{
		private const string EnvironmentName = "test";

		private readonly InitializationOptions _options;

		public UnityServicesController(InitializationOptions options)
		{
			_options = options;
		}

		public async UniTask InitializeUnityServices()
		{
			await UnityServices.InitializeAsync();
			await AuthenticationService.Instance.SignInAnonymouslyAsync();
			_options.SetProfile(EnvironmentName);

			Dictionary<string, object> data = new Dictionary<string, object> { { "MySaveKey", "HelloWorld" } };

			await CloudSaveService.Instance.Data.ForceSaveAsync(data);

			Dictionary<string, string> a =
				await CloudSaveService.Instance.Data.LoadAsync(new HashSet<string> { "MySaveKey" });

			foreach (KeyValuePair<string, string> pair in a)
				Debug.Log($"{pair.Key} {pair.Value}");
		}
	}
}