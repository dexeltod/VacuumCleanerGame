using System.Threading.Tasks;
using Sources.DomainInterfaces;

namespace Sources.InfrastructureInterfaces.Factory
{
	public interface IProgressFactory
	{
		Task<IGlobalProgress> Create();
	}
}