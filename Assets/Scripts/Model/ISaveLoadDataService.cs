using Cysharp.Threading.Tasks;

namespace Model
{
	public interface ISaveLoadDataService : IService
	{
		void SaveProgress();
		UniTask<GameProgress> LoadProgress();
		void SetStartProgress();
		void SaveToJson(string fileName, object data);
		string LoadFromJson(string fileName);
		T LoadFromJson<T>(string fileName);
	}
}