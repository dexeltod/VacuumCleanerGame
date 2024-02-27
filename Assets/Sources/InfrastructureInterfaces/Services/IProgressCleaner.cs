using Cysharp.Threading.Tasks;
using Sources.DomainInterfaces;

namespace Sources.InfrastructureInterfaces.Services
{
	public interface IProgressCleaner
	{
		UniTask<IGlobalProgress> ClearAndSaveCloud();
	}
}