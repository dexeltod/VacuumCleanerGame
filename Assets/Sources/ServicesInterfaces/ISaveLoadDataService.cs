using Sources.DIService;
using Sources.DomainInterfaces;

namespace Sources.ServicesInterfaces
{
	public interface ISaveLoadDataService : IService
	{
		void SaveProgress();
		IGameProgressModel LoadProgress();
		void SaveToJson(string fileName, object data);
		string LoadFromJson(string fileName);
		T LoadFromJson<T>(string fileName);
	}
}