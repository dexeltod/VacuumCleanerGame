using Cysharp.Threading.Tasks;

namespace Sources.DomainInterfaces.DomainServicesInterfaces
{
	public interface ISaveLoader
	{
		void Save(IGameProgressModel @object);
		UniTask<IGameProgressModel> Load();
	}
}