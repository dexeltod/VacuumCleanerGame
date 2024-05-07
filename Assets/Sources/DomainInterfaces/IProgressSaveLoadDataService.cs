using System;
using Cysharp.Threading.Tasks;

namespace Sources.DomainInterfaces
{
	public interface IProgressSaveLoadDataService
	{
		void SaveToJson(string fileName, object data);
		string LoadFromJson(string fileName);
		T LoadFromJson<T>(string fileName);
		UniTask SaveToCloud(IGlobalProgress progress, Action succeededCallback = null);
		UniTask SaveToCloud(Action succeededCallback = null);
		UniTask<IGlobalProgress> LoadFromCloud();
		UniTask ClearSaves();
	}
}