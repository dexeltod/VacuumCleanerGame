using System;
using Cysharp.Threading.Tasks;
using Sources.ApplicationServicesInterfaces;
using Sources.Domain.Progress;
using Sources.DomainInterfaces;
using Sources.DomainInterfaces.DomainServicesInterfaces;
using UnityEngine;

namespace Sources.Services.DomainServices
{
	public class YandexSaveLoader : ISaveLoader
	{
		private readonly IYandexSDKController _yandexController;

		public YandexSaveLoader(IYandexSDKController yandexController)
		{
			_yandexController = yandexController ?? throw new ArgumentNullException(nameof(yandexController));
		}

		public async UniTask Save(IGameProgressModel @object, Action succeededCallback)
		{
			await _yandexController.Save
			(
				JsonUtility.ToJson((GameProgressModel)@object)
			);

			succeededCallback.Invoke();
		}

		public async UniTask<IGameProgressModel> Load(Action succeededCallback)
		{
			string json = await _yandexController.Load();

			Debug.Log("JSON" + json);

			if (string.IsNullOrEmpty(json) || json == "{}")
			{
				Debug.LogError("PROGRESS IS NULL");
				return null;
			}

			try
			{
				Debug.Log("" + json);
				GameProgressModel convertedJson = JsonUtility.FromJson<GameProgressModel>(json);
				succeededCallback.Invoke();
				return convertedJson;
			}
			catch (Exception e)
			{
				Debug.LogError(e + "RETURN NULL");
				throw new InvalidCastException("Json is wrong");
			}
		}

		public async UniTask ClearSaves(IGameProgressModel gameProgressModel, Action succeededCallback)
		{
			await _yandexController.DeleteSaves(gameProgressModel);
			succeededCallback.Invoke();
		}
	}
}