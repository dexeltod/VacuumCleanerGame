using Cysharp.Threading.Tasks;

namespace Sources.DomainInterfaces
{
	public interface IProgressSaveLoadDataService
	{
		void SaveToJson(string fileName, object data);
		string LoadFromJson(string fileName);
		T LoadFromJson<T>(string fileName);
		UniTask SaveToCloud(IGlobalProgress progress);
		UniTask SaveToCloud();
		UniTask<IGlobalProgress> LoadFromCloud();
		UniTask ClearSaves();
	}
}