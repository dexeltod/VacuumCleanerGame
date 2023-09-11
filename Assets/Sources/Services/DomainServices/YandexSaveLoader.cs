using Cysharp.Threading.Tasks;
using Sources.ApplicationServicesInterfaces;
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
			_yandexController = yandexController;
		}
		
		public void Save(IGameProgressModel @object)
		{
			string json = JsonUtility.ToJson(@object);

			_yandexController.Save(json);
		}

		public async UniTask<IGameProgressModel> Load()
		{
			string json = await _yandexController.Load();
			return JsonUtility.FromJson<IGameProgressModel>(json);
		}
	}
}