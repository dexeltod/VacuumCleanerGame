using Cysharp.Threading.Tasks;
using Sources.DIService;
using Sources.DomainInterfaces;

namespace Sources.ServicesInterfaces
{
	public interface ISaveLoadDataService : IService
	{
		void SaveProgressBinary();
		IGameProgressModel LoadProgressBinary();
		void SaveToJson(string fileName, object data);
		string LoadFromJson(string fileName);
		T LoadFromJson<T>(string fileName);
		void SaveToUnityCloud();
		UniTask<IGameProgressModel> LoadFromUnityCloud();
	}
}