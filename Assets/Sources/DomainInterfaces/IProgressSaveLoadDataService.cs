using System;
using Cysharp.Threading.Tasks;

namespace Sources.DomainInterfaces
{
	public interface IProgressSaveLoadDataService
	{
		void SaveToJson(string fileName, object data);
		string LoadFromJson(string fileName);
		T LoadFromJson<T>(string fileName);
		event Func<IGameProgressProvider> ProgressCleared;
		UniTask SaveToCloud(IGameProgressProvider provider, Action succeededCallback = null);
		UniTask SaveToCloud(Action succeededCallback = null);
		UniTask<IGameProgressProvider> LoadFromCloud();
		UniTask ClearSaves();
	}
}