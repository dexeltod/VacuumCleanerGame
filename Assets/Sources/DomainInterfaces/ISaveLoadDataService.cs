#if !YANDEX_GAMES
using System.Threading.Tasks;
#endif
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Sources.DIService;

namespace Sources.DomainInterfaces
{
	public interface ISaveLoadDataService : IService
	{
#if !YANDEX_GAMES
		void SaveProgressBinary();
		IGameProgressModel LoadProgressBinary();
#endif
		void SaveToJson(string fileName, object data);
		string LoadFromJson(string fileName);
		T LoadFromJson<T>(string fileName);
#if !YANDEX_GAMES
		void SaveToUnityCloud();
		UniTask<IGameProgressModel> LoadFromUnityCloud();
#endif
#if YANDEX_GAMES && !UNITY_EDITOR
		IGameProgressModel LoadFromYandex();
#endif
	}
}