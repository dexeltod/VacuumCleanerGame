using Cysharp.Threading.Tasks;
using Sources.DomainInterfaces;

namespace Sources.Infrastructure.Factories.Player
{
	public interface IProgressFactory
	{
		UniTask<IGlobalProgress> Load();
		UniTask Save(IGlobalProgress provider);
	}
}