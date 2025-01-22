using System.Threading.Tasks;
using Sources.DomainInterfaces;

namespace Sources.BusinessLogic.Interfaces.Factory
{
	public interface IProgressFactory
	{
		Task<IGlobalProgress> Create();
	}
}