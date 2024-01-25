using System;
using Cysharp.Threading.Tasks;
using Sources.DomainInterfaces.DomainServicesInterfaces;

namespace Sources.DomainInterfaces
{
	public interface IProgressLoadDataService
	{
		void SaveProgressBinary();
		IGameProgressModel LoadProgressBinary();

		void SaveToJson(string fileName, object data);
		string LoadFromJson(string fileName);
		T LoadFromJson<T>(string fileName);
		event Func<IGameProgressModel> ProgressCleared;
		UniTask SaveToCloud(IGameProgressModel model, Action succeededCallback = null);
		UniTask SaveToCloud(Action succeededCallback = null);
		UniTask<IGameProgressModel> LoadFromCloud();
		UniTask ClearSaves();
	}
}