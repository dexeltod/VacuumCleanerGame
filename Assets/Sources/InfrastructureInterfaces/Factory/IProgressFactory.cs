using Cysharp.Threading.Tasks;
using Sources.DomainInterfaces;

namespace Sources.InfrastructureInterfaces.Factory
{
	public interface IProgressFactory
	{
		UniTask<IGlobalProgress> Load();
		UniTask Save(IGlobalProgress provider);
	}
}