using Cysharp.Threading.Tasks;
using Model.Data;

namespace ViewModel.Infrastructure.Services
{
	public interface ISaveLoadDataService : IService
	{
		void SaveProgress();
		GameProgressModel LoadProgress();
		void SaveToJson(string fileName, object data);
		string LoadFromJson(string fileName);
		T LoadFromJson<T>(string fileName);
	}
}