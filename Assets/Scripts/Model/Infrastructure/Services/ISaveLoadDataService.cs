using Cysharp.Threading.Tasks;
using Model.Infrastructure.Data;

namespace Model.Infrastructure.Services
{
	public interface ISaveLoadDataService : IService
	{
		void SaveProgress();
		UniTask<GameProgressModel> LoadProgress();
		void SetStartProgress();
		void SaveToJson(string fileName, object data);
		string LoadFromJson(string fileName);
		T LoadFromJson<T>(string fileName);
	}
}