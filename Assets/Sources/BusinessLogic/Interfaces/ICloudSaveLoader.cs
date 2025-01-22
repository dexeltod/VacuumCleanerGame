using Cysharp.Threading.Tasks;
using Sources.DomainInterfaces;

namespace Sources.BusinessLogic.Interfaces
{
	public interface ICloudSaveLoader
	{
		UniTask DeleteSaves(IGlobalProgress globalProgress);
		UniTask<string> Load();
		UniTask Save(string json);
	}
}