using Cysharp.Threading.Tasks;
using Sources.DomainInterfaces;

namespace Sources.ApplicationServicesInterfaces
{
	public interface ICloudSave
	{
		UniTask Save(string json);
		UniTask<string> Load();
		UniTask DeleteSaves(IGlobalProgress globalProgress);
	}
}