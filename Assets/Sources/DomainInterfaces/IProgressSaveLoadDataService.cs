using System;
using Cysharp.Threading.Tasks;

namespace Sources.DomainInterfaces
{
	public interface IProgressSaveLoadDataService
	{
		UniTask ClearSaves();
		UniTask<IGlobalProgress> LoadFromCloud();
		string LoadFromJson(string fileName);
		T LoadFromJson<T>(string fileName);
		UniTask SaveToCloud(Action succeededCallback = null);
		void SaveToJson(string fileName, object data);
	}
}