using Cysharp.Threading.Tasks;
using Sources.DomainInterfaces;

namespace Sources.Services.DomainServices
{
	public interface IProgressCleaner
	{
		UniTask<IGlobalProgress> ClearAndSaveCloud();
	}
}