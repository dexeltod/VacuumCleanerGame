using Cysharp.Threading.Tasks;

namespace Sources.InfrastructureInterfaces
{
	public interface ILeaderFactory
	{
		UniTask Create();
	}
}