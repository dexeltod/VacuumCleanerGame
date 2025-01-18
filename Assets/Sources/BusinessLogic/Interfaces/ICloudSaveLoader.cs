using Cysharp.Threading.Tasks;
using Sources.DomainInterfaces;

namespace Sources.BuisenessLogic.Interfaces
{
	public interface ICloudSaveLoader
	{
		UniTask Save(string json);
		UniTask<string> Load();
		UniTask DeleteSaves(IGlobalProgress globalProgress);
	}
}