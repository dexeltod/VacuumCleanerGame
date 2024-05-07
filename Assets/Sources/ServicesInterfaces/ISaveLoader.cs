using Cysharp.Threading.Tasks;

namespace Sources.DomainInterfaces.DomainServicesInterfaces
{
	public interface ISaveLoader
	{
		UniTask Save(IGlobalProgress @object);
		UniTask<IGlobalProgress> Load();
		UniTask Initialize();
	}
}