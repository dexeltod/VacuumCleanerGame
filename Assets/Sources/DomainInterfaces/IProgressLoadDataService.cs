
using System;
using Cysharp.Threading.Tasks;
using Sources.DIService;

namespace Sources.DomainInterfaces
{
	public interface IProgressLoadDataService : IProgressClearable, IService
	{
		void SaveProgressBinary();
		IGameProgressModel LoadProgressBinary();

		void SaveToJson(string fileName, object data);
		string LoadFromJson(string fileName);
		T LoadFromJson<T>(string fileName);
		UniTask SaveToCloud(IGameProgressModel model, Action succeededCallback = null);
		UniTask SaveToCloud(Action succeededCallback = null);
		UniTask<IGameProgressModel> LoadFromCloud();
		UniTask ClearSaves();
	}
}